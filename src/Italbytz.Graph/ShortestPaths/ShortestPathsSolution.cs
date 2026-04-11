using System;
using System.Collections.Generic;
using Italbytz.Graph.Abstractions;
using QuikGraph;

namespace Italbytz.Graph
{
    public class ShortestPathsSolution : IShortestPathsSolution
    {
        public ShortestPathsSolution()
        {
        }

        public List<string> Paths { get; set; }
    }
}
