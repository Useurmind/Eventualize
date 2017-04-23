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

        /// <inheritdoc />
        public IEventSource FromAll()
        {
            return this.eventStore;
        }

        /// <inheritdoc />
        public IEventSource FromBoundedContext(BoundedContextName boundedContextName)
        {
            return this.eventStore.Where(x => x.BoundedContextName == boundedContextName);
        }

        /// <inheritdoc />
        public IAggregateEventSource FromAggregateType(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName)
        {
            return this.eventStore.AsAggregateEventSource().Where(x => x.BoundedContextName == boundedContextName && x.AggregateIdentity.AggregateTypeName == aggregateTypeName);
        }

        /// <inheritdoc />
        public IEventSource FromEventType(BoundedContextName boundedContextName, EventTypeName eventTypeName)
        {
            return
                this.eventStore.Where(
                    x => x.BoundedContextName == boundedContextName && x.EventTypeName == eventTypeName);
        }
    }
}