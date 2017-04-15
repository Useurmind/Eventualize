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

        public SnapShotStreamName(BoundedContext boundedContext, AggregateTypeName aggregateTypeName, Guid aggregateId)
        {
            this.BoundedContext = boundedContext;
            this.AggregateTypeName = aggregateTypeName;
            this.AggregateId = aggregateId;
        }

        public Guid AggregateId { get; }

        public AggregateTypeName AggregateTypeName { get; }

        public BoundedContext BoundedContext { get; }

        public static SnapShotStreamName FromAggregateIdentity(AggregateIdentity aggregateIdentity)
        {
            return new SnapShotStreamName(aggregateIdentity.BoundedContext, aggregateIdentity.AggregateTypeName, aggregateIdentity.Id);
        }

        public string ToString()
        {
            return $"{SnapshotPrefix}{this.BoundedContext.Value}-{this.AggregateTypeName.Value}-{this.AggregateId}";
        }
    }
}