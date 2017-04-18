using System;
using System.Linq;
using System.Reflection;

using Eventualize.Domain.Aggregates;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Domain.Events
{
    public static class EventExtensions
    {
        public static EventTypeName GetEventTypeName(this IEventData eventData)
        {
            return GetEventTypeName(eventData.GetType());
        }

        public static EventTypeName GetEventTypeName(this Type eventType)
        {
            var eventTypeNameAttribute = (EventTypeNameAttribute)eventType.GetCustomAttribute(typeof(EventTypeNameAttribute));
            if (eventTypeNameAttribute == null)
            {
                throw new Exception($"The class {eventType.FullName} was not decorated with the attribute EventTypeName but is used as an event. Please specify an event type name for it.");
            }

            return new EventTypeName(eventTypeNameAttribute.Name);
        }
    }
}