using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Materialization.AggregateMaterialization;

namespace Eventualize.Dapper.Materialization
{
    public interface IReadModelMapping<TAggregate, TReadModel>
            where TAggregate : class, IAggregate
            where TReadModel : class, IReadModel
    {
        TReadModel MapFromAggregate(TAggregate aggregate);
    }

    public class DefaultAutoMap<TAggregate, TReadModel> : IReadModelMapping<TAggregate, TReadModel>
        where TAggregate : class, IAggregate
        where TReadModel : class, IReadModel, new()
    {
        public TReadModel MapFromAggregate(TAggregate aggregate)
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

    public class DapperSqlTableMaterializer<TAggregate, TReadModel> : SingleAggregateMaterializerBase<TAggregate>
        where TAggregate : class, IAggregate
        where TReadModel : class, IReadModel, new()
    {
        private Func<TAggregate, IAggregateEvent, TReadModel> mapToReadModel;

        private Func<IDbConnection> getConnection;

        public DapperSqlTableMaterializer(Func<IDbConnection> getConnection)
        {
            this.getConnection = getConnection;
            this.mapToReadModel = null;
        }

        public DapperSqlTableMaterializer(Func<IDbConnection> getConnection, Func<TAggregate, IAggregateEvent, TReadModel> mapToReadModel)
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
