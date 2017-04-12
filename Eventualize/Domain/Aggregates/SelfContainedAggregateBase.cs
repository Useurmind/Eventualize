using System;
using System.Linq;

namespace Eventualize.Domain.Aggregates
{
    public abstract class SelfContainedAggregateBase : AggregateBase
    {
        public override Guid Id { get; protected set; }

        public override long Version { get; protected set; } = AggregateVersion.NotCreated;
    }
}