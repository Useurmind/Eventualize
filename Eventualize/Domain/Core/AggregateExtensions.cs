using System;
using System.Linq;
using System.Reflection;

namespace Eventualize.Domain.Core
{
    public static class AggregateExtensions
    {
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

    public static class EventExtensions
    {
        public static string GetEventTypeName(this IEventData eventData)
        {
            return GetEventTypeName(eventData.GetType());
        }

        public static string GetEventTypeName(this Type eventType)
        {
            var eventTypeNameAttribute = (EventTypeNameAttribute)eventType.GetCustomAttribute(typeof(EventTypeNameAttribute));
            if (eventTypeNameAttribute == null)
            {
                throw new Exception($"The class {eventType.FullName} was not decorated with the attribute EventTypeName but is used as an event. Please specify an event type name for it.");
            }

            return eventTypeNameAttribute.Name;
        }
    }
}