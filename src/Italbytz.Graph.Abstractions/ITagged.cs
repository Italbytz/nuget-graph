using System;
namespace Italbytz.Graph.Abstractions
{
    /// <summary>
    /// Represents an object that is able to be tagged.
    /// </summary>
    /// <typeparam name="TTag">Tag type.</typeparam>
    public interface ITagged<TTag>
    {
        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        TTag Tag { get; set; }
    }
}
