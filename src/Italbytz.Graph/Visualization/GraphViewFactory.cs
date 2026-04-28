using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Italbytz.Graph.Abstractions;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Miscellaneous;

namespace Italbytz.Graph.Visualization;

public static class GraphViewFactory
{
    public static GraphViewModel BuildTreeGraphView(
        IReadOnlyCollection<TreeLayoutNode> treeNodes,
        double horizontalSpacing = 120.0,
        double verticalSpacing = 140.0,
        double nodeRadius = 24.0,
        double padding = 40.0,
        double rootSpacing = 1.0,
        TreeLayoutOrientation orientation = TreeLayoutOrientation.TopDown)
    {
        if (treeNodes.Count == 0)
        {
            return new GraphViewModel(400, 280, [], []);
        }

        var orderedNodes = treeNodes
            .OrderBy(node => node.Order)
            .ThenBy(node => node.Id, StringComparer.Ordinal)
            .ToArray();

        var byId = orderedNodes.ToDictionary(node => node.Id, node => node, StringComparer.Ordinal);
        var childrenByParent = orderedNodes
            .GroupBy(node => node.ParentId ?? string.Empty, StringComparer.Ordinal)
            .ToDictionary(
                group => group.Key,
                group => group.OrderBy(node => node.Order).ThenBy(node => node.Id, StringComparer.Ordinal).ToArray(),
                StringComparer.Ordinal);

        var roots = orderedNodes
            .Where(node => string.IsNullOrEmpty(node.ParentId) || (node.ParentId is not null && !byId.ContainsKey(node.ParentId)))
            .OrderBy(node => node.Order)
            .ThenBy(node => node.Id, StringComparer.Ordinal)
            .ToArray();

        var positions = new Dictionary<string, (double X, int Depth)>(StringComparer.Ordinal);
        var nextLeafIndex = 0.0;

        foreach (var root in roots)
        {
            LayoutTreeNode(root.Id, 0, childrenByParent, positions, ref nextLeafIndex);
            nextLeafIndex += Math.Max(0.0, rootSpacing);
        }

        var nodes = orderedNodes
            .Select(node =>
            {
                var position = positions[node.Id];
                var (centerX, centerY) = ProjectTreePoint(position.X, position.Depth, horizontalSpacing, verticalSpacing, padding, orientation);
                return new GraphNodeViewModel(
                    node.Id,
                    node.Label,
                    centerX,
                    centerY,
                    nodeRadius,
                    nodeRadius,
                    node.IsPartOfSolution);
            })
            .ToArray();

        var edges = orderedNodes
            .Where(node => !string.IsNullOrEmpty(node.ParentId) && byId.ContainsKey(node.ParentId!))
            .Select(node =>
            {
                var parent = byId[node.ParentId!];
                var parentPosition = positions[parent.Id];
                var childPosition = positions[node.Id];
                var (parentX, parentY) = ProjectTreePoint(parentPosition.X, parentPosition.Depth, horizontalSpacing, verticalSpacing, padding, orientation);
                var (childX, childY) = ProjectTreePoint(childPosition.X, childPosition.Depth, horizontalSpacing, verticalSpacing, padding, orientation);
                var edgeKey = $"{parent.Id}->{node.Id}";

                return new GraphEdgeViewModel(
                    edgeKey,
                    parent.Id,
                    node.Id,
                    node.EdgeLabel,
                    $"M {parentX.ToString("0.##", CultureInfo.InvariantCulture)} {parentY.ToString("0.##", CultureInfo.InvariantCulture)} L {childX.ToString("0.##", CultureInfo.InvariantCulture)} {childY.ToString("0.##", CultureInfo.InvariantCulture)}",
                    (parentX + childX) / 2.0,
                    (parentY + childY) / 2.0,
                    node.IsPartOfSolution);
            })
            .ToArray();

        var maxX = nodes.Max(node => node.CenterX);
        var maxY = nodes.Max(node => node.CenterY);

        return new GraphViewModel(
            Math.Max(400, maxX + padding),
            Math.Max(280, maxY + padding),
            nodes,
            edges);
    }

    public static IReadOnlyList<GraphStateViewModel> BuildMinimumSpanningTreeStates(
        IUndirectedGraph<string, ITaggedEdge<string, double>> graph,
        IReadOnlyList<ITaggedEdge<string, double>> orderedSolutionEdges)
    {
        var states = new List<GraphStateViewModel>();
        var activeNodeIds = new HashSet<string>(StringComparer.Ordinal);
        var activeEdgeKeys = new List<string>();
        var directedActiveEdgeIds = new List<string>();

        var initialNodeId = orderedSolutionEdges.Count > 0
            ? orderedSolutionEdges[0].Source
            : graph.Edges.SelectMany(edge => new[] { edge.Source, edge.Target }).Distinct(StringComparer.Ordinal).FirstOrDefault() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(initialNodeId))
        {
            activeNodeIds.Add(initialNodeId);
        }

        states.Add(CreateGraphState(graph, activeNodeIds, activeEdgeKeys, directedActiveEdgeIds, initialNodeId));

        foreach (var edge in orderedSolutionEdges)
        {
            var orientation = ResolveOrientation(activeNodeIds, edge.Source, edge.Target);
            var edgeKey = GraphViewKeys.CreateUndirectedEdgeKey(edge.Source, edge.Target, edge.Tag);
            activeEdgeKeys.Add(edgeKey);
            directedActiveEdgeIds.Add(GraphViewKeys.CreateDirectedEdgeId(orientation.From, orientation.To));
            activeNodeIds.Add(orientation.From);
            activeNodeIds.Add(orientation.To);

            states.Add(CreateGraphState(graph, activeNodeIds, activeEdgeKeys, directedActiveEdgeIds, orientation.To));
        }

        return states;
    }

    public static GraphViewModel BuildSvgGraphView(
        IUndirectedGraph<string, ITaggedEdge<string, double>> graph,
        IReadOnlyCollection<string> selectedEdgeKeys)
    {
        const double padding = 40.0;
        var geometryGraph = CreateGeometryGraph(graph);
        var settings = new SugiyamaLayoutSettings
        {
            AspectRatio = 1.5,
            MinimalHeight = 0.0,
            MinimalWidth = 0.0
        };

        LayoutHelpers.CalculateLayout(geometryGraph, settings, null);
        geometryGraph.UpdateBoundingBox();
        geometryGraph.Translate(new Point(-geometryGraph.Left + padding, -geometryGraph.Bottom + padding));
        geometryGraph.UpdateBoundingBox();

        var solutionNodeKeys = new HashSet<string>(
            selectedEdgeKeys.SelectMany(key => key.Split(new[] { '|' }, 3).Take(2)),
            StringComparer.Ordinal);

        var nodes = geometryGraph.Nodes
            .Select(node =>
            {
                var label = node.UserData as string ?? string.Empty;
                return new GraphNodeViewModel(
                    label,
                    label,
                    node.BoundingBox.Center.X,
                    node.BoundingBox.Center.Y,
                    node.BoundingBox.Width / 2.0,
                    node.BoundingBox.Height / 2.0,
                    solutionNodeKeys.Contains(label));
            })
            .OrderBy(node => node.Label, StringComparer.Ordinal)
            .ToArray();

        var edges = geometryGraph.Edges
            .Select(edge =>
            {
                var taggedEdge = (ITaggedEdge<string, double>)edge.UserData;
                var edgeKey = GraphViewKeys.CreateUndirectedEdgeKey(taggedEdge.Source, taggedEdge.Target, taggedEdge.Tag);
                var labelCenter = edge.Label?.BoundingBox.Center ?? new Point(
                    (edge.Source.BoundingBox.Center.X + edge.Target.BoundingBox.Center.X) / 2.0,
                    (edge.Source.BoundingBox.Center.Y + edge.Target.BoundingBox.Center.Y) / 2.0);

                return new GraphEdgeViewModel(
                    edgeKey,
                    taggedEdge.Source,
                    taggedEdge.Target,
                    taggedEdge.Tag.ToString("0.##", CultureInfo.InvariantCulture),
                    BuildSvgPathData(edge.Curve),
                    labelCenter.X,
                    labelCenter.Y,
                    selectedEdgeKeys.Contains(edgeKey));
            })
            .OrderBy(edge => edge.Source, StringComparer.Ordinal)
            .ThenBy(edge => edge.Target, StringComparer.Ordinal)
            .ToArray();

        return new GraphViewModel(
            geometryGraph.BoundingBox.Right + padding,
            geometryGraph.BoundingBox.Top + padding,
            nodes,
            edges);
    }

    private static GraphStateViewModel CreateGraphState(
        IUndirectedGraph<string, ITaggedEdge<string, double>> graph,
        HashSet<string> activeNodeIds,
        IReadOnlyCollection<string> activeEdgeKeys,
        IReadOnlyCollection<string> directedActiveEdgeIds,
        string currentNodeId)
    {
        var successorEdgeKeys = new HashSet<string>(StringComparer.Ordinal);
        var directedSuccessorEdgeIds = new HashSet<string>(StringComparer.Ordinal);
        var frontierNodeIds = new HashSet<string>(StringComparer.Ordinal);

        foreach (var edge in graph.Edges)
        {
            var edgeKey = GraphViewKeys.CreateUndirectedEdgeKey(edge.Source, edge.Target, edge.Tag);
            if (activeEdgeKeys.Contains(edgeKey))
            {
                continue;
            }

            var sourceActive = activeNodeIds.Contains(edge.Source);
            var targetActive = activeNodeIds.Contains(edge.Target);
            if (sourceActive == targetActive)
            {
                continue;
            }

            successorEdgeKeys.Add(edgeKey);
            var fromNodeId = sourceActive ? edge.Source : edge.Target;
            var toNodeId = sourceActive ? edge.Target : edge.Source;
            frontierNodeIds.Add(toNodeId);
            directedSuccessorEdgeIds.Add(GraphViewKeys.CreateDirectedEdgeId(fromNodeId, toNodeId));
        }

        var exploredNodeIds = activeNodeIds
            .Where(nodeId => !string.Equals(nodeId, currentNodeId, StringComparison.Ordinal))
            .OrderBy(nodeId => nodeId, StringComparer.Ordinal)
            .ToArray();

        return new GraphStateViewModel(
            currentNodeId,
            activeNodeIds.OrderBy(nodeId => nodeId, StringComparer.Ordinal).ToArray(),
            frontierNodeIds.OrderBy(nodeId => nodeId, StringComparer.Ordinal).ToArray(),
            exploredNodeIds,
            activeEdgeKeys.ToArray(),
            successorEdgeKeys.OrderBy(edgeKey => edgeKey, StringComparer.Ordinal).ToArray(),
            directedActiveEdgeIds.ToArray(),
            directedSuccessorEdgeIds.OrderBy(edgeId => edgeId, StringComparer.Ordinal).ToArray());
    }

    private static double LayoutTreeNode(
        string nodeId,
        int depth,
        IReadOnlyDictionary<string, TreeLayoutNode[]> childrenByParent,
        IDictionary<string, (double X, int Depth)> positions,
        ref double nextLeafIndex)
    {
        var children = childrenByParent.TryGetValue(nodeId, out var childNodes)
            ? childNodes
            : [];

        if (children.Length == 0)
        {
            var xLeaf = nextLeafIndex;
            nextLeafIndex += 1.0;
            positions[nodeId] = (xLeaf, depth);
            return xLeaf;
        }

        var childXs = new List<double>(children.Length);
        foreach (var child in children)
        {
            childXs.Add(LayoutTreeNode(child.Id, depth + 1, childrenByParent, positions, ref nextLeafIndex));
        }

        var x = childXs.Average();
        positions[nodeId] = (x, depth);
        return x;
    }

    private static (double X, double Y) ProjectTreePoint(
        double breadth,
        int depth,
        double horizontalSpacing,
        double verticalSpacing,
        double padding,
        TreeLayoutOrientation orientation)
    {
        return orientation switch
        {
            TreeLayoutOrientation.LeftToRight => (
                padding + (depth * verticalSpacing),
                padding + (breadth * horizontalSpacing)),
            _ => (
                padding + (breadth * horizontalSpacing),
                padding + (depth * verticalSpacing))
        };
    }

    private static (string From, string To) ResolveOrientation(HashSet<string> activeNodeIds, string source, string target)
    {
        var sourceActive = activeNodeIds.Contains(source);
        var targetActive = activeNodeIds.Contains(target);

        if (sourceActive && !targetActive)
        {
            return (source, target);
        }

        if (targetActive && !sourceActive)
        {
            return (target, source);
        }

        return (source, target);
    }

    private static GeometryGraph CreateGeometryGraph(
        IUndirectedGraph<string, ITaggedEdge<string, double>> graph,
        double nodeSize = 52.0,
        double labelWidth = 44.0,
        double labelHeight = 24.0)
    {
        var geometryGraph = new GeometryGraph();
        var nodes = new Dictionary<string, Node>(StringComparer.Ordinal);

        foreach (var edge in graph.Edges)
        {
            if (!nodes.ContainsKey(edge.Source))
            {
                var sourceNode = new Node(CurveFactory.CreateRectangle(nodeSize, nodeSize, new Point()), edge.Source);
                nodes.Add(edge.Source, sourceNode);
                geometryGraph.Nodes.Add(sourceNode);
            }

            if (!nodes.ContainsKey(edge.Target))
            {
                var targetNode = new Node(CurveFactory.CreateRectangle(nodeSize, nodeSize, new Point()), edge.Target);
                nodes.Add(edge.Target, targetNode);
                geometryGraph.Nodes.Add(targetNode);
            }

            geometryGraph.Edges.Add(new Edge(nodes[edge.Source], nodes[edge.Target])
            {
                Label = new Label
                {
                    Width = labelWidth,
                    Height = labelHeight
                },
                UserData = edge
            });
        }

        return geometryGraph;
    }

    private static string BuildSvgPathData(ICurve curve)
    {
        var builder = new StringBuilder();
        AppendMove(builder, curve.Start);

        switch (curve)
        {
            case LineSegment line:
                AppendLine(builder, line.End);
                break;
            case Curve compositeCurve:
                foreach (var segment in compositeCurve.Segments)
                {
                    AppendSegment(builder, segment);
                }
                break;
            case Ellipse ellipse:
                AppendEllipse(builder, ellipse);
                break;
            default:
                AppendLine(builder, curve.End);
                break;
        }

        return builder.ToString();
    }

    private static void AppendSegment(StringBuilder builder, ICurve segment)
    {
        switch (segment)
        {
            case LineSegment line:
                AppendLine(builder, line.End);
                break;
            case CubicBezierSegment bezier:
                builder.AppendFormat(
                    CultureInfo.InvariantCulture,
                    " C {0:0.##} {1:0.##}, {2:0.##} {3:0.##}, {4:0.##} {5:0.##}",
                    bezier.B(1).X,
                    bezier.B(1).Y,
                    bezier.B(2).X,
                    bezier.B(2).Y,
                    bezier.B(3).X,
                    bezier.B(3).Y);
                break;
            case Ellipse ellipse:
                AppendEllipse(builder, ellipse);
                break;
            case Curve nestedCurve:
                foreach (var nestedSegment in nestedCurve.Segments)
                {
                    AppendSegment(builder, nestedSegment);
                }
                break;
            default:
                AppendLine(builder, segment.End);
                break;
        }
    }

    private static void AppendEllipse(StringBuilder builder, Ellipse ellipse)
    {
        const int steps = 12;
        var stepSize = (ellipse.ParEnd - ellipse.ParStart) / steps;

        for (var index = 1; index <= steps; index++)
        {
            var parameter = ellipse.ParStart + (stepSize * index);
            var point = ellipse.Center + (Math.Cos(parameter) * ellipse.AxisA) + (Math.Sin(parameter) * ellipse.AxisB);
            AppendLine(builder, point);
        }
    }

    private static void AppendMove(StringBuilder builder, Point point)
        => builder.AppendFormat(CultureInfo.InvariantCulture, "M {0:0.##} {1:0.##}", point.X, point.Y);

    private static void AppendLine(StringBuilder builder, Point point)
        => builder.AppendFormat(CultureInfo.InvariantCulture, " L {0:0.##} {1:0.##}", point.X, point.Y);
}