using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Materialization.ReactiveStreams
{
    public interface IEventSourceFactory
    {
        /// <summary>
        /// Get an event source that delivers all events from the underlying store.
        /// </summary>
        /// <param name="afterEventIndex">The index from which to start (the event with this index will not be included).</param>
        /// <returns></returns>
        IEventSource FromAll(EventStreamIndex? afterEventIndex= null);

        /// <summary>
        /// Get an event source that delivers all events from the underlying store which belong to a specific bounded context.
        /// </summary>
        /// <param name="boundedContextName">The name of the bounded context for which the events should be delivered.</param>
        /// <param name="afterEventIndex">The index from which to start (the event with this index will not be included).</param>
        /// <returns></returns>
        IEventSource FromBoundedContext(BoundedContextName boundedContextName, EventStreamIndex? afterEventIndex = null);

        /// <summary>
        /// Get an event source that delivers all events from the underlying store which belong to a specific bounded context and aggregate type.
        /// </summary>
        /// <param name="boundedContextName">The name of the bounded context for which the events should be delivered.</param>
        /// <param name="aggregateTypeName">The name of the aggregate type for which the events should be delivered.</param>
        /// <param name="afterEventIndex">The index from which to start (the event with this index will not be included).</param>
        /// <returns></returns>
        IAggregateEventSource FromAggregateType(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName, EventStreamIndex? afterEventIndex = null);

        /// <summary>
        /// Get an event source that delivers all events from the underlying store which belong to a specific bounded context and event type (across aggregates if so). 
        /// </summary>
        /// <param name="boundedContextName">The name of the bounded context for which the events should be delivered.</param>
        /// <param name="eventTypeName">The name of the event type that should be delivered.</param>
        /// <param name="afterEventIndex">The index from which to start (the event with this index will not be included).</param>
        /// <returns></returns>
        IEventSource FromEventType(BoundedContextName boundedContextName, EventTypeName eventTypeName, EventStreamIndex? afterEventIndex = null);
    }
}