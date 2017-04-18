using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain.MetaModel;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Test.TestDomain;
using Eventualize.Test.TestDomain.FirstContext.MyFirstAggregates;
using Eventualize.Test.TestDomain.FirstContext.MySecondAggregates;
using Eventualize.Test.TestDomain.SecondContext.MyThirdAggregate;

using FluentAssertions;

using Xunit;

namespace Eventualize.Test.Domain.MetaModel
{
    public class ReflectionBasedMetaModelFactoryTest
    {
        [Fact]
        public void TestDomainIsCorrectlyExtracted()
        {
            var factory = new ReflectionBasedMetaModelFactory(new [] { Assembly.GetExecutingAssembly()});

            var domainModel = factory.Build();
            var firstContext = domainModel.GetBoundedContext(new BoundedContextName(DomainNames.FirstContextName));
            var secondContext = domainModel.GetBoundedContext(new BoundedContextName(DomainNames.SecondContextName));


            domainModel.BoundedContexts.Count().Should().BeGreaterOrEqualTo(2);

            firstContext.Should().NotBeNull();
            firstContext.AggregateTypes.Count().Should().Be(2);
            firstContext.EventTypes.Count().Should().Be(2);
            firstContext.BoundedContextName.Value.Should().Be(DomainNames.FirstContextName);

            secondContext.Should().NotBeNull();
            secondContext.AggregateTypes.Count().Should().Be(1);
            secondContext.EventTypes.Count().Should().Be(1);
            secondContext.BoundedContextName.Value.Should().Be(DomainNames.SecondContextName);

            var firstAggregate = firstContext.GetAggregateType(new AggregateTypeName("MyFirstAggregateWoot"));
            firstAggregate.Should().NotBeNull();
            firstAggregate.BoundedContextName.Value.Should().Be(DomainNames.FirstContextName);
            firstAggregate.TypeName.Value.Should().Be("MyFirstAggregateWoot");
            firstAggregate.ModelType.Should().Be(typeof(MyFirstAggregate));

            var firstEvent = firstContext.GetEventType(new EventTypeName("MyFirstEventWoot"));
            firstEvent.Should().NotBeNull();
            firstEvent.BoundedContextName.Value.Should().Be(DomainNames.FirstContextName);
            firstEvent.TypeName.Value.Should().Be("MyFirstEventWoot");
            firstEvent.ModelType.Should().Be(typeof(MyFirstEvent));

            var secondAggregate = firstContext.GetAggregateType(new AggregateTypeName("MySecondAggregate"));
            secondAggregate.Should().NotBeNull();
            secondAggregate.BoundedContextName.Value.Should().Be(DomainNames.FirstContextName);
            secondAggregate.TypeName.Value.Should().Be("MySecondAggregate");
            secondAggregate.ModelType.Should().Be(typeof(MySecondAggregate));

            var secondEvent = firstContext.GetEventType(new EventTypeName("MySecondEvent"));
            secondEvent.Should().NotBeNull();
            secondEvent.BoundedContextName.Value.Should().Be(DomainNames.FirstContextName);
            secondEvent.TypeName.Value.Should().Be("MySecondEvent");
            secondEvent.ModelType.Should().Be(typeof(MySecondEvent));

            var thirdAggregate = secondContext.GetAggregateType(new AggregateTypeName("MyThirdAggregate"));
            thirdAggregate.Should().NotBeNull();
            thirdAggregate.BoundedContextName.Value.Should().Be(DomainNames.SecondContextName);
            thirdAggregate.TypeName.Value.Should().Be("MyThirdAggregate");
            thirdAggregate.ModelType.Should().Be(typeof(MyThirdAggregate));

            var thirdEvent = secondContext.GetEventType(new EventTypeName("MyThirdEvent"));
            thirdEvent.Should().NotBeNull();
            thirdEvent.BoundedContextName.Value.Should().Be(DomainNames.SecondContextName);
            thirdEvent.TypeName.Value.Should().Be("MyThirdEvent");
            thirdEvent.ModelType.Should().Be(typeof(MyThirdEvent));

        }
    }
}
