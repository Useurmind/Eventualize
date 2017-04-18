using System;
using System.Linq;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.EventStore.Persistence
{
    public class SnapShotStreamName
    {
        public const string SnapshotPrefix = "Snap-";

        public SnapShotStreamName(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName, Guid aggregateId)
        {
            this.BoundedContextName = boundedContextName;
            this.AggregateTypeName = aggregateTypeName;
            this.AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }

        public AggregateTypeName AggregateTypeName { get; }

        public BoundedContextName BoundedContextName { get; }

        public static SnapShotStreamName FromAggregateIdentity(AggregateIdentity aggregateIdentity)
        {
            return new SnapShotStreamName(aggregateIdentity.BoundedContextName, aggregateIdentity.AggregateTypeName, aggregateIdentity.Id);
        }

        public string ToString()
        {
            return $"{SnapshotPrefix}{this.BoundedContextName.Value}-{this.AggregateTypeName.Value}-{this.AggregateId}";
        }
    }
}