using System;
using System.Linq;
using System.Reflection;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Aggregates
{
    public static class AggregateExtensions
    {
        public static AggregateIdentity GetAggregateIdentity(this IAggregate aggregate, BoundedContext boundedContext)
        {
            return new AggregateIdentity(boundedContext, aggregate.GetAggregtateTypeName(), aggregate.Id);
        }

        public static AggregateTypeName GetAggregtateTypeName(this IAggregate aggregate)
        {
            return GetAggregtateTypeName(aggregate.GetType());
        }

        public static AggregateTypeName GetAggregtateTypeName(this Type aggregateType)
        {
            var aggregateTypeNameAttribute = (AggregateTypeNameAttribute)aggregateType.GetCustomAttribute(typeof(AggregateTypeNameAttribute));
            if (aggregateTypeNameAttribute == null)
            {
                throw new Exception($"The class {aggregateType.FullName} was not decorated with the attribute AggregateTypeName but is used as an aggregate. Please specify an aggregate type name for it.");
            }

            return new AggregateTypeName(aggregateTypeNameAttribute.Name);
        }
    }
}