using System;

using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.EventStore.Persistence
{
    public class StreamName
    {
        public StreamName(string buckedIt, string aggregateTypeName, Guid aggregateId)
        {
            this.BucketId = buckedIt;
            this.AggregateTypeName = aggregateTypeName;
            this.AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }

        public string AggregateTypeName { get; }

        public string BucketId { get; }

        public static StreamName FromStreamName(string streamName)
        {
            var streamNameParts = streamName.Split('-');
            Guid aggregateId;

            if (streamNameParts.Length < 3 || !Guid.TryParse(streamNameParts[2], out aggregateId))
            {
                throw new Exception($"Stream name {streamName} is not valid. A stream name must have the form <BucketId>-<AggregateTypeName>-<AggregateGuid>");
            }

            return new StreamName(streamNameParts[0], streamNameParts[1], aggregateId);
        }

        public static StreamName FromAggregateType<TAggregate>(string bucketId, Guid id) where TAggregate : class, IAggregate
        {
            return new StreamName(bucketId, typeof(TAggregate).GetAggregtateTypeName(), id);
        }

        public string ToString()
        {
            return $"{this.BucketId}-{this.AggregateTypeName}-{this.AggregateId}";
        }
    }
}