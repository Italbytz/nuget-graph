using System.Collections.Generic;

namespace Italbytz.Graph.Abstractions
{
    public interface IMinimumSpanningTreeSolution
    {
        IEnumerable<ITaggedEdge<string, double>> Edges { get; set; }
    }
}
