﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.EventStore.Persistence;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Materialization;
using Eventualize.Materialization;
using Eventualize.Materialization.AggregateMaterialization;
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
        private IAggregateFactory aggregateFactory;

        private IEventStoreConnection connection;

        private IEventStoreEventConverter eventConverter;

        private IEnumerable<IMaterializationStrategy> materializationStrategies;

        private EventStoreAllCatchUpSubscription subscription;

        private IEnumerable<IAggregateMaterializationStrategy> aggregateMaterializationStrategies;

        private BoundedContext boundedContext;

        private IMaterializationProgess materializationProgess;

        public EventStoreMaterializationEventPoller(IAggregateFactory aggregateFactory, IEventStoreEventConverter eventConverter, IEventStoreConnection connection, IEnumerable<IMaterializationStrategy> materializationStrategies, 
            IMaterializationProgess materializationProgess, IEnumerable<IAggregateMaterializationStrategy> aggregateMaterializationStrategies, BoundedContext boundedContext)
        {
            this.connection = connection;
            this.aggregateFactory = aggregateFactory;
            this.eventConverter = eventConverter;
            this.materializationStrategies = materializationStrategies;
            this.aggregateMaterializationStrategies = aggregateMaterializationStrategies;
            this.materializationProgess = materializationProgess;
            this.boundedContext = boundedContext;
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
                    if (AggregateStreamName.IsAggregateStreamName(recordedEvent.EventStreamId, this.boundedContext))
                    {
                        var streamName = AggregateStreamName.FromStreamName(recordedEvent.EventStreamId);
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
                        var @event = eventConverter.GetDomainEvent(recordedEvent, this.boundedContext);

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
