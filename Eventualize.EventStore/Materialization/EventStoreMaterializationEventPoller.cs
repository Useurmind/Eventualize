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
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;

namespace Eventualize.EventStore.Materialization
{
    public class EventStoreAllStreamPosition
    {
        public EventStoreAllStreamPosition(Position position )
        {
            this.CommitPosition = position.CommitPosition;
            this.PreparePosition =position.PreparePosition;
        }

        public Position ToPosition()
        {
            return new Position(this.CommitPosition, this.PreparePosition);
        }

        public long PreparePosition { get; set; }

        public long CommitPosition { get; set; }
    }

    public class EventStoreMaterializationEventPoller : IMaterializationEventPoller
    {
        private IConstructInstances aggregateFactory;

        private IEventStoreConnection connection;

        private IEventStoreEventConverter eventConverter;

        private IEnumerable<IMaterializationStrategy> materializationStrategies;

        private EventStoreAllCatchUpSubscription subscription;

        private IEnumerable<IAggregateMaterializationStrategy> aggregateMaterializationStrategies;

        private EventNamespace eventNamespace;

        private IMaterializationProgess materializationProgess;

        public EventStoreMaterializationEventPoller(IConstructInstances aggregateFactory, IEventStoreEventConverter eventConverter, IEventStoreConnection connection, IEnumerable<IMaterializationStrategy> materializationStrategies, 
            IMaterializationProgess materializationProgess, IEnumerable<IAggregateMaterializationStrategy> aggregateMaterializationStrategies, EventNamespace eventNamespace)
        {
            this.connection = connection;
            this.aggregateFactory = aggregateFactory;
            this.eventConverter = eventConverter;
            this.materializationStrategies = materializationStrategies;
            this.aggregateMaterializationStrategies = aggregateMaterializationStrategies;
            this.materializationProgess = materializationProgess;
            this.eventNamespace = eventNamespace;
        }

        public void Run()
        {
            var currentProgess = this.materializationProgess.Get<EventStoreAllStreamPosition>();

            this.subscription = this.connection.SubscribeToAllFrom(
                currentProgess == null ? Position.Start : currentProgess.ToPosition(),
                new CatchUpSubscriptionSettings(100, 50, false, true),
                (subscription, resolvedevent) =>
                {
                    var recordedEvent = resolvedevent.Event;
                    if (StreamName.IsAggregateStreamName(recordedEvent.EventStreamId, this.eventNamespace))
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
                        var @event = eventConverter.GetDomainEvent(recordedEvent, this.eventNamespace);

                        foreach (var strat in this.materializationStrategies)
                        {
                            strat.HandleEvent(@event);
                        }
                    }

                    this.materializationProgess.Set(new EventStoreAllStreamPosition(resolvedevent.OriginalPosition.Value));
                });
        }

        public void Dispose()
        {
            this.subscription.Stop();
        }
    }
}
