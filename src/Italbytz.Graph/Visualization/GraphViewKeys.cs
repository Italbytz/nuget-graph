using System;
using System.Globalization;
using System.Linq;

namespace Italbytz.Graph.Visualization;

public static class GraphViewKeys
{
    public static string CreateUndirectedEdgeKey(string source, string target, double weight)
    {
        var ordered = new[] { source, target }
            .OrderBy(value => value, StringComparer.Ordinal)
            .ToArray();

        return $"{ordered[0]}|{ordered[1]}|{weight.ToString("0.##", CultureInfo.InvariantCulture)}";
    }

    public static string CreateDirectedEdgeId(string source, string target) => $"{source}>{target}";
}