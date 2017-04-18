using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Persistence;
using Eventualize.Persistence;

using NEventStore;
using NEventStore.Persistence;

namespace Eventualize.NEventStore.Persistence
{
    public class AggregateNEventStoreImplementation : IAggregateEventStore
    {
        private const string AggregateTypeHeader = "AggregateType";
        
        private IStoreEvents eventStore;

        public AggregateNEventStoreImplementation(IStoreEvents eventStore)
        {
            this.eventStore = eventStore;
        }

        public void Dispose()
        {
            
        }

        public IEnumerable<IAggregateEvent> GetEvents(AggregateIdentity aggregateIdentity, long start, long end)
        {
            using (IEventStream stream = this.eventStore.OpenStream(NEventStoreBuckets.Aggregates, aggregateIdentity.Id, (int)start, (int)end))
            {
                return stream.CommittedEvents.Select((x, index) => NEventStoreEventConverter.CreateAggregateEvent(aggregateIdentity, Guid.Empty, x, index));
            }
        }

        public void AppendEvents(AggregateIdentity aggregateIdentity, long expectedAggregateVersion, IEnumerable<IEventData> newAggregateEvents, Guid replayId)
        {
            Dictionary<string, object> headers = PrepareHeaders(aggregateIdentity);
            while (true)
            {
                using (IEventStream stream = this.PrepareStream(NEventStoreBuckets.Aggregates, aggregateIdentity, expectedAggregateVersion, headers))
                {
                    foreach (var item in headers)
                    {
                        stream.UncommittedHeaders[item.Key] = item.Value;
                    }

                    newAggregateEvents.Cast<object>()
                                      .Select(x => new EventMessage { Body = x })
                                      .ToList()
                                      .ForEach(stream.Add);

                    int commitEventCount = stream.CommittedEvents.Count;

                    try
                    {
                        stream.CommitChanges(replayId);
                    }
                    catch (DuplicateCommitException)
                    {
                        stream.ClearChanges();
                    }
                    catch (ConcurrencyException e)
                    {
                        throw;
                    }
                    catch (StorageException e)
                    {
                        throw;
                    }
                }
            }
        }

        private IEventStream PrepareStream(string bucketId, AggregateIdentity aggregateIdentity, long expectedAggregateVersion, Dictionary<string, object> headers)
        {
            IEventStream stream;

            if (expectedAggregateVersion != AggregateVersion.NotCreated)
            {
                stream = this.eventStore.OpenStream(bucketId, aggregateIdentity.Id);
            }
            else
            {
                stream = this.eventStore.CreateStream(bucketId, aggregateIdentity.Id);
            }

            return stream;
        }

        private static Dictionary<string, object> PrepareHeaders(
            AggregateIdentity aggregate)
        {
            var headers = new Dictionary<string, object>();

            headers[AggregateTypeHeader] = aggregate.AggregateTypeName;

            return headers;
        }
    }
}