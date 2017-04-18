using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Persistence;

namespace Eventualize.Persistence
{
    public interface IPagedEventLoader
    {
        void LoadAllPages(IAggregateEventStore eventStore, AggregateIdentity aggregateIdentity, PageEventLoaderOptions options, Action<IAggregateEvent> eventAction);
    }
}