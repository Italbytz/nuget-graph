# nuget-graph

[![Documentation](https://img.shields.io/badge/Documentation-GitHub%20Pages-blue?style=for-the-badge)](https://italbytz.github.io/nuget-graph/)

`nuget-graph` bundles the refactored `Italbytz.Graph.*` package family for reusable graph contracts, solver implementations, and .NET MAUI drawing helpers.

## Which package should I use?

- Use `Italbytz.Graph.Abstractions` for graph contracts such as `IUndirectedGraph<TVertex, TEdge>` and solver-facing abstractions.
- Use `Italbytz.Graph` for concrete graph implementations and algorithms such as shortest-path and minimum-spanning-tree helpers.
- Use `Italbytz.Graph.Maui` when you need graph drawing helpers for .NET MAUI applications.

## Quality checks

This repository is prepared for preview packaging and includes:

- solution-level restore/build/pack validation
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
dotnet pack nuget-graph.sln -c Release --output ./artifacts/packages -v minimal
```
