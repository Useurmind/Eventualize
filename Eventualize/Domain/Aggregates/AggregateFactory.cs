using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Infrastructure;
using Eventualize.Interfaces;
using Eventualize.Interfaces.Aggregates;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Infrastructure;
using Eventualize.Interfaces.Snapshots;
using Eventualize.Persistence;
using Eventualize.Snapshots;

namespace Eventualize.Domain.Aggregates
{
    public class AggregateFactory : IAggregateFactory
    {
        private TypeRegister aggregateTypeRegister;

        public AggregateFactory(ISerializer serializer)
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

        public IAggregate BuildAggregate(AggregateIdentity aggregateIdentity, ISnapShot snapshot)
        {
            Type aggregateType = this.aggregateTypeRegister.GetType(aggregateIdentity.AggregateTypeName.Value, () => $"Could not find type for aggregate {aggregateIdentity.AggregateTypeName} with id {aggregateIdentity.Id}");
            IAggregate aggregate = null;
            if (snapshot != null)
            {
                aggregate = (IAggregate)Activator.CreateInstance(aggregateType);
                aggregate.ApplySnapshot(snapshot);
            }
            else
            {
                aggregate = (IAggregate)Activator.CreateInstance(aggregateType, aggregateIdentity.Id);
            }

            return aggregate;
        }

        public IAggregate BuildAggregate(AggregateIdentity aggregateIdentity, ISnapShot snapshot, IEnumerable<IEventData> events)
        {
            var aggregate = this.BuildAggregate(aggregateIdentity, snapshot);

            foreach (var eventData in events)
            {
                aggregate.ApplyEvent(eventData);
            }

            return aggregate;
        }
    }
}