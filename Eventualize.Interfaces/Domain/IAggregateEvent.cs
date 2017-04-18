using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Domain
{
    public interface IAggregateEvent : IEvent
    {
        AggregateIdentity AggregateIdentity { get; }
    }
}