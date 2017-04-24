using System.Linq;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Materialization.ReactiveStreams;

namespace Eventualize.Persistence
{
    /// <summary>
    /// Creates different event sources from an <see cref="InMemoryAggregateEventStore"/>.
    /// </summary>
    public class InMemoryEventSourceFactory : IEventSourceFactory
    {
        private InMemoryAggregateEventStore eventStore;

        public InMemoryEventSourceFactory(InMemoryAggregateEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        public void ConnectAll()
        {
            // no connection necessary
        }

        /// <inheritdoc />
        public IEventSource FromAll(EventStreamIndex? afterEventIndex=null)
        {
            return this.eventStore.SkipAfter(afterEventIndex);
        }

        /// <inheritdoc />
        public IEventSource FromBoundedContext(BoundedContextName boundedContextName, EventStreamIndex? afterEventIndex = null)
        {
            return this.eventStore.SkipAfter(afterEventIndex)
                .Where(x => x.BoundedContextName == boundedContextName);
        }

        /// <inheritdoc />
        public IAggregateEventSource FromAggregateType(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName, EventStreamIndex? afterEventIndex = null)
        {
            return this.eventStore.SkipAfter(afterEventIndex)
                .AsAggregateEventSource().Where(x => x.BoundedContextName == boundedContextName && x.AggregateIdentity.AggregateTypeName == aggregateTypeName);
        }

        /// <inheritdoc />
        public IEventSource FromEventType(BoundedContextName boundedContextName, EventTypeName eventTypeName, EventStreamIndex? afterEventIndex = null)
        {
            return
                this.eventStore.SkipAfter(afterEventIndex)
                .Where(x => x.BoundedContextName == boundedContextName && x.EventTypeName == eventTypeName);
        }
    }
}