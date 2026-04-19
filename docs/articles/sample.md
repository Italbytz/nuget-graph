# Blazor sample

The sample under `samples/Italbytz.Graph.Blazor.Sample/` demonstrates how `Italbytz.Graph` and `Italbytz.Graph.Blazor` work together in a browser application.

## What the sample shows

- step-by-step minimum spanning tree exploration
- shortest path visualization on the same viewport model
- pan, zoom, refit, and keyboard interaction in the browser
- use of the `GraphViewport` component without application-specific framework code in the package itself

## Run locally

```bash
dotnet pack ./nuget-graph.sln -c Release --output ./artifacts/packages
dotnet run --project ./samples/Italbytz.Graph.Blazor.Sample/Italbytz.Graph.Blazor.Sample.csproj
```

## Open the published sample

The published sample is available at [Live sample](../sample/index.md).

The main product documentation remains available at the site root.

## When to use the sample

Use the sample if you want to:

- see the interactive viewport before integrating the package into your own app
- understand how algorithm state is projected into UI state
- validate package behavior in a browser-hosted scenario