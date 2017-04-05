using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain;
using Eventualize.Materialization;

namespace Eventualize.Dapper.Materialization
{
    public class SimpleAggregateMaterializer<TAggregate, TReadModel> : AggregateMaterializerBase<TAggregate>
        where TAggregate : class, IAggregate
        where TReadModel: class, IReadModel
    {
        private Func<TAggregate, IAggregateEvent, TReadModel> mapToReadModel;

        private Func<IDbConnection> getConnection;

        public SimpleAggregateMaterializer(Func<IDbConnection> getConnection, Func<TAggregate, IAggregateEvent, TReadModel> mapToReadModel)
        {
            this.getConnection = getConnection;
            this.mapToReadModel = mapToReadModel;
        }

        protected override void HandleAggregateEvent(TAggregate aggregate, IAggregateEvent materializationEvent)
        {
            var readModel = this.mapToReadModel(aggregate, materializationEvent);
            readModel.LastEventDate = materializationEvent.CreationTime;
            readModel.LastEventStoreIndex = materializationEvent.StoreIndex;

            using (var connection = this.getConnection())
            {
                connection.Merge(readModel);
            }
        }
    }
}
