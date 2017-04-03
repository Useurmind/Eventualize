using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.EventStore.Persistence;
using Eventualize.Materialization;
using Eventualize.Persistence;

namespace Eventualize.EventStore.Materialization
{
    public class EventStoreMaterializer : IMaterializer
    {
        private IConstructInstances aggregateFactory;

        private IEventStoreConnection connection;

        private IEventStoreEventConverter eventConverter;

        private IEnumerable<IMaterializationStrategy> materializationStrategies;

        private EventStoreAllCatchUpSubscription subscription;

        public EventStoreMaterializer(IConstructInstances aggregateFactory, IEventStoreEventConverter eventConverter, IEventStoreConnection connection, IEnumerable<IMaterializationStrategy> materializationStrategies)
        {
            this.connection = connection;
            this.aggregateFactory = aggregateFactory;
            this.eventConverter = eventConverter;
            this.materializationStrategies = materializationStrategies;
        }

        public void Run()
        {
            this.subscription = this.connection.SubscribeToAllFrom(
                Position.Start,
                new CatchUpSubscriptionSettings(100, 50, false, true),
                (subscription, resolvedevent) =>
                {
                    var recordedEvent = resolvedevent.Event;
                    var streamName = StreamName.FromStreamName(recordedEvent.EventStreamId);
                    var eventData = eventConverter.GetDomainEvent(recordedEvent);

                    foreach (var materializationStrategy in this.materializationStrategies)
                    {
                        materializationStrategy.HandleEvent(
                        () =>
                        {
                            return this.aggregateFactory.BuildAggregate(
                                streamName.AggregateTypeName,
                                streamName.AggregateId,
                                null);
                        },
                        streamName.AggregateId,
                        eventData);
                    }
                });
        }

        public void Dispose()
        {
            this.subscription.Stop();
        }
    }
}
