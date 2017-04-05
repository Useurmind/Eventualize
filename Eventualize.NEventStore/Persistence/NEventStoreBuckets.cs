using System.Linq;

using CommonDomain;
using CommonDomain.Persistence;

using IAggregate = Eventualize.Domain.IAggregate;
using IConstructInstances = Eventualize.Persistence.IConstructInstances;
using IMemento = Eventualize.Domain.IMemento;

namespace Eventualize.NEventStore.Persistence
{
    public class NEventStoreBuckets
    {
        public const string Aggregates = "Aggregates";
    }
}
