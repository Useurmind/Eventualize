using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Dapper.Materialization;
using Eventualize.Dapper.Proxies;
using Eventualize.Domain.Events;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;

using FluentAssertions;

using Xunit;

namespace Eventualize.Dapper.Test.Materialization
{
    public class ProjectionPropertyModificationInterceptorTest
    {
        public class EventData : IEventData
        {
            
        }

        public interface ITestProjectionModel : IProjectionModel
        {
            Guid Id { get; set; }

            int SomeInt { get; set; }

            String SomeString { get; set; }
        }

        [Fact]
        public void AssignedPropertiesAreFound()
        {
            ProjectionPropertyModificationInterceptor interceptor;
            var proxy = (ITestProjectionModel)ProjectionModelProxyFactory.GenerateProxy(typeof(ITestProjectionModel), out interceptor);
            proxy.SomeInt = 1;
            proxy.SomeString = "sdfgdfgf";

            interceptor.ModifiedProperties.Count().Should().Be(2);
            interceptor.ModifiedProperties.Should().BeEquivalentTo(new[]
                                                                   {
                                                                       typeof(ITestProjectionModel).GetProperty("SomeInt"),
                                                                       typeof(ITestProjectionModel).GetProperty("SomeString")
                                                                   });

            proxy.Id.Should().Be(default(Guid));
            proxy.SomeInt.Should().Be(1);
            proxy.SomeString.Should().Be("sdfgdfgf");
        }

        [Fact]
        public void BaseInterfacePropertiesWork()
        {
            var testEvent = new Event<EventData>(
                0,
                new BoundedContextName("a"),
                Guid.NewGuid(),
                new EventTypeName("b"),
                DateTime.Now,
                new UserId("c"),
                null,
                new EventStreamIndex(3));

            ProjectionPropertyModificationInterceptor interceptor;
            var proxy = (ITestProjectionModel)ProjectionModelProxyFactory.GenerateProxy(typeof(ITestProjectionModel), out interceptor);
            proxy.SomeInt = 1;
            proxy.SomeString = "sdfgdfgf";

            proxy.ApplyKnownProperties(testEvent);

            interceptor.ModifiedProperties.Count().Should().Be(5);
            interceptor.ModifiedProperties.Select(x => x.Name).Should().BeEquivalentTo(new[]
                                                                   {
                                                                       "SomeInt",
                                                                       "SomeString",
                                                                       "LastEventNumber",
                                                                       "LastEventDate",
                                                                       "LastModifierId"
                                                                   });

            proxy.LastEventNumber.Should().Be(testEvent.EventStreamIndex.Value);
            proxy.LastModifierId.Should().Be(testEvent.CreatorId.Value);
            proxy.LastEventDate.Should().Be(testEvent.CreationTime);
        }
    }
}
