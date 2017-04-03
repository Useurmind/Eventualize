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
            return this.GetById<TAggregate>(bucketId, id, 0);
        }

        public TAggregate GetById<TAggregate>(string bucketId, Guid id, int version) where TAggregate : class, IAggregate
        {
            var streamName = StreamName.FromAggregateType<TAggregate>(bucketId, id);

            var resultSlice = this.connection.ReadStreamEventsForwardAsync(streamName.ToString(), 0, version, true).Result;

            var aggregate = (TAggregate)this.instanceFactory.BuildAggregate(typeof(TAggregate).GetAggregtateTypeName(), id, null);

            this.ApplyEventsToAggregate(aggregate, resultSlice);

            return aggregate;
        }

        private void ApplyEventsToAggregate(IAggregate aggregate, StreamEventsSlice resultSlice)
        {
            foreach (var @event in resultSlice.Events)
            {
                var storedEvent = @event.Event;

                var eventData = this.eventConverter.GetDomainEvent(storedEvent);
                
                aggregate.ApplyEvent(eventData);
            }
        }

        public void Save(IAggregate aggregate, Guid commitId)
        {
            this.Save(DefaultBucketId, aggregate, commitId);
        }

        public void Save(string bucketId, IAggregate aggregate, Guid commitId)
        {
            var streamName = new StreamName(bucketId, aggregate.GetAggregtateTypeName(), aggregate.Id);

            var eventDatas = this.ExtractEventData(aggregate);

            var expectedVersion = aggregate.CommittedVersion == 0 ? ExpectedVersion.NoStream : aggregate.CommittedVersion;

            this.connection.AppendToStreamAsync(streamName.ToString(), expectedVersion, eventDatas).Wait();
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
