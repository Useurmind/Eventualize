using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Domain
{
    public class AttributeBasedIdentityProvider : IDomainIdentityProvider
    {
        public BoundedContextName GetAggregateBoundedContext(Type aggregateType)
        {
            var boundedContextAttribute = (BoundedContextAttribute)aggregateType.GetCustomAttribute(typeof(BoundedContextAttribute));
            if (boundedContextAttribute == null)
            {
                throw new Exception($"The class {aggregateType.FullName} was not decorated with the attribute BoundedContext but is used as an aggregate. Please specify a bounded context for it.");
            }

            return new BoundedContextName(boundedContextAttribute.Name);
        }

        public AggregateTypeName GetAggregtateTypeName(Type aggregateType)
        {
            var aggregateTypeNameAttribute = (AggregateTypeNameAttribute)aggregateType.GetCustomAttribute(typeof(AggregateTypeNameAttribute));
            if (aggregateTypeNameAttribute == null)
            {
                throw new Exception($"The class {aggregateType.FullName} was not decorated with the attribute AggregateTypeName but is used as an aggregate. Please specify an aggregate type name for it.");
            }

            return new AggregateTypeName(aggregateTypeNameAttribute.Name);
        }

        public EventTypeName GetEventTypeName(IEventData eventData)
        {
            return this.GetEventTypeName(eventData.GetType());
        }

        public EventTypeName GetEventTypeName(Type eventType)
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
