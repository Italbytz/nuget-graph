using System;
namespace Italbytz.Graph.Abstractions
{
    public interface ITaggedEdge<TVertex, TTag> : IEdge<TVertex>, ITagged<TTag>
    {
    }
}
