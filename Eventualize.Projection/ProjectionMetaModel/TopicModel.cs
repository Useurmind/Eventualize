using System.Collections.Generic;
using System.Linq;

using Eventualize.Projection.FluentProjection;

namespace Eventualize.Projection.ProjectionMetaModel
{
    /// <summary>
    /// Model that describes how a topic should be projected.
    /// </summary>
    public class TopicModel
    {
        /// <summary>
        /// The topic that is projected.
        /// </summary>
        public ITopic Topic { get; set; }

        /// <summary>
        /// All projections for this topic.
        /// </summary>
        public IEnumerable<ProjectionModel> Projections { get; set; } = new List<ProjectionModel>();
    }
}