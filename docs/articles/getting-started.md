# Getting started

This guide helps you choose the right packages and get to a working first integration quickly.

## Choose your scenario

### Algorithms without UI

Use `Italbytz.Graph.Abstractions` and `Italbytz.Graph` if you only need graph models and solver logic.

### Blazor application

Use `Italbytz.Graph` together with `Italbytz.Graph.Blazor` if you want interactive graph rendering in the browser.

### .NET MAUI application

Use `Italbytz.Graph` together with `Italbytz.Graph.Maui` if you want native UI rendering in a MAUI application.

## Typical workflow

1. Install the packages required for your scenario.
2. Create or load graph data in your application.
3. Use the solver APIs from `Italbytz.Graph`.
4. If needed, project results into UI state and render them with the appropriate UI package.

## Next steps

- [Installation](installation.md)
- [Package overview](overview.md)
- [Blazor sample](sample.md)
- [API reference](../api/index.md)