using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Materialization.ReactiveStreams
{
    /// <summary>
    /// This is a simple wrapper around <see cref="IEventSourceFactory"/> that includes the afterEventIndex for each factory call.
    /// The <see cref="ISubscribeToEventStreams"/> does not need to care about it in this way.
    /// </summary>
    public class EventSourceProvider : IEventSourceProvider
    {
        private EventStreamIndex? afterEventIndex;

        private IEventSourceFactory eventSourceFactory;

        public EventSourceProvider(IEventSourceFactory eventSourceFactory, EventStreamIndex? afterEventIndex = null)
        {
            this.afterEventIndex = afterEventIndex;
            this.eventSourceFactory = eventSourceFactory;
        }

        /// <inheritdoc />
        public IEventSource FromAll()
        {
            return this.eventSourceFactory.FromAll(this.afterEventIndex);
        }

        /// <inheritdoc />
        public IEventSource FromBoundedContext(BoundedContextName boundedContextName)
        {
            return this.eventSourceFactory.FromBoundedContext(boundedContextName, this.afterEventIndex);
        }

        /// <inheritdoc />
        public IAggregateEventSource FromAggregateType(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName)
        {
            return this.eventSourceFactory.FromAggregateType(boundedContextName, aggregateTypeName, this.afterEventIndex);
        }

        /// <inheritdoc />
        public IEventSource FromEventType(BoundedContextName boundedContextName, EventTypeName eventTypeName)
        {
            return this.eventSourceFactory.FromEventType(boundedContextName, eventTypeName, this.afterEventIndex);
        }
    }
}