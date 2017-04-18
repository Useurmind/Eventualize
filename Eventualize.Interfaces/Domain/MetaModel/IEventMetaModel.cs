using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Domain.MetaModel
{
    public interface IEventMetaModel
    {
        /// <summary>
        /// The name of the event type.
        /// </summary>
        EventTypeName TypeName { get; }

        /// <summary>
        /// The actual .NET type for this event type.
        /// </summary>
        Type ModelType { get; }

        /// <summary>
        /// The name of the bounded context to which this instance belongs.
        /// </summary>
        BoundedContextName BoundedContextName { get; }
    }
}