using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Aggregates
{
    public static class DomainIdentityProviderExtensions
    {
        public static AggregateIdentity GetAggregateIdentity<TAggregate>(
            this IDomainIdentityProvider identityProvider,
            Guid id) where TAggregate : IAggregate
        {
            return new AggregateIdentity(
                identityProvider.GetAggregateBoundedContext(typeof(TAggregate)),
                identityProvider.GetAggregtateTypeName(typeof(TAggregate)),
                id);
        }

        public static AggregateIdentity GetAggregateIdentity(this IDomainIdentityProvider identityProvider, IAggregate aggregate)
        {
            return new AggregateIdentity(identityProvider.GetAggregateBoundedContext(aggregate), identityProvider.GetAggregtateTypeName(aggregate), aggregate.Id);
        }

        public static AggregateTypeName GetAggregtateTypeName(this IDomainIdentityProvider identityProvider, IAggregate aggregate)
        {
            return identityProvider.GetAggregtateTypeName(aggregate.GetType());
        }

        public static BoundedContextName GetAggregateBoundedContext(this IDomainIdentityProvider identityProvider, IAggregate aggregate)
        {
            return identityProvider.GetAggregateBoundedContext(aggregate.GetType());
        }

    }
}