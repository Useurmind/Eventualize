using System;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Persistence
{
    public interface IConstructAggregates
    {
        IAggregate Build(string aggregateTypeName, Guid id, IMemento snapshot);
    }
}