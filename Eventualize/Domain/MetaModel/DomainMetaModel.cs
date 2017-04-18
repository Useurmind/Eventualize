using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain.MetaModel;

namespace Eventualize.Domain.MetaModel
{
    /// <summary>
    /// Meta model of the complete domain with all bounded contexts in it.
    /// </summary>
    public class DomainMetaModel : IDomainMetaModel
    {
        private IDictionary<BoundedContextName, IBoundedContextMetaModel> boundedContextsByName;

        public DomainMetaModel(IEnumerable<IBoundedContextMetaModel> boundedContexts)
        {
            this.BoundedContexts = boundedContexts;
            this.boundedContextsByName = boundedContexts.ToDictionary(x => x.BoundedContextName, x => x);
        }

        /// <summary>
        /// The bounded contexts contained in the domain.
        /// </summary>
        public IEnumerable<IBoundedContextMetaModel> BoundedContexts { get; }

        /// <summary>
        /// Get the bounded context with the given name.
        /// </summary>
        /// <param name="name">The name of the bounded context.</param>
        /// <returns>The meta model for the bounded context or null.</returns>
        public IBoundedContextMetaModel GetBoundedContext(BoundedContextName name)
        {
            if (this.boundedContextsByName.ContainsKey(name))
            {
                return this.boundedContextsByName[name];
            }

            return null;
        }
    }
}