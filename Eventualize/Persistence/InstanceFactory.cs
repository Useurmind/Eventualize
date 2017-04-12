using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.Persistence
{
    public class InstanceFactory : IConstructInstances
    {
        private TypeRegister aggregateTypeRegister;

        public InstanceFactory(ISerializer serializer)
        {
            this.aggregateTypeRegister = new TypeRegister();
        }

        public void ScanAggregateTypes(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                this.ScanAggregateTypes(assembly);
            }
        }

        public void ScanAggregateTypes(Assembly assembly)
        {
            this.aggregateTypeRegister.ScanTypes(assembly, x => x.GetCustomAttribute<AggregateTypeNameAttribute>() != null, x => x.GetCustomAttribute<AggregateTypeNameAttribute>().Name);
        }

        public IAggregate BuildAggregate(AggregateIdentity aggregateIdentity, IMemento snapshot)
        {
            if (snapshot != null)
            {
                throw new NotImplementedException();
            }

            Type aggregateType = this.aggregateTypeRegister.GetType(aggregateIdentity.AggregateTypeName.Value, () => $"Could not find type for aggregate {aggregateIdentity.AggregateTypeName} with id {aggregateIdentity.Id}");
            IAggregate aggregate = (IAggregate)Activator.CreateInstance(aggregateType, aggregateIdentity.Id);

            return aggregate;
        }

        public IAggregate BuildAggregate(AggregateIdentity aggregateIdentity, IMemento snapshot, IEnumerable<IEventData> events)
        {
            var aggregate = BuildAggregate(aggregateIdentity, snapshot);

            foreach (var eventData in events)
            {
                aggregate.ApplyEvent(eventData);
            }

            return aggregate;
        }
    }
}