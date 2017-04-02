using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.Persistence
{
    public class AggregateFactory : IConstructAggregates
    {
        private Dictionary<string, Type> aggregateTypesByName;

        public AggregateFactory()
        {
            this.aggregateTypesByName = new Dictionary<string, Type>();
        }

        public void ScanAggregateTypes(Assembly assembly)
        {
            var aggregateTypes = assembly.GetExportedTypes()
                                         .Where(x => x.GetCustomAttribute<AggregateTypeNameAttribute>() != null)
                                         .Select(x => new
                                                          {
                                                              AggregateType = x,
                                                              AggregateTypeName = x.GetCustomAttribute<AggregateTypeNameAttribute>().Name
                                                          });

            foreach (var agg in aggregateTypes)
            {
                this.aggregateTypesByName.Add(agg.AggregateTypeName, agg.AggregateType);
            }
        }

        public IAggregate Build(string aggregateTypeName, Guid id, IMemento snapshot)
        {
            if (snapshot != null)
            {
                throw new NotImplementedException();
            }

            Type aggregateType = null;
            if (!this.aggregateTypesByName.TryGetValue(aggregateTypeName, out aggregateType))
            {
                throw new Exception($"Could not find type for aggregate {aggregateTypeName} with id {id}");
            }

            IAggregate aggregate = (IAggregate)Activator.CreateInstance(aggregateType, id);

            return aggregate;
        }
    }
}