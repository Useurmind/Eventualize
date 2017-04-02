using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain;

namespace Eventualize.Materialization
{
    public class InMemoryMaterialization : IMaterializationStrategy
    {
        private ConcurrentDictionary<Guid, IAggregate> aggregates = new ConcurrentDictionary<Guid, IAggregate>();

        public IEnumerable<TAggregate> GetAggregates<TAggregate>() where TAggregate : IAggregate
        {
            return this.aggregates.Values.OfType<TAggregate>();
        }

        public void HandleEvent(Func<IAggregate> createAggregate, Guid aggregateId, object @event)
        {
            IAggregate aggregate = null;
            if (!this.aggregates.TryGetValue(aggregateId, out aggregate))
            {
                aggregate = createAggregate();
                this.aggregates[aggregate.Id] = aggregate;
            }

            aggregate.ApplyEvent(@event);
        }
    }
}