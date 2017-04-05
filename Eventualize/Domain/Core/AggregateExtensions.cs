using System;
using System.Linq;
using System.Reflection;

namespace Eventualize.Domain.Core
{
    public static class AggregateExtensions
    {
        public static AggregateIdentity GetAggregateIdentity(this IAggregate aggregate)
        {
            return new AggregateIdentity()
            {
                Id = aggregate.Id,
                AggregateTypeName = aggregate.GetAggregtateTypeName()
            };
        }

        public static string GetAggregtateTypeName(this IAggregate aggregate)
        {
            return GetAggregtateTypeName(aggregate.GetType());
        }

        public static string GetAggregtateTypeName(this Type aggregateType)
        {
            var aggregateTypeNameAttribute = (AggregateTypeNameAttribute)aggregateType.GetCustomAttribute(typeof(AggregateTypeNameAttribute));
            if (aggregateTypeNameAttribute == null)
            {
                throw new Exception($"The class {aggregateType.FullName} was not decorated with the attribute AggregateTypeName but is used as an aggregate. Please specify an aggregate type name for it.");
            }

            return aggregateTypeNameAttribute.Name;
        }
    }
}