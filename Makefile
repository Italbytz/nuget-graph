SHELL := /bin/bash

SOLUTION := nuget-graph.sln
CONFIGURATION ?= Debug
PACK_CONFIGURATION ?= Release
DOCFX_CONFIG := docs/docfx.json
SAMPLE_PROJECT := samples/Italbytz.Graph.Blazor.Sample/Italbytz.Graph.Blazor.Sample.csproj
SAMPLE_HTTP_URL := http://127.0.0.1:5178
SAMPLE_PUBLISH_DIR := artifacts/sample-build
PAGES_DIR := artifacts/pages
PAGES_PORT ?= 8080
PAGES_HTTP_URL := http://localhost:$(PAGES_PORT)
SAMPLE_PORT := 5178

.PHONY: help restore build test pack docs sample-run sample-watch sample-open sample-publish pages-prepare pages-serve pages-open feedback clean
.PHONY: pages-serve-open sample-stop clean-sample

help:
	@echo "Available targets:"
	@echo "  make restore          - Restore solution and tools"
	@echo "  make build            - Build the full solution"
	@echo "  make test             - Run the full test suite"
	@echo "  make pack             - Pack NuGet packages"
	@echo "  make docs             - Build the docfx site"
	@echo "  make sample-run       - Run the Blazor sample once on $(SAMPLE_HTTP_URL)"
	@echo "  make sample-watch     - Run the Blazor sample with dotnet watch on $(SAMPLE_HTTP_URL)"
	@echo "  make sample-open      - Open the running sample in the browser"
	@echo "  make sample-stop      - Stop a running local sample process on port $(SAMPLE_PORT)"
	@echo "  make clean-sample     - Remove sample build caches (bin/obj) for a true fresh start"
	@echo "  make sample-publish   - Publish the sample into $(SAMPLE_PUBLISH_DIR)"
	@echo "  make pages-prepare    - Build the combined local Pages artifact"
	@echo "  make pages-serve      - Serve the combined local Pages artifact on http://localhost:$(PAGES_PORT)"
	@echo "  make pages-open       - Open the running local Pages preview in the browser"
	@echo "  make pages-serve-open - Start a local Pages preview in the background and open it in the browser"
	@echo "  make feedback         - Fast local feedback loop: restore, build sample, docs, pages artifact"
	@echo "  make clean            - Remove build artifacts"

restore:
	@if [ -f .config/dotnet-tools.json ]; then \
		dotnet tool restore; \
	else \
		echo "No local dotnet tool manifest found - skipping dotnet tool restore"; \
	fi
	dotnet restore $(SOLUTION)

build: restore
	dotnet build $(SOLUTION) --configuration $(CONFIGURATION) --no-restore

test: restore
	dotnet test $(SOLUTION) --configuration $(CONFIGURATION) --no-restore --verbosity minimal

pack: restore
	dotnet pack $(SOLUTION) --configuration $(PACK_CONFIGURATION) --no-restore --verbosity minimal --output ./artifacts/packages

docs:
	@if command -v docfx >/dev/null 2>&1; then \
		docfx $(DOCFX_CONFIG); \
	elif [ -f .config/dotnet-tools.json ]; then \
		dotnet tool restore; \
		dotnet tool run docfx $(DOCFX_CONFIG); \
	else \
		echo "docfx not found, installing globally"; \
		dotnet tool install --global docfx; \
		export PATH="$$HOME/.dotnet/tools:$$PATH"; \
		docfx $(DOCFX_CONFIG); \
	fi

sample-run:
	ASPNETCORE_URLS=$(SAMPLE_HTTP_URL) dotnet run --project $(SAMPLE_PROJECT) --no-launch-profile

sample-watch: sample-stop clean-sample
	ASPNETCORE_URLS=$(SAMPLE_HTTP_URL) dotnet watch --project $(SAMPLE_PROJECT) run --no-launch-profile

sample-stop:
	-lsof -ti tcp:$(SAMPLE_PORT) | xargs kill

clean-sample:
	rm -rf ./samples/Italbytz.Graph.Blazor.Sample/bin ./samples/Italbytz.Graph.Blazor.Sample/obj

sample-open:
	open $(SAMPLE_HTTP_URL)

sample-publish:
	dotnet publish $(SAMPLE_PROJECT) --configuration $(PACK_CONFIGURATION) --output ./$(SAMPLE_PUBLISH_DIR)

pages-prepare: docs sample-publish
	rm -rf ./$(PAGES_DIR)
	mkdir -p ./$(PAGES_DIR)/sample
	perl -0pi -e 's#<base href="/" />#<base href="/nuget-graph/sample/" />#' ./$(SAMPLE_PUBLISH_DIR)/wwwroot/index.html
	cp ./$(SAMPLE_PUBLISH_DIR)/wwwroot/index.html ./$(SAMPLE_PUBLISH_DIR)/wwwroot/404.html
	cp -R ./docs/_site/. ./$(PAGES_DIR)
	cp -R ./$(SAMPLE_PUBLISH_DIR)/wwwroot/. ./$(PAGES_DIR)/sample
	touch ./$(PAGES_DIR)/.nojekyll
	@echo "Pages artifact ready in ./$(PAGES_DIR)"

pages-serve: pages-prepare
	cd ./$(PAGES_DIR) && python3 -m http.server $(PAGES_PORT)

pages-open:
	open $(PAGES_HTTP_URL)

pages-serve-open: pages-prepare
	cd ./$(PAGES_DIR) && nohup python3 -m http.server $(PAGES_PORT) >/tmp/nuget-graph-pages-serve.log 2>&1 &
	@sleep 1
	open $(PAGES_HTTP_URL)
	@echo "Pages preview started in the background on $(PAGES_HTTP_URL)"
	@echo "Log: /tmp/nuget-graph-pages-serve.log"

feedback: restore sample-publish docs pages-prepare
	@echo "Sample: $(SAMPLE_HTTP_URL) via 'make sample-watch' and 'make sample-open'"
	@echo "Pages preview: $(PAGES_HTTP_URL) via 'make pages-serve', 'make pages-open', or 'make pages-serve-open'"

clean:
	$(MAKE) clean-sample
	rm -rf ./artifacts/sample-build ./artifacts/pages
	dotnet clean $(SOLUTION)