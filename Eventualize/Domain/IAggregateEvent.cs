using System.Linq;

namespace Eventualize.Domain
{
    public interface IAggregateEvent : IEvent
    {
        AggregateIdentity AggregateIdentity { get; }
    }
}