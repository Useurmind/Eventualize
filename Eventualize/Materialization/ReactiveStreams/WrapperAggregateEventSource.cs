using System;
using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Materialization.ReactiveStreams
{
    public class WrapperAggregateEventSource : IAggregateEventSource
    {
        private IObservable<IAggregateEvent> observable;

        public WrapperAggregateEventSource(IObservable<IAggregateEvent> observable)
        {
            this.observable = observable;
        }

        public IDisposable Subscribe(IObserver<IAggregateEvent> observer)
        {
            return this.observable.Subscribe(observer);
        }
    }
}