using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.EventStore.Persistence;
using Eventualize.EventStore.Projections;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Progress;
using Eventualize.Materialization;
using Eventualize.Materialization.AggregateMaterialization;
using Eventualize.Materialization.Progress;
using Eventualize.Persistence;

namespace Eventualize.EventStore.Materialization
{
    public class EventStoreAllStreamPosition
    {
        public EventStoreAllStreamPosition(Position position)
        {
            this.CommitPosition = position.CommitPosition;
            this.PreparePosition = position.PreparePosition;
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
        private IAggregateFactory aggregateFactory;

        private IEventStoreConnection connection;

        private IEventStoreEventConverter eventConverter;

        private IEnumerable<IMaterializationStrategy> materializationStrategies;

        private IEnumerable<IAggregateMaterializationStrategy> aggregateMaterializationStrategies;

        private IMaterializationProgess materializationProgess;

        private EventStoreStreamCatchUpSubscription subscription;

        public EventStoreMaterializationEventPoller(IAggregateFactory aggregateFactory, IEventStoreEventConverter eventConverter, IEventStoreConnection connection, IEnumerable<IMaterializationStrategy> materializationStrategies,
            IMaterializationProgess materializationProgess, IEnumerable<IAggregateMaterializationStrategy> aggregateMaterializationStrategies)
        {
            this.connection = connection;
            this.aggregateFactory = aggregateFactory;
            this.eventConverter = eventConverter;
            this.materializationStrategies = materializationStrategies;
            this.aggregateMaterializationStrategies = aggregateMaterializationStrategies;
            this.materializationProgess = materializationProgess;
        }

        public void Run()
        {
            var currentProgess = this.materializationProgess.Get<long?>();

            this.subscription = this.connection.SubscribeToStreamFrom(
                new ProjectionStreamName().ToString(),
                currentProgess,
                new CatchUpSubscriptionSettings(100, 50, false, true),
                (subscription, resolvedevent) =>
                {
                    var recordedEvent = resolvedevent.Event;
                    if (recordedEvent == null)
                    {
                        throw new Exception();
                    }

                    var streamName = AggregateStreamName.FromStreamName(recordedEvent.EventStreamId);
                    var aggregateEvent = eventConverter.GetDomainEvent(streamName.GetAggregateIdentity(), recordedEvent, resolvedevent.OriginalEventNumber);

                    foreach (var aStrat in this.aggregateMaterializationStrategies)
                    {
                        aStrat.HandleEvent(aggregateEvent);
                    }

                    foreach (var strat in this.materializationStrategies)
                    {
                        strat.HandleEvent(aggregateEvent);
                    }

                    this.materializationProgess.Set(resolvedevent.OriginalEventNumber);
                });
        }

        public void Dispose()
        {
            this.subscription.Stop();
        }
    }
}
