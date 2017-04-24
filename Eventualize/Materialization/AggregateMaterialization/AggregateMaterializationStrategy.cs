using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Progress;
using Eventualize.Interfaces.Persistence;
using Eventualize.Materialization.ReactiveStreams;
using Eventualize.Persistence;

namespace Eventualize.Materialization.AggregateMaterialization
{
    public class AggregateMaterializationSubscriber : ISubscribeToEventStreams
    {
        private IMaterializationProgess progress;

        private IAggregateMaterializationStrategy aggregateMaterializationStrategy;

        public AggregateMaterializationSubscriber(IMaterializationProgess progress, IAggregateMaterializationStrategy aggregateMaterializationStrategy)
        {
            this.progress = progress;
            this.aggregateMaterializationStrategy = aggregateMaterializationStrategy;
        }

        public async Task<EventStreamIndex?> GetLastHandledEventIndexAsync()
        {
            return await this.progress.GetAsync<EventStreamIndex?>();
        }

        public void SubscribeStreams(IEventSourceProvider eventSourceProvider)
        {
            eventSourceProvider.FromAll()
                               .AsAggregateEventSource()
                               .SubscribeWith(this.aggregateMaterializationStrategy);
        }
    }

    public class AggregateMaterializationStrategy : IAggregateMaterializationStrategy
    {
        private IEnumerable<IAggregateMaterializer> materializersForAllTypes;
        private Dictionary<string, IEnumerable<IAggregateMaterializer>> materializerByAggregateType;
        private IAggregateRepository aggregateRepository;

        private IDomainIdentityProvider domainIdentityProvider;

        public AggregateMaterializationStrategy(IAggregateRepository aggregateRepository, IDomainIdentityProvider domainIdentityProvider, IEnumerable<IAggregateMaterializer> aggregateMaterializers)
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
            IEnumerable<IAggregateMaterializer> materializers;

            if (!this.materializerByAggregateType.TryGetValue(
                    materializationEvent.AggregateIdentity.AggregateTypeName.Value,
                    out materializers))
            {
                materializers = Enumerable.Empty<IAggregateMaterializer>();
            }
            
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