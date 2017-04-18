using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Infrastructure;
using Eventualize.Interfaces;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Domain.MetaModel;
using Eventualize.Interfaces.Infrastructure;
using Eventualize.Interfaces.Snapshots;
using Eventualize.Persistence;
using Eventualize.Snapshots;

namespace Eventualize.Domain.Aggregates
{
    public class AggregateFactory : IAggregateFactory
    {
        private IDomainMetaModel metaModel;

        public AggregateFactory(IDomainMetaModel metaModel)
        {
            this.metaModel = metaModel;
        }

        public IAggregate BuildAggregate(AggregateIdentity aggregateIdentity, ISnapShot snapshot)
        {
            var aggregateMetaModel = metaModel.GetBoundedContext(aggregateIdentity.BoundedContextName).GetAggregateType(aggregateIdentity.AggregateTypeName);

            Type aggregateType = aggregateMetaModel.ModelType;
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