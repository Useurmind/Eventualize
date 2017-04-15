using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Aggregates
{
    public interface IAggregateEvent : IEvent
    {
        AggregateIdentity AggregateIdentity { get; }
    }
}