# Blazor sample

Das Sample unter `samples/Italbytz.Graph.Blazor.Sample/` demonstriert die Kombination aus Kernalgorithmen, Zustandsmodellen und wiederverwendbarem SVG-Viewport.

## Was das Sample zeigt

- schrittweise Exploration eines Minimum Spanning Tree
- Shortest-Path-Darstellung auf demselben Viewport
- Pan, Zoom, Refit und Tastaturinteraktion im Browser
- Verwendung der `GraphViewport` Komponente ohne anwendungsspezifische Speziallogik in der Bibliothek

## Lokal starten

```bash
dotnet pack ./nuget-graph.sln -c Release --output ./artifacts/packages
dotnet run --project ./samples/Italbytz.Graph.Blazor.Sample/Italbytz.Graph.Blazor.Sample.csproj
```

## Auf GitHub Pages

Das veröffentlichte Sample liegt unter [Live Blazor sample](../sample/index.md).

Die Hauptdokumentation bleibt unterhalb des Site-Root verfügbar und kann aus dem Sample wieder direkt erreicht werden.