﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Persistence;

namespace Eventualize.Materialization.AggregateMaterialization
{
    public class InMemoryMaterializationStrategy : IAggregateMaterializationStrategy
    {
        private ConcurrentDictionary<Guid, IAggregate> aggregates = new ConcurrentDictionary<Guid, IAggregate>();

        private IAggregateFactory aggregateFactory;

        public InMemoryMaterializationStrategy(IAggregateFactory aggregateFactory)
        {
            this.aggregateFactory = aggregateFactory;
        }

        public IEnumerable<TAggregate> GetAggregates<TAggregate>() where TAggregate : IAggregate
        {
            return this.aggregates.Values.OfType<TAggregate>();
        }

        public void HandleEvent(IAggregateEvent materializationEvent)
        {
            IAggregate aggregate = null;
            if (!this.aggregates.TryGetValue(materializationEvent.AggregateIdentity.Id, out aggregate))
            {
                aggregate = this.aggregateFactory.BuildAggregate(materializationEvent.AggregateIdentity, null);
                this.aggregates[aggregate.Id] = aggregate;
            }

            aggregate.ApplyEvent(materializationEvent.EventData);
        }
    }
}