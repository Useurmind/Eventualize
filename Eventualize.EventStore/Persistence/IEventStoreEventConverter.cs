using System.Linq;

using EventStore.ClientAPI;

using Eventualize.Domain;

namespace Eventualize.EventStore.Persistence
{
    public interface IEventStoreEventConverter
    {
        IAggregateEvent GetDomainEvent(AggregateIdentity aggregateIdentity, RecordedEvent recordedEvent);

        IEvent GetDomainEvent(RecordedEvent recordedEvent);

        EventData GetEventData(IEventData eventData);
    }
}