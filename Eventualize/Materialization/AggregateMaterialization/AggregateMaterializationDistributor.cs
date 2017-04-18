using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Persistence;
using Eventualize.Persistence;

namespace Eventualize.Materialization.AggregateMaterialization
{
    public class AggregateMaterializationDistributor : IAggregateMaterializationStrategy
    {
        private IEnumerable<IAggregateMaterializer> materializersForAllTypes;
        private Dictionary<string, IEnumerable<IAggregateMaterializer>> materializerByAggregateType;
        private IAggregateRepository aggregateRepository;

        private IDomainIdentityProvider domainIdentityProvider;

        public AggregateMaterializationDistributor(IAggregateRepository aggregateRepository, IDomainIdentityProvider domainIdentityProvider, IEnumerable<IAggregateMaterializer> aggregateMaterializers)
        {
            this.aggregateRepository = aggregateRepository;
            this.domainIdentityProvider = domainIdentityProvider;

            var allAggregateTypes = aggregateMaterializers.Where(x => !x.ChosenAggregateTypes.AllAggregateTypesChosen)
                .SelectMany(x => x.ChosenAggregateTypes.AggregateTypes)
                .Distinct()
                .ToArray();

            this.materializersForAllTypes = aggregateMaterializers.Where(x => x.ChosenAggregateTypes.AllAggregateTypesChosen);
            this.materializerByAggregateType = new Dictionary<string, IEnumerable<IAggregateMaterializer>>();
            foreach (var aggregateType in allAggregateTypes)
            {
                var aggregateTypeName = this.domainIdentityProvider.GetAggregtateTypeName(aggregateType);
                this.materializerByAggregateType[aggregateTypeName.Value] =
                    aggregateMaterializers.Where(x => !x.ChosenAggregateTypes.AllAggregateTypesChosen && 
                            x.ChosenAggregateTypes.AggregateTypes.Contains(aggregateType))
                            .ToArray();
            }
        }

        public void HandleEvent(IAggregateEvent materializationEvent)
        {
            IEnumerable<IAggregateMaterializer> materializers = Enumerable.Empty<IAggregateMaterializer>();

            this.materializerByAggregateType.TryGetValue(
                materializationEvent.AggregateIdentity.AggregateTypeName.Value,
                out materializers);
            
            if(materializers.Any() || this.materializersForAllTypes.Any())
            {
                // only get aggregate if there are materializers waiting
                var aggregate = this.aggregateRepository.GetById(materializationEvent.AggregateIdentity);

                foreach (var materializer in materializers)
                {
                    materializer.HandleAggregateEvent(aggregate, materializationEvent);
                }

                foreach (var materializer in this.materializersForAllTypes)
                {
                    materializer.HandleAggregateEvent(aggregate, materializationEvent);
                }
            }
        }
    }
}