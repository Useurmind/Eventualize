using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Domain.MetaModel
{
    public interface IBoundedContextMetaModel
    {
        /// <summary>
        /// The aggregate types in this bounded context.
        /// </summary>
        IEnumerable<IAggregateMetaModel> AggregateTypes { get; }

        /// <summary>
        /// The event types in this bounded context.
        /// </summary>
        IEnumerable<IEventMetaModel> EventTypes { get; }

        /// <summary>
        /// The name of the bounded context to which this instance belongs.
        /// </summary>
        BoundedContextName BoundedContextName { get; }

        /// <summary>
        /// Get the aggregate type with the given name.
        /// </summary>
        /// <param name="name">The name of the aggregate type.</param>
        /// <returns>The meta model for the aggregate type or null.</returns>
        IAggregateMetaModel GetAggregateType(AggregateTypeName name);

        /// <summary>
        /// Get the event type with the given name.
        /// </summary>
        /// <param name="name">The name of the event type.</param>
        /// <returns>The meta model for the aggregateevent type or null.</returns>
        IEventMetaModel GetEventType(EventTypeName name);
    }
}