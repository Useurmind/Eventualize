using System.Linq;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;

namespace Eventualize.Dapper.Materialization
{
    public interface IReadModelMapping<TAggregate, TReadModel>
        where TAggregate : class, IAggregate
        where TReadModel : class, IReadModel
    {
        TReadModel MapFromAggregate(TAggregate aggregate);
    }
}