using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.ClientAPI;

using Eventualize.Domain;
using Eventualize.Domain.Core;
using Eventualize.Persistence;

using Newtonsoft.Json;

namespace Eventualize.EventStore.Persistence
{
    public class EventStoreRepository : IRepository
    {
        private const string DefaultBucketId = "Default";

        private const int MaxPageSize = 4096; // limitation of event store

        private IEventStoreConnection connection;

        private IConstructInstances instanceFactory;

        private IEventStoreEventConverter eventConverter;

        public EventStoreRepository(IEventStoreConnection connection, IEventStoreEventConverter eventConverter, IConstructInstances instanceFactory)
        {
            this.connection = connection;
            this.instanceFactory = instanceFactory;
            this.eventConverter = eventConverter;
        }

        public TAggregate GetById<TAggregate>(Guid id) where TAggregate : class, IAggregate
        {
            return this.GetById<TAggregate>(DefaultBucketId, id);
        }

        public TAggregate GetById<TAggregate>(Guid id, int version) where TAggregate : class, IAggregate
        {
            return this.GetById<TAggregate>(DefaultBucketId, id, version);
        }

        public TAggregate GetById<TAggregate>(string bucketId, Guid id) where TAggregate : class, IAggregate
        {
            return this.GetById<TAggregate>(bucketId, id, MaxPageSize);
        }

        public TAggregate GetById<TAggregate>(string bucketId, Guid id, int version) where TAggregate : class, IAggregate
        {
            var streamName = StreamName.FromAggregateType<TAggregate>(bucketId, id);

            var resultSlice = this.connection.ReadStreamEventsForwardAsync(streamName.ToString(), 0, version, true).Result;

            var domainEvents = resultSlice.Events.Select(resolvedEvent => this.eventConverter.GetDomainEvent(resolvedEvent.Event));

            var aggregateIdentity = new AggregateIdentity()
                                    {
                                        Id = id,
                                        AggregateTypeName = typeof(TAggregate).GetAggregtateTypeName()
            };

            var aggregate = (TAggregate)this.instanceFactory.BuildAggregate(aggregateIdentity, null, domainEvents);

            return aggregate;
        }

        public void Save(IAggregate aggregate, Guid commitId)
        {
            this.Save(DefaultBucketId, aggregate, commitId);
        }

        public void Save(string bucketId, IAggregate aggregate, Guid commitId)
        {
            var streamName = new StreamName(bucketId, aggregate.GetAggregtateTypeName(), aggregate.Id);

            var eventDatas = this.ExtractEventData(aggregate);

            var expectedVersion = aggregate.CommittedVersion == 0 ? ExpectedVersion.NoStream : aggregate.CommittedVersion-1;

            var result = this.connection.AppendToStreamAsync(streamName.ToString(), expectedVersion, eventDatas).Result;
        }

        private IEnumerable<EventData> ExtractEventData(IAggregate aggregate)
        {
            return aggregate.GetUncommittedEvents()
                     .Cast<IEventData>()
                     .Select(
                         ed =>
                             this.eventConverter.GetEventData(ed));
        }

        public void Dispose()
        {
            
        }
    }
}
