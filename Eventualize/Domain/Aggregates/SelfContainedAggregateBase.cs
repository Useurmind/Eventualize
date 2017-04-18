using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Domain.Aggregates
{
    public abstract class SelfContainedAggregateBase : AggregateBase
    {
        public override Guid Id { get; protected set; }

        public override AggregateVersion Version { get; protected set; } = AggregateVersion.NotCreated();
    }
}