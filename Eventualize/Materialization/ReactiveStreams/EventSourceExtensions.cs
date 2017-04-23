using System;
using System.Linq;
using System.Reactive.Linq;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;

namespace Eventualize.Materialization.ReactiveStreams
{
    public static class EventSourceExtensions
    {
        public static IEventSource Where(this IEventSource eventSource, Func<IEvent, bool> predicate)
        {
            var observable = eventSource as IObservable<IEvent>;
            return new WrapperEventSource(observable.Where(predicate));
        }

        public static IAggregateEventSource Where(this IAggregateEventSource eventSource, Func<IAggregateEvent, bool> predicate)
        {
            var observable = eventSource as IObservable<IAggregateEvent>;
            return new WrapperAggregateEventSource(observable.Where(predicate));
        }

        public static IEventSource SkipAfter(this IEventSource eventSource, EventStreamIndex? eventStreamIndex)
        {
            var observable = eventSource as IObservable<IEvent>;
            if (eventStreamIndex.HasValue)
            {
                observable = observable.SkipWhile(x => x.StoreIndex >= eventStreamIndex.Value.Value);
            }
            return new WrapperEventSource(observable);
        }

        public static IAggregateEventSource SkipAfter(this IAggregateEventSource eventSource, EventStreamIndex? eventStreamIndex)
        {
            var observable = eventSource as IObservable<IAggregateEvent>;
            if (eventStreamIndex.HasValue)
            {
                observable = observable.SkipWhile(x => x.StoreIndex >= eventStreamIndex.Value.Value);
            }
            return new WrapperAggregateEventSource(observable);
        }

        public static IAggregateEventSource AsAggregateEventSource(this IEventSource eventSource)
        {
            var observable = eventSource as IObservable<IEvent>;
            return new WrapperAggregateEventSource(observable.Where(x => x is IAggregateEvent).Cast<IAggregateEvent>());
        }
    }
}