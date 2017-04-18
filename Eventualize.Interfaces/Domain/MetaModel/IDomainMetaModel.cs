using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Domain.MetaModel
{
    public interface IDomainMetaModel
    {
        /// <summary>
        /// The bounded contexts contained in the domain.
        /// </summary>
        IEnumerable<IBoundedContextMetaModel> BoundedContexts { get; }

        /// <summary>
        /// Get the bounded context with the given name.
        /// </summary>
        /// <param name="name">The name of the bounded context.</param>
        /// <returns>The meta model for the bounded context or null.</returns>
        IBoundedContextMetaModel GetBoundedContext(BoundedContextName name);
    }
}