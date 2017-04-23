using System;
using System.Linq;
using System.Reactive.Linq;

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

        public static IAggregateEventSource AsAggregateEventSource(this IEventSource eventSource)
        {
            var observable = eventSource as IObservable<IEvent>;
            return new WrapperAggregateEventSource(observable.Where(x => x is IAggregateEvent).Cast<IAggregateEvent>());
        }
    }
}