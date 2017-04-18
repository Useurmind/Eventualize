using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Domain.MetaModel
{
    /// <summary>
    /// Metadata about an event type in the domain.
    /// </summary>
    public class EventMetaModel : BoundedContextRelatedMetaModel
    {
        public EventMetaModel(BoundedContextName boundedContextName, EventTypeName typeName, Type modelType)
            : base(boundedContextName)
        {
            this.TypeName = typeName;
            this.ModelType = modelType;
        }

        /// <summary>
        /// The name of the event type.
        /// </summary>
        public EventTypeName TypeName { get; }

        /// <summary>
        /// The actual .NET type for this event type.
        /// </summary>
        public Type ModelType { get; }
    }
}