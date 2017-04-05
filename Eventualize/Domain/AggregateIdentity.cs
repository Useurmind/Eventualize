using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain.Core;

namespace Eventualize.Domain
{
    public struct AggregateIdentity
    {
        public Guid Id;

        public string AggregateTypeName;

        public static AggregateIdentity FromAggregate(IAggregate aggregate)
        {
            return aggregate.GetAggregateIdentity();
        }

        public static AggregateIdentity FromAggregateType(Type aggregateType, Guid id)
        {
            return new AggregateIdentity()
                   {
                       Id = id,
                       AggregateTypeName = aggregateType.GetAggregtateTypeName()
                   };
        }
    }
}
