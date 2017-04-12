using System;
using System.Linq;
using System.Reflection;

namespace Eventualize.Domain.Core
{
    public static class EventExtensions
    {
        public static EventType GetEventTypeName(this IEventData eventData)
        {
            return GetEventTypeName(eventData.GetType());
        }

        public static EventType GetEventTypeName(this Type eventType)
        {
            var eventTypeNameAttribute = (EventTypeNameAttribute)eventType.GetCustomAttribute(typeof(EventTypeNameAttribute));
            if (eventTypeNameAttribute == null)
            {
                throw new Exception($"The class {eventType.FullName} was not decorated with the attribute EventTypeName but is used as an event. Please specify an event type name for it.");
            }

            return new EventType(eventTypeNameAttribute.Name);
        }
    }
}