using System.Linq;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.Aggregates;

namespace Eventualize.Test.TestDomain.FirstContext.MySecondAggregates
{
    [BoundedContext(DomainNames.FirstContextName)]
    public class MySecondEvent : IEventData
    {

    }
}