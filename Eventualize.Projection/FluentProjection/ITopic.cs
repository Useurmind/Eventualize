using System.Linq;

namespace Eventualize.Projection.FluentProjection
{
    /// <summary>
    /// A topic/stream/queue whose events should be projected.
    /// </summary>
    public interface ITopic
    {
        /// <summary>
        /// The name of the topic.
        /// </summary>
        string Name { get; }
    }
}