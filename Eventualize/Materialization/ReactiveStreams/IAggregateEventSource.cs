using System;
using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Materialization.ReactiveStreams
{
    public interface IAggregateEventSource : IObservable<IAggregateEvent>
    {
    }
}