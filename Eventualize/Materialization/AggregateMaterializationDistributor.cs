using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Core;
using Eventualize.Persistence;

namespace Eventualize.Materialization
{
    public class AggregateMaterializationDistributor : IAggregateMaterializationStrategy
    {
        private Dictionary<string, IEnumerable<IAggregateMaterializer>> materializerByAggregateType;
        private IAggregateRepository aggregateRepository;

        public AggregateMaterializationDistributor(IAggregateRepository aggregateRepository, IEnumerable<IAggregateMaterializer> aggregateMaterializers)
        {
            this.aggregateRepository = aggregateRepository;

            var allAggregateTypes = aggregateMaterializers.SelectMany(x => x.AggregateTypes).Distinct().ToArray();

            this.materializerByAggregateType = new Dictionary<string, IEnumerable<IAggregateMaterializer>>();
            foreach (var aggregateType in allAggregateTypes)
            {
                this.materializerByAggregateType[aggregateType.GetAggregtateTypeName().Value] =
                    aggregateMaterializers.Where(x => x.AggregateTypes.Contains(aggregateType)).ToArray();
            }
        }

        public void HandleEvent(IAggregateEvent materializationEvent)
        {
            IEnumerable<IAggregateMaterializer> materializers = null;

            var aggregate = this.aggregateRepository.GetById(materializationEvent.AggregateIdentity);

            if (this.materializerByAggregateType.TryGetValue(materializationEvent.AggregateIdentity.AggregateTypeName.Value, out materializers))
            {
                foreach (var materializer in materializers)
                {
                    materializer.HandleAggregateEvent(aggregate, materializationEvent);
                }
            }
        }
    }
}