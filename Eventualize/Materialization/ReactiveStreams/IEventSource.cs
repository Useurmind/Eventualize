using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Materialization.ReactiveStreams
{
    public interface IEventSource : IObservable<IEvent>
    {
    }
}
