using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Domain;
using Eventualize.Domain.MetaModel;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Domain.MetaModel;
using Eventualize.Persistence;
using Eventualize.Security;

namespace Eventualize.Test.Persistence
{
    public class TestContainer
    {

        public IEnumerable<IEventData> CreateEvents<TEvent>(int number = 1)
            where TEvent : IEventData, new()
        {
            return Enumerable.Range(0, number).Select(x => new TEvent()).Cast<IEventData>().ToArray();
        }

        public AggregateIdentity CreateAggregateIdentity<TAggregate>()
            where TAggregate : IAggregate, new()
        {
            return this.GetDomainIdentityProvider().GetAggregateIdentity(new TAggregate());
        }

        public InMemoryAggregateEventStore CreateStore()
        {
            return new InMemoryAggregateEventStore(this.GetDomainIdentityProvider());
        }

        public DomainModelIdentityProvider GetDomainIdentityProvider()
        {
            return new DomainModelIdentityProvider(this.GetDomainModel());
        }

        public IDomainMetaModel GetDomainModel()
        {
            EventualizeContext.Init(new UserId("MyUser"));
            var domainModel = new ReflectionBasedMetaModelFactory(new[] { Assembly.GetExecutingAssembly() }).Build();
            return domainModel;
        }
    }
}