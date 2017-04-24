using System;
using System.Linq;
using System.Reactive.Disposables;

using EventStore.ClientAPI;

using Eventualize.EventStore.Persistence;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Materialization.ReactiveStreams;

namespace Eventualize.EventStore.Materialization
{
    public class EventStoreStreamPoller : IEventSource
    {
        private IEventStoreConnection connection;

        private IEventStoreEventConverter eventConverter;

        private EventStoreStreamCatchUpSubscription subscription;

        private EventStreamIndex startAfterEventIndex;

        private string streamName;
        
        public EventStoreStreamPoller(IEventStoreEventConverter eventConverter, IEventStoreConnection connection, string streamName)
        {
            this.connection = connection;
            this.eventConverter = eventConverter;
            this.startAfterEventIndex = EventStreamIndex.Start();
            this.streamName = streamName;
        }

        public void AddAdditionalEventStreamIndex(EventStreamIndex? newEventIndex)
        {
            if (!newEventIndex.HasValue)
            {
                newEventIndex = EventStreamIndex.Start();
            }

            this.startAfterEventIndex = EventStreamIndex.Min(this.startAfterEventIndex, newEventIndex.Value);
        }

        public IDisposable Subscribe(IObserver<IEvent> observer)
        {
            this.subscription = this.connection.SubscribeToStreamFrom(
                this.streamName,
                this.startAfterEventIndex == EventStreamIndex.Start() ? (long?)null : this.startAfterEventIndex.Value,
                new CatchUpSubscriptionSettings(100, 50, false, true),
                (subscription, resolvedevent) =>
                    {
                        var recordedEvent = resolvedevent.Event;
                        if (recordedEvent == null)
                        {
                            throw new Exception();
                        }

                        var streamName = AggregateStreamName.FromStreamName(recordedEvent.EventStreamId);
                        var aggregateEvent = this.eventConverter.GetDomainEvent(streamName.GetAggregateIdentity(), recordedEvent, resolvedevent.OriginalEventNumber);

                        observer.OnNext(aggregateEvent);
                    });

            return Disposable.Create(() => { this.subscription.Stop(); });
        }
    }
}