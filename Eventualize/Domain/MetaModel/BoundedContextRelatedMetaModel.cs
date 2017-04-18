using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Domain.MetaModel
{
    /// <summary>
    /// Base class for all meta models that are connected to a bounded context.
    /// </summary>
    public class BoundedContextRelatedMetaModel
    {
        public BoundedContextRelatedMetaModel(BoundedContextName boundedContextName)
        {
            this.BoundedContextName = boundedContextName;
        }

        /// <summary>
        /// The name of the bounded context to which this instance belongs.
        /// </summary>
        public BoundedContextName BoundedContextName { get; }
    }
}