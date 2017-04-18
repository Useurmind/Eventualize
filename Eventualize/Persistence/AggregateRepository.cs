using System;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Persistence;
using Eventualize.Interfaces.Snapshots;
using Eventualize.Security;
using Eventualize.Snapshots;

namespace Eventualize.Persistence
{
    public class AggregateRepository : IAggregateRepository
    {
        private IAggregateEventStore eventStore;

        private IAggregateFactory aggregateFactory;

        private ISnapShotStore snapShotStore;

        private IDomainIdentityProvider domainIdentityProvider;

        private RepositoryOptions repositoryOptions;

        public AggregateRepository(IAggregateEventStore eventStore, IDomainIdentityProvider domainIdentityProvider, IAggregateFactory aggregateFactory, ISnapShotStore snapShotStore, RepositoryOptions repositoryOptions)
        {
            this.eventStore = eventStore;
            this.aggregateFactory = aggregateFactory;
            this.snapShotStore = snapShotStore;
            this.domainIdentityProvider = domainIdentityProvider;
            this.repositoryOptions = repositoryOptions;
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
            var aggregateIdentity = this.domainIdentityProvider.GetAggregateIdentity<TAggregate>(id);

            return (TAggregate)this.GetById(aggregateIdentity);
        }

        public IAggregate GetById(AggregateIdentity aggregateIdentity)
        {
            return this.GetById(aggregateIdentity, AggregateVersion.Latest);
        }

        public IAggregate GetById(AggregateIdentity aggregateIdentity, int version)
        {
            var snapShot = this.snapShotStore.GetSnapshot(aggregateIdentity);
            var aggregate = this.aggregateFactory.BuildAggregate(aggregateIdentity, snapShot, Enumerable.Empty<IEventData>());
            var startVersion = snapShot == null ? 0 : snapShot.Version + 1;

            var pagedEventLoader=  new PagedEventLoader();
            var pagedEventLoaderOptions = new PageEventLoaderOptions()
                                              {
                                                  PageSize = this.repositoryOptions.PageSize,
                                                  StartEventNumber = startVersion,
                                                  EndEventNumber = version
                                              };

            // we load the events in pages because it is unreasonable to assume we can always load them in one chunk
            pagedEventLoader.LoadAllPages(
                this.eventStore,
                aggregateIdentity, 
                pagedEventLoaderOptions, 
                aggregateEvent =>
                    {
                        aggregate.ApplyEvent(aggregateEvent.EventData);
                    });

            return aggregate;
        }

        public void Save(IAggregate aggregate, Guid replayGuid)
        {
            var aggregateIdentity = this.domainIdentityProvider.GetAggregateIdentity(aggregate);
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