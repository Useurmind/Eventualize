using System;
using System.Linq;
using System.Threading.Tasks;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Materialization.ReactiveStreams
{
    public interface IEventSink : IObserver<IEvent>
    {
        Task<long> GetProgressAsync();
    }
}