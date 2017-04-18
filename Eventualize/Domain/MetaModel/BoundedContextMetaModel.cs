using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain.MetaModel;

namespace Eventualize.Domain.MetaModel
{
    /// <summary>
    /// A meta model for a single bounded context with all aggregate and event types in it.
    /// </summary>
    public class BoundedContextMetaModel : BoundedContextRelatedMetaModel, IBoundedContextMetaModel
    {
        private IDictionary<AggregateTypeName, IAggregateMetaModel> aggregateTypesByName;
        private IDictionary<EventTypeName, IEventMetaModel> eventTypesByName;

        public BoundedContextMetaModel(BoundedContextName boundedContextName, IEnumerable<IAggregateMetaModel> aggregateTypes, IEnumerable<IEventMetaModel> eventTypes)
            : base(boundedContextName)
        {
            this.AggregateTypes = aggregateTypes;
            this.EventTypes = eventTypes;

            this.aggregateTypesByName = this.AggregateTypes.ToDictionary(x => x.TypeName, x => x);
            this.eventTypesByName = this.EventTypes.ToDictionary(x => x.TypeName, x => x);
        }

        /// <summary>
        /// The aggregate types in this bounded context.
        /// </summary>
        public IEnumerable<IAggregateMetaModel> AggregateTypes { get; }

        /// <summary>
        /// The event types in this bounded context.
        /// </summary>
        public IEnumerable<IEventMetaModel> EventTypes { get; }

        /// <summary>
        /// Get the aggregate type with the given name.
        /// </summary>
        /// <param name="name">The name of the aggregate type.</param>
        /// <returns>The meta model for the aggregate type or null.</returns>
        public IAggregateMetaModel GetAggregateType(AggregateTypeName name)
        {
            if (this.aggregateTypesByName.ContainsKey(name))
            {
                return this.aggregateTypesByName[name];
            }

            return null;
        }

        /// <summary>
        /// Get the event type with the given name.
        /// </summary>
        /// <param name="name">The name of the event type.</param>
        /// <returns>The meta model for the aggregateevent type or null.</returns>
        public IEventMetaModel GetEventType(EventTypeName name)
        {
            if (this.eventTypesByName.ContainsKey(name))
            {
                return this.eventTypesByName[name];
            }

            return null;
        }
    }
}