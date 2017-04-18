using System.Linq;

using EventStore.ClientAPI;

using Eventualize.Domain;
using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;

namespace Eventualize.EventStore.Persistence
{
    public interface IEventStoreEventConverter
    {
        IAggregateEvent GetDomainEvent(AggregateIdentity aggregateIdentity, RecordedEvent recordedEvent);

        IEvent GetDomainEvent(RecordedEvent recordedEvent, BoundedContextName boundedContextName);

        EventData GetEventData(IEventData eventData);
    }
}