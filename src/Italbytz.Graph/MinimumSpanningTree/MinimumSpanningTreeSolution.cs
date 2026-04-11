using System;
using System.Collections.Generic;
using Italbytz.Graph.Abstractions;
using QuikGraph;

namespace Italbytz.Graph
{
    public class MinimumSpanningTreeSolution : IMinimumSpanningTreeSolution
    {
        public MinimumSpanningTreeSolution()
        {
        }

        public IEnumerable<ITaggedEdge<string, double>> Edges { get; set; }
    }
}
