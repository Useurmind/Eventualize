using System.Linq;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Test.TestDomain.FirstContext.MySecondAggregates
{
    [BoundedContext(DomainNames.FirstContextName)]
    public class MySecondEvent : IEventData
    {

    }
}