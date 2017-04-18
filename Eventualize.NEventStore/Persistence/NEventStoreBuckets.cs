using System.Linq;

using CommonDomain;
using CommonDomain.Persistence;

using IAggregate = Eventualize.Interfaces.Domain.IAggregate;

namespace Eventualize.NEventStore.Persistence
{
    public class NEventStoreBuckets
    {
        public const string Aggregates = "Aggregates";
    }
}
