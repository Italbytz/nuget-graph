using System.Collections.Generic;

namespace Italbytz.Graph.Visualization;

public enum TreeLayoutOrientation
{
    TopDown,
    LeftToRight
}

public sealed class GraphViewModel
{
    public GraphViewModel(double viewBoxWidth, double viewBoxHeight, IReadOnlyList<GraphNodeViewModel> nodes, IReadOnlyList<GraphEdgeViewModel> edges)
    {
        ViewBoxWidth = viewBoxWidth;
        ViewBoxHeight = viewBoxHeight;
        Nodes = nodes;
        Edges = edges;
    }

    public double ViewBoxWidth { get; }

    public double ViewBoxHeight { get; }

    public IReadOnlyList<GraphNodeViewModel> Nodes { get; }

    public IReadOnlyList<GraphEdgeViewModel> Edges { get; }
}

public sealed class GraphStateViewModel
{
    public GraphStateViewModel(
        string currentNodeId,
        IReadOnlyList<string> activeNodeIds,
        IReadOnlyList<string> frontierNodeIds,
        IReadOnlyList<string> exploredNodeIds,
        IReadOnlyList<string> activeEdgeKeys,
        IReadOnlyList<string> successorEdgeKeys,
        IReadOnlyList<string> directedActiveEdgeIds,
        IReadOnlyList<string> directedSuccessorEdgeIds)
    {
        CurrentNodeId = currentNodeId;
        ActiveNodeIds = activeNodeIds;
        FrontierNodeIds = frontierNodeIds;
        ExploredNodeIds = exploredNodeIds;
        ActiveEdgeKeys = activeEdgeKeys;
        SuccessorEdgeKeys = successorEdgeKeys;
        DirectedActiveEdgeIds = directedActiveEdgeIds;
        DirectedSuccessorEdgeIds = directedSuccessorEdgeIds;
    }

    public string CurrentNodeId { get; }

    public IReadOnlyList<string> ActiveNodeIds { get; }

    public IReadOnlyList<string> FrontierNodeIds { get; }

    public IReadOnlyList<string> ExploredNodeIds { get; }

    public IReadOnlyList<string> ActiveEdgeKeys { get; }

    public IReadOnlyList<string> SuccessorEdgeKeys { get; }

    public IReadOnlyList<string> DirectedActiveEdgeIds { get; }

    public IReadOnlyList<string> DirectedSuccessorEdgeIds { get; }
}

public sealed class GraphNodeViewModel
{
    public GraphNodeViewModel(string id, string label, double centerX, double centerY, double radiusX, double radiusY, bool isPartOfSolution)
    {
        Id = id;
        Label = label;
        CenterX = centerX;
        CenterY = centerY;
        RadiusX = radiusX;
        RadiusY = radiusY;
        IsPartOfSolution = isPartOfSolution;
    }

    public string Id { get; }

    public string Label { get; }

    public double CenterX { get; }

    public double CenterY { get; }

    public double RadiusX { get; }

    public double RadiusY { get; }

    public bool IsPartOfSolution { get; }
}

public sealed class GraphEdgeViewModel
{
    public GraphEdgeViewModel(
        string edgeKey,
        string source,
        string target,
        string weight,
        string pathData,
        double labelX,
        double labelY,
        bool isPartOfSolution)
    {
        EdgeKey = edgeKey;
        Source = source;
        Target = target;
        Weight = weight;
        PathData = pathData;
        LabelX = labelX;
        LabelY = labelY;
        IsPartOfSolution = isPartOfSolution;
    }

    public string EdgeKey { get; }

    public string Source { get; }

    public string Target { get; }

    public string Weight { get; }

    public string PathData { get; }

    public double LabelX { get; }

    public double LabelY { get; }

    public bool IsPartOfSolution { get; }
}

public sealed class TreeLayoutNode
{
    public TreeLayoutNode(
        string id,
        string label,
        string? parentId,
        string edgeLabel,
        bool isPartOfSolution,
        int order = 0)
    {
        Id = id;
        Label = label;
        ParentId = parentId;
        EdgeLabel = edgeLabel;
        IsPartOfSolution = isPartOfSolution;
        Order = order;
    }

    public string Id { get; }

    public string Label { get; }

    public string? ParentId { get; }

    public string EdgeLabel { get; }

    public bool IsPartOfSolution { get; }

    public int Order { get; }
}