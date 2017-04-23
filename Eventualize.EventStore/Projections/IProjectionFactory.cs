using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain.MetaModel;

namespace Eventualize.EventStore.Test.Projections
{
    /// <summary>
    /// Interface for a factory that creates the necessary projections for the materialization process.
    /// </summary>
    public interface IProjectionFactory
    {
        /// <summary>
        /// Ensures that the projection merging all bounded contexts as well as the projections for each single bounded context are created.
        /// </summary>
        /// <param name="domainMetaModel">The domain model with the bounded contexts.</param>
        void EnsureProjectionFor(IDomainMetaModel domainMetaModel);

        /// <summary>
        /// Ensures that there is a projection that merges events from all bounded contexts.
        /// </summary>
        void EnsureProjectionForAllBoundedContexts();

        /// <summary>
        /// Ensures that there is a projection that merges all events from the given bounded context.
        /// </summary>
        /// <param name="boundedContextName">The name of the bounded context.</param>
        void EnsureProjectionFor(BoundedContextName boundedContextName);

        /// <summary>
        /// Ensures that there is a projection that merges all events from the given aggregate type in the given bounded context.
        /// </summary>
        /// <param name="boundedContextName">The name of the bounded context.</param>
        /// <param name="aggregateTypeName">The name of the aggregate type.</param>
        void EnsureProjectionFor(BoundedContextName boundedContextName, AggregateTypeName aggregateTypeName);
    }
}