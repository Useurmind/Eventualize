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
        BoundedContextName GetAggregateBoundedContext(Type aggregateType);

        AggregateTypeName GetAggregtateTypeName(Type aggregateType);

        EventTypeName GetEventTypeName(IEventData eventData);

        EventTypeName GetEventTypeName(Type eventType);
    }
}
