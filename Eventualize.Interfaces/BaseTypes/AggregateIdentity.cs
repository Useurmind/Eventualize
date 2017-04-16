using System;
using System.Linq;

using Eventualize.Interfaces.Aggregates;

namespace Eventualize.Interfaces.BaseTypes
{
    public struct AggregateIdentity
    {
        public Guid Id { get; }

        public BoundedContext BoundedContext { get; }

        public AggregateTypeName AggregateTypeName { get; }

        public AggregateIdentity(BoundedContext boundedContext, AggregateTypeName aggregateTypeName, Guid aggregateId)
        {
            this.AggregateTypeName = aggregateTypeName;
            this.BoundedContext = boundedContext;
            this.Id = aggregateId;
        }

        public override string ToString()
        {
            return $"{this.BoundedContext}.{this.AggregateTypeName}.{this.Id}";
        }
    }
}
