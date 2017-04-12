using System;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.EventStore.Persistence
{
    public class SnapShotStreamName
    {
        public const string SnapshotPrefix = "Snap-";

        public SnapShotStreamName(EventNamespace eventNamespace, AggregateTypeName aggregateTypeName, Guid aggregateId)
        {
            this.EventNamespace = eventNamespace;
            this.AggregateTypeName = aggregateTypeName;
            this.AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }

        public AggregateTypeName AggregateTypeName { get; }

        public EventNamespace EventNamespace { get; }

        public static SnapShotStreamName FromAggregateIdentity(AggregateIdentity aggregateIdentity)
        {
            return new SnapShotStreamName(aggregateIdentity.EventSpace, aggregateIdentity.AggregateTypeName, aggregateIdentity.Id);
        }

        public string ToString()
        {
            return $"{SnapshotPrefix}{this.EventNamespace.Value}-{this.AggregateTypeName.Value}-{this.AggregateId}";
        }
    }
}