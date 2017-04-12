using System;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Core;
using Eventualize.Persistence.Snapshots;
using Eventualize.Security;

namespace Eventualize.Persistence
{
    public class AggregateRepository : IAggregateRepository
    {
        private IAggregateEventStore eventStore;

        private IConstructInstances aggregateFactory;

        private ISnapShotStore snapShotStore;

        public AggregateRepository(IAggregateEventStore eventStore, IConstructInstances aggregateFactory, ISnapShotStore snapShotStore)
        {
            this.eventStore = eventStore;
            this.aggregateFactory = aggregateFactory;
            this.snapShotStore = snapShotStore;
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
            return
                (TAggregate)
                this.GetById(
                    new AggregateIdentity(EventualizeContext.Current.DefaultEventNamespace, typeof(TAggregate).GetAggregtateTypeName(), id));
        }

        public IAggregate GetById(AggregateIdentity aggregateIdentity)
        {
            return this.GetById(aggregateIdentity, AggregateVersion.Latest);
        }

        public IAggregate GetById(AggregateIdentity aggregateIdentity, int version)
        {
            var snapShot = this.snapShotStore.GetSnapshot(aggregateIdentity);
            var startVersion = snapShot == null ? 0 : snapShot.Version + 1;
            var events = this.eventStore.GetEvents(aggregateIdentity, startVersion, version);
            return this.aggregateFactory.BuildAggregate(aggregateIdentity, snapShot, events.Select(x => x.EventData));
        }

        public void Save(IAggregate aggregate, Guid replayGuid)
        {
            var aggregateIdentity = aggregate.GetAggregateIdentity(EventualizeContext.Current.DefaultEventNamespace);
            var uncommitedEvents = aggregate.GetUncommittedEvents().Cast<IEventData>();

            this.eventStore.AppendEvents(aggregateIdentity, aggregate.CommittedVersion, uncommitedEvents, replayGuid);

            aggregate.ClearUncommittedEvents();
        }

        public IAggregate Refresh(IAggregate aggregate)
        {
            throw new NotImplementedException();
        }
    }
}