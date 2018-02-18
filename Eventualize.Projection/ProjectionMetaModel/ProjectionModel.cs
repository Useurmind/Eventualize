using System;
using System.Collections.Generic;
using System.Text;

namespace Eventualize.Projection.ProjectionMetaModel
{
    /// <summary>
    /// A model that describes the projection to one specific output model.
    /// </summary>
    public class ProjectionModel
    {
        /// <summary>
        /// The type of the model to which the events are projected.
        /// </summary>
        public Type ProjectionModelType { get; set; }

        /// <summary>
        /// The event handlers for the different events in the topic.
        /// </summary>
        public IEnumerable<ProjectionEventHandler> EventHandlers { get; set; } = new List<ProjectionEventHandler>();
    }
}
