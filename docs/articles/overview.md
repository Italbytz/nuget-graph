# Package overview

Die Paketfamilie ist bewusst getrennt, damit Algorithmen, Verträge und UI-spezifische Darstellung unabhängig voneinander verwendbar bleiben.

## Packages

### Italbytz.Graph.Abstractions

Verträge für Graphmodelle und Solver, zum Beispiel `IUndirectedGraph<TVertex, TEdge>` und solverbezogene Interfaces.

### Italbytz.Graph

Konkrete Graphimplementierungen, Minimum-Spanning-Tree- und Shortest-Path-Logik sowie UI-neutrale Visualisierungsmodelle.

### Italbytz.Graph.Blazor

Wiederverwendbare Blazor-Komponenten für SVG-Viewport, Interaktion und Darstellung von Algorithmuszuständen im Browser.

### Italbytz.Graph.Maui

MAUI-spezifische Zeichen- und Rendering-Helfer für native Oberflächen.

## Empfohlene Kombinationen

- Nur Algorithmen: `Italbytz.Graph.Abstractions` + `Italbytz.Graph`
- Blazor-Anwendung: `Italbytz.Graph` + `Italbytz.Graph.Blazor`
- .NET MAUI-Anwendung: `Italbytz.Graph` + `Italbytz.Graph.Maui`

## Nächster Schritt

Für eine direkte Demonstration der Browser-Komponenten siehe [Blazor sample guide](sample.md) oder starte das [Live Blazor sample](../sample/index.md).