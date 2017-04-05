using System;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.Persistence
{
    public class AggregateRepository : IAggregateRepository
    {
        private IAggregateEventStore eventStore;

        private IConstructInstances aggregateFactory;

        public AggregateRepository(IAggregateEventStore eventStore, IConstructInstances aggregateFactory)
        {
            this.eventStore = eventStore;
            this.aggregateFactory = aggregateFactory;
        }

        public void Dispose()
        {
        }

        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            return this.GetById<TAggregate>(id, AggregateVersion.Latest);
        }

        public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
        {
            return (TAggregate)this.GetById(
                new AggregateIdentity
                {
                    Id = id,
                    AggregateTypeName = typeof(TAggregate).GetAggregtateTypeName()
                });
        }

        public IAggregate GetById(AggregateIdentity aggregateIdentity)
        {
            return this.GetById(aggregateIdentity, AggregateVersion.Latest);
        }

        public IAggregate GetById(AggregateIdentity aggregateIdentity, int version)
        {
            var events = this.eventStore.GetEvents(aggregateIdentity, 0, version);
            return this.aggregateFactory.BuildAggregate(aggregateIdentity, null, events.Select(x => x.EventData));
        }

        public void Save(IAggregate aggregate, Guid replayGuid)
        {
            var uncommitedEvents = aggregate.GetUncommittedEvents().Cast<IEventData>();

            this.eventStore.AppendEvents(aggregate.GetAggregateIdentity(), aggregate.CommittedVersion, uncommitedEvents, replayGuid);

            aggregate.ClearUncommittedEvents();
        }

        public IAggregate Refresh(IAggregate aggregate)
        {
            throw new NotImplementedException();
        }
    }
}