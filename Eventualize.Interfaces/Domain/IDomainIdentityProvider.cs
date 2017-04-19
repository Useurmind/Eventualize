using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Domain
{
    /// <summary>
    /// Provides services to retrieve the names and types of aggregates and events.
    /// </summary>
    public interface IDomainIdentityProvider
    {
        /// <summary>
        /// Get the bounded context name for an .Net aggregate type.
        /// </summary>
        /// <param name="aggregateType">The .NET aggregate type.</param>
        /// <returns>The bounded context name.</returns>
        BoundedContextName GetAggregateBoundedContext(Type aggregateType);

        /// <summary>
        /// Ge the aggregate type name for a .Net aggregate type.
        /// </summary>
        /// <param name="aggregateType">The .NET aggregate type.</param>
        /// <returns>The name of the aggregate type</returns>
        AggregateTypeName GetAggregtateTypeName(Type aggregateType);

        /// <summary>
        /// Get the event type name from an event data instance.
        /// </summary>
        /// <param name="eventData">The event data from which to retrieve the event type name.</param>
        /// <returns>The event type name.</returns>
        EventTypeName GetEventTypeName(IEventData eventData);

        /// <summary>
        /// Get the event type name from the type of event data.
        /// </summary>
        /// <param name="eventType">The .NET event data type.</param>
        /// <returns>The event type name.</returns>
        EventTypeName GetEventTypeName(Type eventType);
    }
}
