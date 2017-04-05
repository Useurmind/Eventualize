using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain;
using Eventualize.Persistence;

namespace Eventualize.Materialization
{
    public class InMemoryMaterialization : IAggregateMaterializationStrategy
    {
        private ConcurrentDictionary<Guid, IAggregate> aggregates = new ConcurrentDictionary<Guid, IAggregate>();

        private IConstructInstances aggregateFactory;

        public InMemoryMaterialization(IConstructInstances aggregateFactory)
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