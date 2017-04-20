using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Materialization.AggregateMaterialization;

namespace Eventualize.Dapper.Materialization
{
    public class M2NMaterializer<TAggregate, TReadModel> : SingleAggregateMaterializerBase<TAggregate>
        where TAggregate : class, IAggregate
        where TReadModel: class, IReadModel, new()
    {
        private Func<TAggregate, IAggregateEvent, TReadModel> mapToReadModel;

        private Func<IDbConnection> getConnection;

        public M2NMaterializer(Func<IDbConnection> getConnection)
        {
            this.getConnection = getConnection;
            this.mapToReadModel = null;
        }

        public M2NMaterializer(Func<IDbConnection> getConnection, Func<TAggregate, IAggregateEvent, TReadModel> mapToReadModel)
        {
            this.getConnection = getConnection;
            this.mapToReadModel = mapToReadModel;
        }

        protected override void HandleAggregateEvent(TAggregate aggregate, IAggregateEvent materializationEvent)
        {
            var readModel = this.MapToReadModel(aggregate, materializationEvent);
            readModel.Version = aggregate.Version.Value;
            readModel.LastEventDate = materializationEvent.CreationTime;
            readModel.LastEventNumber = materializationEvent.EventStreamIndex.Value;
            readModel.LastModifierId = materializationEvent.CreatorId.Value;

            using (var connection = this.getConnection())
            {
                connection.Merge(readModel);
            }
        }

        private TReadModel MapToReadModel(TAggregate aggregate, IAggregateEvent materializationEvent)
        {
            TReadModel readModel = null;
            if (this.mapToReadModel != null)
            {
                readModel = this.mapToReadModel(aggregate, materializationEvent);
            }
            else
            {
                readModel = this.AutoMapToReadModel(aggregate, materializationEvent);
            }
            return readModel;
        }

        private TReadModel AutoMapToReadModel(TAggregate aggregate, IAggregateEvent materializationEvent)
        {
            TReadModel readModel = new TReadModel();

            var baseReadModelProperties = typeof(IReadModel).GetProperties();
            var actualReadModelProperties = typeof(TReadModel).GetProperties();
            var aggregateProperties = typeof(TAggregate).GetProperties();

            // only map properties not contained in the IReadModel interface
            var propertiesToMap = actualReadModelProperties.Where(x => !baseReadModelProperties.Select(y => y.Name).Contains(x.Name));

            foreach (var property in propertiesToMap)
            {
                var aggregateProperty = aggregateProperties.FirstOrDefault(x => x.Name == property.Name);
                if (aggregateProperty == null)
                {
                    throw new Exception($"Could not find property '{property.Name}' in aggregate of type '{typeof(TAggregate).FullName}' for mapping into read model of type '{typeof(TReadModel).FullName}'");
                }

                var aggregateValue = aggregateProperty.GetValue(aggregate);
                property.SetValue(readModel, aggregateValue);
            }

            return readModel;
        }
    }
}
