using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain.Aggregates;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Domain.MetaModel;

namespace Eventualize.Domain
{
    public class DomainModelIdentityProvider : IDomainIdentityProvider
    {
        private IDomainMetaModel metaModel;

        public DomainModelIdentityProvider(IDomainMetaModel metaModel)
        {
            this.metaModel = metaModel;
        }

        public BoundedContextName GetAggregateBoundedContext(Type aggregateType)
        {
            var boundedContextMetaModel = this.metaModel.BoundedContexts.FirstOrDefault(bc => bc.AggregateTypes.Any(x => x.ModelType == aggregateType));
            if (boundedContextMetaModel == null)
            {
                throw new Exception($"The class {aggregateType.FullName} is not registered in the domain meta model as an aggregate.");
            }

            return boundedContextMetaModel.BoundedContextName;
        }

        public AggregateTypeName GetAggregtateTypeName(Type aggregateType)
        {
            var aggregateMetaModel = this.metaModel.BoundedContexts.SelectMany(x => x.AggregateTypes).FirstOrDefault(a => a.ModelType == aggregateType);
            if (aggregateMetaModel == null)
            {
                throw new Exception($"The class {aggregateType.FullName} was not registered in the domain meta model as an aggregate.");
            }

            return aggregateMetaModel.TypeName;
        }

        public EventTypeName GetEventTypeName(IEventData eventData)
        {
            return this.GetEventTypeName(eventData.GetType());
        }

        public EventTypeName GetEventTypeName(Type eventType)
        {
            var eventMetaModel = this.metaModel.BoundedContexts.SelectMany(x => x.EventTypes).FirstOrDefault(a => a.ModelType == eventType);
            if (eventMetaModel == null)
            {
                throw new Exception($"The class {eventType.FullName} was not registered in the domain meta model as an event.");
            }

            return eventMetaModel.TypeName;
        }
    }
}
