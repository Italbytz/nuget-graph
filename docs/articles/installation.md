# Installation

Install the packages that match your application type.

## Core graph package

```bash
dotnet add package Italbytz.Graph
```

## Blazor UI package

```bash
dotnet add package Italbytz.Graph.Blazor
```

## .NET MAUI UI package

```bash
dotnet add package Italbytz.Graph.Maui
```

## Optional abstractions package

```bash
dotnet add package Italbytz.Graph.Abstractions
```

## Local package validation

If you are validating local package changes from this repository, create the local package feed first:

```bash
dotnet pack nuget-graph.sln -c Release --output ./artifacts/packages
```

## Verify your setup

For a quick end-to-end validation, run the sample application from this repository:

```bash
dotnet run --project ./samples/Italbytz.Graph.Blazor.Sample/Italbytz.Graph.Blazor.Sample.csproj
```