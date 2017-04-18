using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Domain.MetaModel
{
    /// <summary>
    /// Meta model of the complete domain with all bounded contexts in it.
    /// </summary>
    public class DomainMetaModel
    {
        private IDictionary<BoundedContextName, BoundedContextMetaModel> boundedContextsByName;

        public DomainMetaModel(IEnumerable<BoundedContextMetaModel> boundedContexts)
        {
            this.BoundedContexts = boundedContexts;
            this.boundedContextsByName = boundedContexts.ToDictionary(x => x.BoundedContextName, x => x);
        }

        /// <summary>
        /// The bounded contexts contained in the domain.
        /// </summary>
        public IEnumerable<BoundedContextMetaModel> BoundedContexts { get; }

        /// <summary>
        /// Get the bounded context with the given name.
        /// </summary>
        /// <param name="name">The name of the bounded context.</param>
        /// <returns>The meta model for the bounded context or null.</returns>
        public BoundedContextMetaModel GetBoundedContext(BoundedContextName name)
        {
            if (this.boundedContextsByName.ContainsKey(name))
            {
                return this.boundedContextsByName[name];
            }

            return null;
        }
    }
}