using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain.MetaModel;

namespace Eventualize.Domain.MetaModel
{
    /// <summary>
    /// A model for an aggregate type.
    /// </summary>
    public class AggregateMetaModel : BoundedContextRelatedMetaModel, IAggregateMetaModel
    {
        public AggregateMetaModel(BoundedContextName boundedContextName, AggregateTypeName typeName, Type modelType, Type snapshotType)
            : base(boundedContextName)
        {
            this.TypeName = typeName;
            this.ModelType = modelType;
            this.SnapshotType = snapshotType;
        }

        /// <summary>
        /// The name of the aggregate type.
        /// </summary>
        public AggregateTypeName TypeName { get; }

        /// <summary>
        /// The actual .NET type for this aggregate type.
        /// </summary>
        public Type ModelType { get; }

        /// <summary>
        /// The type of the snapshot for this aggregate type.
        /// </summary>
        public Type SnapshotType { get; }
    }
}
