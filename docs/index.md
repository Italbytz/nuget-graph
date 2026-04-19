# nuget-graph

`nuget-graph` bündelt die `Italbytz.Graph.*` Pakete für Graph-Abstraktionen, Algorithmen und UI-spezifische Rendering-Helfer für Blazor und .NET MAUI.

## Schnellstart

- [Package overview](articles/overview.md)
- [Blazor sample guide](articles/sample.md)
- [API reference](api/index.md)
- [Live Blazor sample](sample/index.md)

## Warum diese Site zwei Perspektiven vereint

Diese GitHub-Pages-Site besteht aus zwei komplementären Teilen:

- einer DocFX-Dokumentation für Überblick, Einstieg und API-Referenz
- einem statisch veröffentlichten Blazor WebAssembly Sample unter `sample/`

Damit sind Referenz und lauffähige Demonstration an derselben URL auffindbar, ohne das Sample von der API-Dokumentation zu entkoppeln.

## Architektur in drei Schichten

### Verträge

`Italbytz.Graph.Abstractions` definiert die Interfaces für Graphen, Kanten und Solver, damit Algorithmen und UI-Komponenten nicht voneinander abhängen.

### Algorithmen und Zustandsmodelle

`Italbytz.Graph` enthält die konkreten Implementierungen, MST- und Shortest-Path-Solver sowie die Visualisierungsmodelle, die von verschiedenen UI-Frontends verwendet werden können.

### UI-spezifische Adapter

`Italbytz.Graph.Blazor` und `Italbytz.Graph.Maui` binden dieselben Kernmodelle jeweils an Browser- oder MAUI-Oberflächen an, ohne die Algorithmen zu vermischen.

## Empfohlene Einstiege

### Ich will die Paketgrenzen verstehen

Starte mit [Package overview](articles/overview.md).

### Ich will die Komponenten live sehen

Öffne das [Live Blazor sample](sample/index.md) und springe danach zurück in die [API reference](api/index.md).

### Ich will Typen und Namespaces nachschlagen

Gehe direkt zur [API reference](api/index.md).