using System;
using System.Linq;

using Eventualize.Interfaces.Aggregates;

namespace Eventualize.Interfaces.BaseTypes
{
    public struct AggregateIdentity
    {
        public Guid Id { get; }

        public BoundedContextName BoundedContextName { get; }

        public AggregateTypeName AggregateTypeName { get; }

        public AggregateIdentity(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName, Guid aggregateId)
        {
            this.AggregateTypeName = aggregateTypeName;
            this.BoundedContextName = boundedContextName;
            this.Id = aggregateId;
        }

        public override string ToString()
        {
            return $"{this.BoundedContextName}.{this.AggregateTypeName}.{this.Id}";
        }

        public static bool operator ==(AggregateIdentity obj1, AggregateIdentity obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(AggregateIdentity obj1, AggregateIdentity obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}
