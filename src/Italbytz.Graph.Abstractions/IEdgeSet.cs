using System;
using System.Collections.Generic;

namespace Italbytz.Graph.Abstractions
{
    /// <summary>
    /// Represents a set of edges.
    /// </summary>
    /// <typeparam name="TVertex">Vertex type.</typeparam>
    /// <typeparam name="TEdge">Edge type.</typeparam>
    public interface IEdgeSet<TVertex, TEdge>
        where TEdge : IEdge<TVertex>
    {
        IEnumerable<TEdge> Edges { get; }
    }
}