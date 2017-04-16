using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Aggregates
{
    public interface IDomainIdentityProvider
    {
        BoundedContext GetAggregateBoundedContext(Type aggregateType);

        AggregateTypeName GetAggregtateTypeName(Type aggregateType);

        EventType GetEventTypeName(IEventData eventData);

        EventType GetEventTypeName(Type eventType);
    }
}
