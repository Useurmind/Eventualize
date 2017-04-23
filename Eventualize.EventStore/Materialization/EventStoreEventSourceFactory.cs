using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.EventStore.Persistence;
using Eventualize.EventStore.Projections;
using Eventualize.EventStore.Test.Projections;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Materialization.ReactiveStreams;

namespace Eventualize.EventStore.Materialization
{
    public class EventStoreEventSourceFactory : IEventSourceFactory
    {
        private IAggregateFactory aggregateFactory;

        private IEventStoreEventConverter eventConverter;

        private IEventStoreConnection connection;

        private IProjectionFactory projectionFactory;

        public EventStoreEventSourceFactory(IAggregateFactory aggregateFactory, IEventStoreEventConverter eventConverter, IEventStoreConnection connection, IProjectionFactory projectionFactory)
        {
            this.aggregateFactory = aggregateFactory;
            this.eventConverter = eventConverter;
            this.connection = connection;
            this.projectionFactory = projectionFactory;
        }

        public IEventSource FromAll(EventStreamIndex? afterEventIndex = null)
        {
            this.projectionFactory.EnsureProjectionForAllBoundedContexts();

            return new EventStoreStreamPoller(this.aggregateFactory, this.eventConverter, this.connection, afterEventIndex, new ProjectionStreamName().ToString());
        }

        public IEventSource FromBoundedContext(BoundedContextName boundedContextName, EventStreamIndex? afterEventIndex = null)
        {
            this.projectionFactory.EnsureProjectionFor(boundedContextName);

            return new EventStoreStreamPoller(this.aggregateFactory, this.eventConverter, this.connection, afterEventIndex, new ProjectionStreamName(boundedContextName).ToString());
        }

        public IAggregateEventSource FromAggregateType(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName, EventStreamIndex? afterEventIndex = null)
        {
            this.projectionFactory.EnsureProjectionFor(boundedContextName, aggregateTypeName);

            return new EventStoreStreamPoller(this.aggregateFactory, this.eventConverter, this.connection, afterEventIndex, new ProjectionStreamName(boundedContextName, aggregateTypeName).ToString()).AsAggregateEventSource();
        }

        public IEventSource FromEventType(BoundedContextName boundedContextName, EventTypeName eventTypeName, EventStreamIndex? afterEventIndex = null)
        {
            throw new NotImplementedException();
        }
    }
}
