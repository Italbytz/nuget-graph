# nuget-graph

[![Documentation](https://img.shields.io/badge/Documentation-GitHub%20Pages-blue?style=for-the-badge)](https://italbytz.github.io/nuget-graph/)

`nuget-graph` provides the `Italbytz.Graph.*` package family for graph contracts, algorithm implementations, and UI-specific rendering helpers for Blazor and .NET MAUI.

Use this repository if you want to:

- build graph-based features on reusable abstractions
- run minimum spanning tree or shortest path logic in .NET
- render graph state in Blazor or .NET MAUI applications
- explore the packages through a live browser sample

## Documentation

- Product documentation: `https://italbytz.github.io/nuget-graph/`
- Live Blazor sample: `https://italbytz.github.io/nuget-graph/sample/`

## Quick start

Install the packages you need in your application:

```bash
dotnet add package Italbytz.Graph
dotnet add package Italbytz.Graph.Blazor
```

For a Blazor application, add the reusable graph viewport from `Italbytz.Graph.Blazor` and provide graph/state data from `Italbytz.Graph`.

## Choose a package

- Use `Italbytz.Graph.Abstractions` for graph contracts such as `IUndirectedGraph<TVertex, TEdge>` and solver-facing abstractions.
- Use `Italbytz.Graph` for concrete graph implementations, algorithms, and UI-neutral visualization/state builders such as SVG layout projection and traversal timelines.
- Use `Italbytz.Graph.Maui` when you need graph drawing helpers for .NET MAUI applications.
- Use `Italbytz.Graph.Blazor` when you need the reusable SVG viewport, interaction logic, and state-aware rendering in Blazor.

## Getting started

The typical consumption model is:

1. Reference `Italbytz.Graph` for graph models and algorithms.
2. Add `Italbytz.Graph.Blazor` or `Italbytz.Graph.Maui` if you need UI rendering.
3. Build your own application-specific orchestration around the package APIs.

See the published documentation for installation guidance, package selection, sample usage, and API reference.

## Sample

- A runnable Blazor sample lives under `samples/Italbytz.Graph.Blazor.Sample/`.
- It demonstrates MST step navigation, graph-state rendering, browser pan/zoom, and the reusable `Italbytz.Graph.Blazor` viewport against graphs from `Italbytz.Graph`.
- Run it locally with `dotnet run --project samples/Italbytz.Graph.Blazor.Sample/Italbytz.Graph.Blazor.Sample.csproj`.

## Quality checks

This repository includes:

- solution-level restore/build/pack validation
- dedicated build validation for the Blazor sample app
- local preview artifacts under `artifacts/packages/`
- GitHub Actions release automation in `.github/workflows/ci.yml`

## Local validation

```bash
dotnet restore nuget-graph.sln
dotnet build nuget-graph.sln -c Release -v minimal
dotnet build samples/Italbytz.Graph.Blazor.Sample/Italbytz.Graph.Blazor.Sample.csproj -c Release -v minimal
dotnet pack nuget-graph.sln -c Release --output ./artifacts/packages -v minimal
```
