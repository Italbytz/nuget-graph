using System;
namespace Italbytz.Graph.Abstractions
{
    public interface IShortestPathsParameters
    {
        IUndirectedGraph<string, ITaggedEdge<string, double>> Graph { get; set; }
        String[] Vertices { get; set; }
    }
}
