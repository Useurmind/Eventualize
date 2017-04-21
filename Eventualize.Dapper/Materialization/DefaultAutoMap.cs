using System;
using System.Linq;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;

namespace Eventualize.Dapper.Materialization
{
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
}