using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain.MetaModel;
using Eventualize.Interfaces.BaseTypes;

using FluentAssertions;

using Xunit;

namespace Eventualize.Test.Domain.MetaModel
{
    public class BoundedContextMetaModelTest
    {
        [Fact]
        public void BoundedContextMetaModelStoresAggregatesAndEventsByName()
        {
            var boundedContextName = new BoundedContextName("alsdkjsd");
            var eventTypeName1= new EventTypeName("sdklfdgjhsfkfdfsg");
            var eventTypeName2 = new EventTypeName("öäldfgpödortz");
            var aggregateTypeName1 = new AggregateTypeName("kldjoiödrfgh");
            var aggregateTypeName2 = new AggregateTypeName("äölfkdöoktz");

            var boundedContextMetaModel = new BoundedContextMetaModel(boundedContextName,
                new[]
                {
                    new AggregateMetaModel(boundedContextName, aggregateTypeName1, null, null),
                    new AggregateMetaModel(boundedContextName, aggregateTypeName2, null, null)
                },
                 new[]
                {
                    new EventMetaModel(boundedContextName, eventTypeName1, null), 
                    new EventMetaModel(boundedContextName, eventTypeName2, null)
                }
                );

            boundedContextMetaModel.BoundedContextName.Should().Be(boundedContextName);
            boundedContextMetaModel.AggregateTypes.Count().Should().Be(2);
            boundedContextMetaModel.EventTypes.Count().Should().Be(2);

            boundedContextMetaModel.GetAggregateType(aggregateTypeName1).TypeName.Should().Be(aggregateTypeName1);
            boundedContextMetaModel.GetAggregateType(aggregateTypeName2).TypeName.Should().Be(aggregateTypeName2);
            boundedContextMetaModel.GetAggregateType(new AggregateTypeName("sdlökjlödfghh")).Should().BeNull();

            boundedContextMetaModel.GetEventType(eventTypeName1).TypeName.Should().Be(eventTypeName1);
            boundedContextMetaModel.GetEventType(eventTypeName2).TypeName.Should().Be(eventTypeName2);
            boundedContextMetaModel.GetEventType(new EventTypeName("s.dkfjdflghöös")).Should().BeNull();
        }
    }
}
