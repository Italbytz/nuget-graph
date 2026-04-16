# nuget-graph

[![Documentation](https://img.shields.io/badge/Documentation-GitHub%20Pages-blue?style=for-the-badge)](https://italbytz.github.io/nuget-graph/)

`nuget-graph` bundles the refactored `Italbytz.Graph.*` package family for reusable graph contracts, solver implementations, and UI-specific rendering helpers for .NET MAUI and Blazor.

## Which package should I use?

- Use `Italbytz.Graph.Abstractions` for graph contracts such as `IUndirectedGraph<TVertex, TEdge>` and solver-facing abstractions.
- Use `Italbytz.Graph` for concrete graph implementations, algorithms, and UI-neutral visualization/state builders such as SVG layout projection and traversal timelines.
- Use `Italbytz.Graph.Maui` when you need graph drawing helpers for .NET MAUI applications.
- Use `Italbytz.Graph.Blazor` when you need the reusable SVG viewport, interaction logic, and state-aware rendering in Blazor.

## Sample app

- A runnable Blazor sample lives under `samples/Italbytz.Graph.Blazor.Sample/`.
- It demonstrates MST step navigation, graph-state rendering, browser pan/zoom, and the reusable `Italbytz.Graph.Blazor` viewport against graphs from `Italbytz.Graph`.
- Run it locally with `dotnet run --project samples/Italbytz.Graph.Blazor.Sample/Italbytz.Graph.Blazor.Sample.csproj`.

## Blazor integration

The current package split keeps browser rendering separate without overloading the core packages.

- Keep `Italbytz.Graph.Abstractions` UI-agnostic and focused on contracts.
- Keep `Italbytz.Graph` responsible for reusable graph models, algorithms, and UI-neutral projection helpers such as layout output or algorithm-state timelines.
- Keep `Italbytz.Graph.Maui` focused on MAUI-specific drawing and interaction helpers.
- Keep `Italbytz.Graph.Blazor` as a separate Razor Class Library for the reusable browser viewport, CSS, and JS interaction layer.

That split fits the existing package landscape well:

- consumer apps can continue to depend on `Italbytz.Graph` for algorithms only when they do not need rendering
- .NET MAUI apps can compose `Italbytz.Graph` + `Italbytz.Graph.Maui`
- Blazor apps can compose `Italbytz.Graph` + `Italbytz.Graph.Blazor`
- future UI targets such as Avalonia or WPF can follow the same pattern without turning `Italbytz.Graph` into a mixed UI package

Applied extraction path for the current prototype:

1. Keep the algorithm-state builder in `Italbytz.Graph` if it can stay free of Razor, JS interop, and consumer-specific view text.
2. Move the reusable SVG viewport, CSS, and JS pan/zoom behavior into `Italbytz.Graph.Blazor`.
3. Leave localized labels and course-specific orchestration inside the consuming application.
4. Add a small sample app under the package repo that demonstrates stateful traversal and MST rendering in Blazor.

This keeps package boundaries aligned with the existing `Italbytz.Graph.*` family and avoids coupling MAUI and browser rendering concerns into the same assembly.

## Quality checks

This repository is prepared for preview packaging and includes:

- solution-level restore/build/pack validation
- dedicated build validation for the Blazor sample app
- local preview artifacts under `artifacts/packages/`
- GitHub Actions release automation in `.github/workflows/ci.yml`

## Release model

- the graph family stays on `1.0.0-preview.*` until the wider consumer and dependency chain is fully stable
- a pushed tag such as `v1.0.0-preview.3` triggers the release-ready CI pipeline
- `dotnet pack nuget-graph.sln -c Release --output ./artifacts/packages` creates a local feed that can be used until NuGet publication is complete

## Local validation

```bash
dotnet restore nuget-graph.sln
dotnet build nuget-graph.sln -c Release -v minimal
dotnet build samples/Italbytz.Graph.Blazor.Sample/Italbytz.Graph.Blazor.Sample.csproj -c Release -v minimal
dotnet pack nuget-graph.sln -c Release --output ./artifacts/packages -v minimal
```
