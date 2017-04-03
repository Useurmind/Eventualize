using System;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Persistence
{
    public interface IConstructInstances
    {
        IAggregate BuildAggregate(string aggregateTypeName, Guid id, IMemento snapshot);
    }
}