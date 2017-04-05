using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.Domain;
using Eventualize.EventStore.Persistence;
using Eventualize.Materialization;
using Eventualize.Persistence;

namespace Eventualize.EventStore.Materialization
{
    public class EventStoreMaterializationEventPoller : IMaterializationEventPoller
    {
        private IConstructInstances aggregateFactory;

        private IEventStoreConnection connection;

        private IEventStoreEventConverter eventConverter;

        private IEnumerable<IMaterializationStrategy> materializationStrategies;

        private EventStoreAllCatchUpSubscription subscription;

        private IEnumerable<IAggregateMaterializationStrategy> aggregateMaterializationStrategies;

        public EventStoreMaterializationEventPoller(IConstructInstances aggregateFactory, IEventStoreEventConverter eventConverter, IEventStoreConnection connection, IEnumerable<IMaterializationStrategy> materializationStrategies, IEnumerable<IAggregateMaterializationStrategy> aggregateMaterializationStrategies)
        {
            this.connection = connection;
            this.aggregateFactory = aggregateFactory;
            this.eventConverter = eventConverter;
            this.materializationStrategies = materializationStrategies;
            this.aggregateMaterializationStrategies = aggregateMaterializationStrategies;
        }

        public void Run()
        {
            this.subscription = this.connection.SubscribeToAllFrom(
                Position.Start,
                new CatchUpSubscriptionSettings(100, 50, false, true),
                (subscription, resolvedevent) =>
                {
                    var recordedEvent = resolvedevent.Event;
                    if (StreamName.IsAggregateStreamName(recordedEvent.EventStreamId))
                    {
                        var streamName = StreamName.FromStreamName(recordedEvent.EventStreamId);
                        var aggregateEvent = eventConverter.GetDomainEvent(streamName.GetAggregateIdentity(), recordedEvent);

                        foreach (var aStrat in this.aggregateMaterializationStrategies)
                        {
                            aStrat.HandleEvent(aggregateEvent);
                        }

                        foreach (var strat in this.materializationStrategies)
                        {
                            strat.HandleEvent(aggregateEvent);
                        }
                    }
                    else
                    {
                        return;

                        // we do not support this yet
                        var @event = eventConverter.GetDomainEvent(recordedEvent);

                        foreach (var strat in this.materializationStrategies)
                        {
                            strat.HandleEvent(@event);
                        }
                    }
                });
        }

        public void Dispose()
        {
            this.subscription.Stop();
        }
    }
}
