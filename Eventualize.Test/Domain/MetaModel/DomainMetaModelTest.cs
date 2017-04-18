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
    public class DomainMetaModelTest
    {
        [Fact]
        public void DomainMetaModelStoresBoundedContextsByName()
        {
            var boundedContextName1 = new BoundedContextName("adkljdföggsdf ");
            var boundedContextName2 = new BoundedContextName("-lkfglöjhopiut");

            var domainMetaModel = new DomainMetaModel(
                new[]
                {
                    new BoundedContextMetaModel(boundedContextName1, Enumerable.Empty<AggregateMetaModel>(), Enumerable.Empty<EventMetaModel>()),
                    new BoundedContextMetaModel(boundedContextName2, Enumerable.Empty<AggregateMetaModel>(), Enumerable.Empty<EventMetaModel>())
                }
                );

            domainMetaModel.BoundedContexts.Count().Should().Be(2);
            domainMetaModel.GetBoundedContext(boundedContextName1).BoundedContextName.Should().Be(boundedContextName1);
            domainMetaModel.GetBoundedContext(boundedContextName2).BoundedContextName.Should().Be(boundedContextName2);
            domainMetaModel.GetBoundedContext(new BoundedContextName("lkjfgdlkjd")).Should().BeNull();
        }
    }
}
