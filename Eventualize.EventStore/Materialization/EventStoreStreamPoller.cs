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
        private IAggregateFactory aggregateFactory;

        private IEventStoreConnection connection;

        private IEventStoreEventConverter eventConverter;

        private EventStoreStreamCatchUpSubscription subscription;

        private EventStreamIndex? startAfterEventIndex;

        private string streamName;
        
        public EventStoreStreamPoller(IAggregateFactory aggregateFactory, IEventStoreEventConverter eventConverter, IEventStoreConnection connection, EventStreamIndex? startAfterEventIndex, string streamName)
        {
            this.connection = connection;
            this.aggregateFactory = aggregateFactory;
            this.eventConverter = eventConverter;
            this.startAfterEventIndex = startAfterEventIndex;
            this.streamName = streamName;
        }

        public IDisposable Subscribe(IObserver<IEvent> observer)
        {
            this.subscription = this.connection.SubscribeToStreamFrom(
                this.streamName,
                this.startAfterEventIndex.HasValue ? this.startAfterEventIndex.Value.Value : (long?)null,
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