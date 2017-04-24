using System;
using System.Linq;
using System.Reactive.Linq;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Materialization.ReactiveStreams
{
    public static class EventSourceExtensions
    {
        /// <summary>
        /// Filter out specific events.
        /// </summary>
        /// <param name="eventSource">The event source to filter.</param>
        /// <param name="predicate">A predicate that returns true for all events that should be returned.</param>
        /// <returns></returns>
        public static IEventSource Where(this IEventSource eventSource, Func<IEvent, bool> predicate)
        {
            var observable = eventSource as IObservable<IEvent>;
            return new WrapperEventSource(observable.Where(predicate));
        }

        /// <summary>
        /// Filter out specific events.
        /// </summary>
        /// <param name="eventSource">The event source to filter.</param>
        /// <param name="predicate">A predicate that returns true for all events that should be returned.</param>
        public static IAggregateEventSource Where(this IAggregateEventSource eventSource, Func<IAggregateEvent, bool> predicate)
        {
            var observable = eventSource as IObservable<IAggregateEvent>;
            return new WrapperAggregateEventSource(observable.Where(predicate));
        }

        /// <summary>
        /// Skip all events until including a specified index.
        /// </summary>
        /// <param name="eventSource">The event source in which to skip events.</param>
        /// <param name="eventStreamIndex">The last event stream index that should be skipped.</param>
        /// <returns></returns>
        public static IEventSource SkipAfter(this IEventSource eventSource, EventStreamIndex? eventStreamIndex)
        {
            var observable = eventSource as IObservable<IEvent>;
            if (eventStreamIndex.HasValue)
            {
                observable = observable.SkipWhile(x => x.StoreIndex >= eventStreamIndex.Value.Value);
            }
            return new WrapperEventSource(observable);
        }

        /// <summary>
        /// Skip all events until including a specified index.
        /// </summary>
        /// <param name="eventSource">The event source in which to skip events.</param>
        /// <param name="eventStreamIndex">The last event stream index that should be skipped.</param>
        /// <returns></returns>
        public static IAggregateEventSource SkipAfter(this IAggregateEventSource eventSource, EventStreamIndex? eventStreamIndex)
        {
            var observable = eventSource as IObservable<IAggregateEvent>;
            if (eventStreamIndex.HasValue)
            {
                observable = observable.SkipWhile(x => x.StoreIndex >= eventStreamIndex.Value.Value);
            }
            return new WrapperAggregateEventSource(observable);
        }

        /// <summary>
        /// Convert the given event source into a source of aggregate events filtering out any non aggregate events in the process.
        /// </summary>
        /// <param name="eventSource">The event source to convert.</param>
        /// <returns></returns>
        public static IAggregateEventSource AsAggregateEventSource(this IEventSource eventSource)
        {
            var observable = eventSource as IObservable<IEvent>;
            return new WrapperAggregateEventSource(observable.Where(x => x is IAggregateEvent).Cast<IAggregateEvent>());
        }
    }
}