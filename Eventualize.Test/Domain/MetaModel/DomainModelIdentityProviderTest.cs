using System.Linq;

using Eventualize.Domain.MetaModel;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Test.TestDomain.FirstContext.MyFirstAggregates;
using Eventualize.Test.TestDomain.FirstContext.MySecondAggregates;
using Eventualize.Test.TestDomain.SecondContext.MyThirdAggregate;

using FluentAssertions;

using Xunit;

namespace Eventualize.Test.Domain.MetaModel
{
    public class DomainModelIdentityProviderTest
    {
        [Fact]
        public void CorrectlyCombinesAggregatesFromBoundedContexts()
        {
            var boundedContextName1 = new BoundedContextName("adkljdföggsdf ");
            var aggregateTypeName1 = new AggregateTypeName("asklgöhdg");
            var aggregateTypeName2 = new AggregateTypeName("öälfgkdh");
            var boundedContextName2 = new BoundedContextName("-lkfglöjhopiut");
            var aggregateTypeName3 = new AggregateTypeName("äölkgöäh");

            var domainMetaModel = new DomainMetaModel(
                new[]
                {
                    new BoundedContextMetaModel(boundedContextName1, new []
                                                                     {
                                                                         new AggregateMetaModel(boundedContextName1,
                                                                             aggregateTypeName1,
                                                                             typeof(MyFirstAggregate),
                                                                             null),
                                                                         new AggregateMetaModel(boundedContextName1,
                                                                             aggregateTypeName2,
                                                                             typeof(MySecondAggregate),
                                                                             null)
                                                                     }, Enumerable.Empty<EventMetaModel>()),
                    new BoundedContextMetaModel(boundedContextName2,  new []
                                                                      {
                                                                          new AggregateMetaModel(boundedContextName2,
                                                                              aggregateTypeName3,
                                                                              typeof(MyThirdAggregate),
                                                                              null),
                                                                      }, Enumerable.Empty<EventMetaModel>())
                }
            );

            var identityProvider = new DomainModelIdentityProvider(domainMetaModel);

            identityProvider.GetAggregateType(typeof(MyFirstAggregate)).BoundedContextName.Should().Be(boundedContextName1);
            identityProvider.GetAggregateType(typeof(MyFirstAggregate)).ModelType.Should().Be(typeof(MyFirstAggregate));
            identityProvider.GetAggregateType(typeof(MyFirstAggregate)).TypeName.Should().Be(aggregateTypeName1);

            identityProvider.GetAggregateType(typeof(MySecondAggregate)).BoundedContextName.Should().Be(boundedContextName1);
            identityProvider.GetAggregateType(typeof(MySecondAggregate)).ModelType.Should().Be(typeof(MySecondAggregate));
            identityProvider.GetAggregateType(typeof(MySecondAggregate)).TypeName.Should().Be(aggregateTypeName2);

            identityProvider.GetAggregateType(typeof(MyThirdAggregate)).BoundedContextName.Should().Be(boundedContextName2);
            identityProvider.GetAggregateType(typeof(MyThirdAggregate)).ModelType.Should().Be(typeof(MyThirdAggregate));
            identityProvider.GetAggregateType(typeof(MyThirdAggregate)).TypeName.Should().Be(aggregateTypeName3);
            
            identityProvider.GetAggregateBoundedContext(typeof(MyFirstAggregate)).Should().Be(boundedContextName1);
            identityProvider.GetAggregateBoundedContext(typeof(MySecondAggregate)).Should().Be(boundedContextName1);
            identityProvider.GetAggregateBoundedContext(typeof(MyThirdAggregate)).Should().Be(boundedContextName2);

            identityProvider.GetAggregtateTypeName(typeof(MyFirstAggregate)).Should().Be(aggregateTypeName1);
            identityProvider.GetAggregtateTypeName(typeof(MySecondAggregate)).Should().Be(aggregateTypeName2);
            identityProvider.GetAggregtateTypeName(typeof(MyThirdAggregate)).Should().Be(aggregateTypeName3);
        }

        [Fact]
        public void CorrectlyCombinesEventsFromBoundedContexts()
        {
            var boundedContextName1 = new BoundedContextName("adkljdföggsdf ");
            var eventTypeName1 = new EventTypeName("asklgöhdg");
            var eventTypeName2 = new EventTypeName("öälfgkdh");
            var boundedContextName2 = new BoundedContextName("-lkfglöjhopiut");
            var eventTypeName3 = new EventTypeName("äölkgöäh");

            var domainMetaModel = new DomainMetaModel(
                new[]
                {
                    new BoundedContextMetaModel(boundedContextName1, Enumerable.Empty<AggregateMetaModel>(), new []
                                                                                                             {
                                                                                                                 new EventMetaModel(boundedContextName1,
                                                                                                                     eventTypeName1,
                                                                                                                     typeof(MyFirstEvent)),
                                                                                                                 new EventMetaModel(boundedContextName1,
                                                                                                                     eventTypeName2,
                                                                                                                     typeof(MySecondEvent))
                                                                                                             }),
                    new BoundedContextMetaModel(boundedContextName2,  Enumerable.Empty<AggregateMetaModel>(), new []
                                                                                                              {
                                                                                                                  new EventMetaModel(boundedContextName2,
                                                                                                                      eventTypeName3,
                                                                                                                      typeof(MyThirdEvent)),
                                                                                                              })
                }
            );

            var identityProvider = new DomainModelIdentityProvider(domainMetaModel);

            identityProvider.GetEventType(typeof(MyFirstEvent)).BoundedContextName.Should().Be(boundedContextName1);
            identityProvider.GetEventType(typeof(MyFirstEvent)).ModelType.Should().Be(typeof(MyFirstEvent));
            identityProvider.GetEventType(typeof(MyFirstEvent)).TypeName.Should().Be(eventTypeName1);

            identityProvider.GetEventType(typeof(MySecondEvent)).BoundedContextName.Should().Be(boundedContextName1);
            identityProvider.GetEventType(typeof(MySecondEvent)).ModelType.Should().Be(typeof(MySecondEvent));
            identityProvider.GetEventType(typeof(MySecondEvent)).TypeName.Should().Be(eventTypeName2);

            identityProvider.GetEventType(typeof(MyThirdEvent)).BoundedContextName.Should().Be(boundedContextName2);
            identityProvider.GetEventType(typeof(MyThirdEvent)).ModelType.Should().Be(typeof(MyThirdEvent));
            identityProvider.GetEventType(typeof(MyThirdEvent)).TypeName.Should().Be(eventTypeName3);

            identityProvider.GetEventTypeName(new MyFirstEvent()).Should().Be(eventTypeName1);
            identityProvider.GetEventTypeName(new MySecondEvent()).Should().Be(eventTypeName2);
            identityProvider.GetEventTypeName(new MyThirdEvent()).Should().Be(eventTypeName3);

            identityProvider.GetEventTypeName(typeof(MyFirstEvent)).Should().Be(eventTypeName1);
            identityProvider.GetEventTypeName(typeof(MySecondEvent)).Should().Be(eventTypeName2);
            identityProvider.GetEventTypeName(typeof(MyThirdEvent)).Should().Be(eventTypeName3);
        }
    }
}