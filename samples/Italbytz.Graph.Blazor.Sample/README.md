# Italbytz.Graph.Blazor sample

This sample demonstrates the reusable browser viewport from `Italbytz.Graph.Blazor` together with MST state generation from `Italbytz.Graph`.

It is intended as an external-facing usage sample for package consumers who want to evaluate the browser experience before integrating the packages into their own application.

## Run locally

Pack the local preview packages first if the `artifacts/packages` folder is empty.

```bash
dotnet pack ./nuget-graph.sln -c Release --output ./artifacts/packages
dotnet run --project ./samples/Italbytz.Graph.Blazor.Sample/Italbytz.Graph.Blazor.Sample.csproj
```

## Deploy to GitHub Pages

The repository now contains a GitHub Pages workflow that publishes a combined DocFX site plus this static Blazor WebAssembly sample. The sample URL is:

```text
https://italbytz.github.io/nuget-graph/sample/
```

The main documentation stays available at:

```text
https://italbytz.github.io/nuget-graph/
```

## What it shows

- switching between a compact and a larger weighted graph
- stepwise minimum-spanning-tree exploration
- SVG pan, zoom, refit and keyboard navigation
- use of the packaged `GraphViewport` component without consumer-specific application code