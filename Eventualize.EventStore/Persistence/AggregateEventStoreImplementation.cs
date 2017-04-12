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
    public class AggregateEventStoreImplementation : IAggregateEventStore
    {
        private const int MaxPageSize = 4096; // limitation of event store
        private IEventStoreConnection connection;

        private IEventStoreEventConverter eventConverter;

        public AggregateEventStoreImplementation(IEventStoreConnection connection, IEventStoreEventConverter eventConverter)
        {
            this.connection = connection;
            this.eventConverter = eventConverter;
        }

        public void Dispose()
        {
        }

        public IEnumerable<IAggregateEvent> GetEvents(AggregateIdentity aggregateIdentity, long start, long end)
        {
            var streamName = StreamName.FromAggregateIdentity(aggregateIdentity);

            if (end == AggregateVersion.Latest)
            {
                end = MaxPageSize;
            }

            var count = (int)(end - start);
            if (count > MaxPageSize)
            {
                throw new NotImplementedException($"Currently no paging is implemtend, so we cannot retrieve more than {MaxPageSize} events for an aggregate");
            }

            var resultSlice = this.connection.ReadStreamEventsForwardAsync(streamName.ToString(), start, count, true).Result;

            var domainEvents = resultSlice.Events.Select(resolvedEvent => this.eventConverter.GetDomainEvent(aggregateIdentity, resolvedEvent.Event));

            return domainEvents;
        }

        public void AppendEvents(AggregateIdentity aggregateIdentity, long expectedAggregateVersion, IEnumerable<IEventData> newAggregateEvents, Guid replayId)
        {
            var streamName = StreamName.FromAggregateIdentity(aggregateIdentity);
            
            var eventDatas = newAggregateEvents.Select(x => this.eventConverter.GetEventData(x));

            var expectedVersion = expectedAggregateVersion == AggregateVersion.NotCreated ? ExpectedVersion.NoStream : expectedAggregateVersion;

            var result = this.connection.AppendToStreamAsync(streamName.ToString(), expectedVersion, eventDatas).Result;
        }
    }
}
