using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Materialization.ReactiveStreams
{
    /// <summary>
    /// This is the <see cref="IEventSourceFactory"/>
    /// </summary>
    public interface IEventSourceProvider
    {
        /// <summary>
        /// Get an event source that delivers all events from the underlying store.
        /// </summary>
        /// <returns></returns>
        IEventSource FromAll();

        /// <summary>
        /// Get an event source that delivers all events from the underlying store which belong to a specific bounded context.
        /// </summary>
        /// <param name="boundedContextName">The name of the bounded context for which the events should be delivered.</param>
        /// <returns></returns>
        IEventSource FromBoundedContext(BoundedContextName boundedContextName);

        /// <summary>
        /// Get an event source that delivers all events from the underlying store which belong to a specific bounded context and aggregate type.
        /// </summary>
        /// <param name="boundedContextName">The name of the bounded context for which the events should be delivered.</param>
        /// <param name="aggregateTypeName">The name of the aggregate type for which the events should be delivered.</param>
        /// <returns></returns>
        IAggregateEventSource FromAggregateType(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName);

        /// <summary>
        /// Get an event source that delivers all events from the underlying store which belong to a specific bounded context and event type (across aggregates if so). 
        /// </summary>
        /// <param name="boundedContextName">The name of the bounded context for which the events should be delivered.</param>
        /// <param name="eventTypeName">The name of the event type that should be delivered.</param>
        /// <returns></returns>
        IEventSource FromEventType(BoundedContextName boundedContextName, EventTypeName eventTypeName);
    }
}