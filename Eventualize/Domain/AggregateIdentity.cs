using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Core;

namespace Eventualize.Domain
{
    public struct AggregateIdentity
    {
        public Guid Id;

        public EventNamespace EventSpace;

        public AggregateTypeName AggregateTypeName;

        public AggregateIdentity(EventNamespace eventNamespace, AggregateTypeName aggregateTypeName, Guid aggregateId)
        {
            this.AggregateTypeName = aggregateTypeName;
            this.EventSpace = eventNamespace;
            this.Id = aggregateId;
        }

        public static AggregateIdentity FromAggregate(IAggregate aggregate, EventNamespace eventNamespace)
        {
            return aggregate.GetAggregateIdentity(eventNamespace);
        }

        public static AggregateIdentity FromAggregateType(Type aggregateType, Guid id, EventNamespace eventNamespace)
        {
            return new AggregateIdentity(eventNamespace, aggregateType.GetAggregtateTypeName(), id);
        }
    }
}
