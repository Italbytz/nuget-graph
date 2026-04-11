namespace Italbytz.Graph.Abstractions
{
    public interface IMinimumSpanningTreeParameters
    {
        IUndirectedGraph<string, ITaggedEdge<string, double>> Graph { get; set; }
    }
}
