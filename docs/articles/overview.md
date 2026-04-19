# Package overview

The package family is intentionally split so that algorithms, contracts, and UI-specific rendering can be composed independently.

## Packages

### Italbytz.Graph.Abstractions

Use this package when you need contracts for graph types and solver-facing abstractions such as `IUndirectedGraph<TVertex, TEdge>`.

### Italbytz.Graph

Use this package for concrete graph implementations, minimum spanning tree logic, shortest path logic, and UI-neutral state or visualization models.

### Italbytz.Graph.Blazor

Use this package for reusable Blazor components such as the interactive SVG viewport and browser-side graph rendering helpers.

### Italbytz.Graph.Maui

Use this package for .NET MAUI-specific drawing and rendering helpers.

## Recommended combinations

- Algorithms only: `Italbytz.Graph.Abstractions` + `Italbytz.Graph`
- Blazor application: `Italbytz.Graph` + `Italbytz.Graph.Blazor`
- .NET MAUI application: `Italbytz.Graph` + `Italbytz.Graph.Maui`

## Next steps

Continue with [Getting started](getting-started.md), review the [Installation](installation.md) guide, or launch the [live sample](../sample/index.md).