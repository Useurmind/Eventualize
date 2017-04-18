using System.Linq;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.Aggregates;

namespace Eventualize.Test.TestDomain.FirstContext.MyFirstAggregates
{
    [BoundedContext(DomainNames.FirstContextName)]
    [EventTypeName("MyFirstEventWoot")]
    public class MyFirstEvent : IEventData
    {

    }
}