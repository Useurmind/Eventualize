using System;
using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Materialization.ReactiveStreams
{
    public class WrapperEventSource : IEventSource
    {
        private IObservable<IEvent> observable;

        public WrapperEventSource(IObservable<IEvent> observable)
        {
            this.observable = observable;
        }

        public IDisposable Subscribe(IObserver<IEvent> observer)
        {
            return this.observable.Subscribe(observer);
        }
    }
}