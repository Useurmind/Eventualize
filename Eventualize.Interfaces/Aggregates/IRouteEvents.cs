using System;
using System.Linq;

namespace Eventualize.Interfaces.Aggregates
{
    public interface IRouteEvents
    {
        void Register<T>(Action<T> handler);

        void Register(IAggregate aggregate);

        void Dispatch(object eventMessage);
    }
}