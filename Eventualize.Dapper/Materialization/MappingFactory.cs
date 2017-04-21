using System.Linq;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;

namespace Eventualize.Dapper.Materialization
{
    public class MappingFactory
    {
        public IReadModelMapping<TAggregate, TReadModel> DefaultAutoMap<TAggregate, TReadModel>()
            where TAggregate : class, IAggregate
            where TReadModel : class, IReadModel, new()
        {
            return new DefaultAutoMap<TAggregate, TReadModel>();
        }

        //public Func<TAggregate, IAggregateEvent, TReadModel> M2NTableMap<TAggregate, TReadModel>(Expression<Func<TAggregate, IEnumerable<AggregateIdentity>> )
        //    where TAggregate : class, IAggregate
        //    where TReadModel : class, IReadModel, new()
        //{

        //}
    }
}