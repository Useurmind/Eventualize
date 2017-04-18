using System.Linq;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Test.TestDomain.FirstContext.MyFirstAggregates
{
    [BoundedContext(DomainNames.FirstContextName)]
    [EventTypeName("MyFirstEventWoot")]
    public class MyFirstEvent : IEventData
    {

    }
}