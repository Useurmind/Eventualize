using System;
using System.Linq;
using System.Linq.Expressions;

namespace Eventualize.Projection.ProjectionMetaModel
{
    /// <summary>
    /// A handler description that describes the actions to take on a specific event.
    /// </summary>
    public class ProjectionEventHandler
    {
        /// <summary>
        /// The type of the event to handle.
        /// </summary>
        public Type EventType { get; set; }

        /// <summary>
        /// The type of action to perform (insert, delete, etc.)
        /// </summary>
        public ProjectionEventActionType ActionType { get; set; }

        /// <summary>
        /// An action that takes the projection model and the event and then applies the correct values to the projection model.
        /// </summary>
        public Action<object, object> Set { get; set; }

        /// <summary>
        /// An expression that gets the projection model and the event and returns true when the model matches.
        /// </summary>
        public LambdaExpression Where { get; set; }
    }
}