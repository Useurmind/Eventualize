using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Domain.MetaModel;

namespace Eventualize.Domain.MetaModel
{
    /// <summary>
    /// This class implements then <see cref="IDomainIdentityProvider"/> for a given <see cref="IDomainMetaModel"/>.
    /// </summary>
    public class DomainModelIdentityProvider : IDomainIdentityProvider
    {
        private IDomainMetaModel metaModel;
        private IDictionary<Type, IAggregateMetaModel> aggregateTypesByType;
        private IDictionary<Type, IEventMetaModel> eventTypesByType;

        public DomainModelIdentityProvider(IDomainMetaModel metaModel)
        {
            this.metaModel = metaModel;

            this.aggregateTypesByType = metaModel.BoundedContexts.SelectMany(x => x.AggregateTypes).ToDictionary(x => x.ModelType, x => x);
            this.eventTypesByType = metaModel.BoundedContexts.SelectMany(x => x.EventTypes).ToDictionary(x => x.ModelType, x => x);
        }

        public BoundedContextName GetAggregateBoundedContext(Type aggregateType)
        {
            IAggregateMetaModel aggregateMetaModel;
            if(!this.aggregateTypesByType.TryGetValue(aggregateType, out aggregateMetaModel))
            {
                throw new Exception($"The class {aggregateType.FullName} is not registered in the domain meta model as an aggregate.");
            }

            return aggregateMetaModel.BoundedContextName;
        }

        public AggregateTypeName GetAggregtateTypeName(Type aggregateType)
        {
            IAggregateMetaModel aggregateMetaModel;
            if (!this.aggregateTypesByType.TryGetValue(aggregateType, out aggregateMetaModel))
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
            IEventMetaModel eventMetaModel;
            if (!this.eventTypesByType.TryGetValue(eventType, out eventMetaModel))
            {
                throw new Exception($"The class {eventType.FullName} was not registered in the domain meta model as an event.");
            }

            return eventMetaModel.TypeName;
        }

        /// <summary>
        /// Get the aggregate type with the given name.
        /// </summary>
        /// <param name="aggregateType">The aggregate type.</param>
        /// <returns>The meta model for the aggregate type or null.</returns>
        public IAggregateMetaModel GetAggregateType(Type aggregateType)
        {
            if (this.aggregateTypesByType.ContainsKey(aggregateType))
            {
                return this.aggregateTypesByType[aggregateType];
            }

            return null;
        }

        /// <summary>
        /// Get the event type with the given name.
        /// </summary>
        /// <param name="eventType">The event type.</param>
        /// <returns>The meta model for the event type or null.</returns>
        public IEventMetaModel GetEventType(Type eventType)
        {
            if (this.eventTypesByType.ContainsKey(eventType))
            {
                return this.eventTypesByType[eventType];
            }

            return null;
        }
    }
}
