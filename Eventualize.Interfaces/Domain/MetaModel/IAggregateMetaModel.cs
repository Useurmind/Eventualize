using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Domain.MetaModel
{
    public interface IAggregateMetaModel
    {
        /// <summary>
        /// The name of the aggregate type.
        /// </summary>
        AggregateTypeName TypeName { get; }

        /// <summary>
        /// The actual .NET type for this aggregate type.
        /// </summary>
        Type ModelType { get; }

        /// <summary>
        /// The type of the snapshot for this aggregate type.
        /// </summary>
        Type SnapshotType { get; }

        /// <summary>
        /// The name of the bounded context to which this instance belongs.
        /// </summary>
        BoundedContextName BoundedContextName { get; }
    }
}