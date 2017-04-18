using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Domain
{
    public interface IDomainIdentityProvider
    {
        BoundedContextName GetAggregateBoundedContext(Type aggregateType);

        AggregateTypeName GetAggregtateTypeName(Type aggregateType);

        EventTypeName GetEventTypeName(IEventData eventData);

        EventTypeName GetEventTypeName(Type eventType);
    }
}
