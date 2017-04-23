using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain.Events;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Persistence;
using Eventualize.Materialization.ReactiveStreams;
using Eventualize.Security;

namespace Eventualize.Persistence
{
    /// <summary>
    /// This is an in memory event store.
    /// Its main purpose is unit testing. 
    /// This class is not intended for productive use at the moment.
    /// </summary>
    public class InMemoryAggregateEventStore : IAggregateEventStore, IEventSource
    {
        private long nextStoreIndex = 0;
        private IDictionary<AggregateIdentity, LinkedList<IAggregateEvent>> eventsByAggregate;

        private IDomainIdentityProvider domainIdentityProvider;

        private ISubject<IEvent> eventSubject;

        public InMemoryAggregateEventStore(IDomainIdentityProvider domainIdentityProvider)
        {
            this.eventsByAggregate = new Dictionary<AggregateIdentity, LinkedList<IAggregateEvent>>();
            this.domainIdentityProvider = domainIdentityProvider;
            this.eventSubject = new Subject<IEvent>();
        }

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public IEnumerable<IAggregateEvent> GetEvents(AggregateIdentity aggregateIdentity, AggregateVersion start, AggregateVersion end)
        {
            var eventList = this.GetAggregateEventList(aggregateIdentity);

            var take = end == AggregateVersion.Latest() ? int.MaxValue : (int)(end.Value - start.Value + 1);

            return eventList.Skip((int)start.Value).Take(take);
        }

        /// <inheritdoc />
        public void AppendEvents(
            AggregateIdentity aggregateIdentity,
            AggregateVersion expectedAggregateVersion,
            IEnumerable<IEventData> newAggregateEvents,
            Guid replayId)
        {
            var eventList = GetAggregateEventList(aggregateIdentity);

            if (!eventList.Any() && expectedAggregateVersion != AggregateVersion.NotCreated()
                && expectedAggregateVersion != AggregateVersion.Any())
            {
                throw new ExpectedAggregateVersionException($"Exptected stream but found no events");
            }
            else if (eventList.Any() && expectedAggregateVersion != new AggregateVersion(eventList.Count - 1))
            {
                throw new ExpectedAggregateVersionException(
                    $"Exptected stream with {expectedAggregateVersion.Value + 1} events but found {eventList.Count} events");
            }

            foreach (var eventData in newAggregateEvents)
            {
                eventList.AddLast(
                    new AggregateEvent(
                        (long)this.nextStoreIndex++,
                        aggregateIdentity.BoundedContextName,
                        replayId,
                        this.domainIdentityProvider.GetEventTypeName(eventData),
                        DateTime.Now,
                        EventualizeContext.Current.CurrentUser.UserId,
                        eventData,
                        new EventStreamIndex(eventList.Count),
                        aggregateIdentity));
            }

            this.PublishNewEvents(newAggregateEvents, eventList);
        }

        private void PublishNewEvents(IEnumerable<IEventData> newAggregateEvents, LinkedList<IAggregateEvent> eventList)
        {
            var newEvents = eventList.Skip(eventList.Count - newAggregateEvents.Count());

            foreach (var newEvent in newEvents)
            {
                this.eventSubject.OnNext(newEvent);
            }
        }

        private LinkedList<IAggregateEvent> GetAggregateEventList(AggregateIdentity aggregateIdentity)
        {
            LinkedList<IAggregateEvent> eventList = null;
            if (!this.eventsByAggregate.TryGetValue(aggregateIdentity, out eventList))
            {
                eventList = new LinkedList<IAggregateEvent>();
                this.eventsByAggregate.Add(aggregateIdentity, eventList);
            }

            return eventList;
        }

        public IDisposable Subscribe(IObserver<IEvent> observer)
        {
            return this.eventSubject.Subscribe(observer);
        }
    }
}
