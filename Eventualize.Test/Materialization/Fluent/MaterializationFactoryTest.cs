using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Infrastructure;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Fluent;
using Eventualize.Materialization.Fluent;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Eventualize.Test.Materialization.Fluent
{
    public class MaterializationFactoryTest
    {
        private class ProjectionModel : IProjectionModel
        {
            public Guid Id { get; set; }

            public string SomeString { get; set; }

            public long LastEventNumber { get; set; }

            public DateTime LastEventDate { get; set; }

            public string LastModifierId { get; set; }
        }

        private class EventModel : IEventData
        {
            public Guid Id { get; set; }

            public string SomeString { get; set; }

        }

        private class InsertEventModel : EventModel
        {
            
        }

        private class DeleteEventModel : EventModel
        {

        }

        private class UpdateEventModel : EventModel
        {

        }

        private class MergeEventModel : EventModel
        {

        }

        [Fact]
        public void FactoryCorrectlyRegistersInsertUpdateDeleteMergeActions()
        {
            var registeredActions = new List<IEventMaterializationAction>();
            var containerBuilder = Substitute.For<IEventualizeContainerBuilder>();
            containerBuilder.RegisterSingleInstance(Arg.Any<Func<IEventualizeContainer, IEventMaterializationAction>>()).Returns(
                (ci) =>
                {
                    registeredActions.Add(ci.Arg<Func<IEventualizeContainer, IEventMaterializationAction>>().Invoke(null));
                    return containerBuilder;
                });

            var factory = new MaterializationFactory(containerBuilder);

            factory.Model<ProjectionModel>()
                .InsertOn<InsertEventModel>().Set((p, e) => p.Id = e.Id)
                .UpdateOn<UpdateEventModel>().Set((p, e) => p.SomeString = e.SomeString).Where((p, e) => p.Id == e.Id)
                .DeleteOn<DeleteEventModel>().Where((p, e) => p.Id == e.Id)
                .MergeOn<MergeEventModel>().Set((p, e) => p.Id = e.Id).Where((p, e) => p.Id == e.Id);

            var insertAction = registeredActions.FirstOrDefault(x => x.ActionType == EventMaterializationActionType.Insert).As<IInsertEventMaterializationAction>();
            var updateAction = registeredActions.FirstOrDefault(x => x.ActionType == EventMaterializationActionType.Update).As<IUpdateEventMaterializationAction>();
            var deleteAction = registeredActions.FirstOrDefault(x => x.ActionType == EventMaterializationActionType.Delete).As<IDeleteEventMaterializationAction>();
            var mergeAction = registeredActions.FirstOrDefault(x => x.ActionType == EventMaterializationActionType.Merge).As<IMergeEventMaterializationAction>();

            insertAction.Should().NotBeNull();
            updateAction.Should().NotBeNull();
            deleteAction.Should().NotBeNull();
            mergeAction.Should().NotBeNull();

            insertAction.EventType.Should().Be(typeof(InsertEventModel));
            insertAction.ProjectionModelType.Should().Be(typeof(ProjectionModel));
            insertAction.ApplyEventProperties.Should().NotBeNull();

            updateAction.EventType.Should().Be(typeof(UpdateEventModel));
            updateAction.ProjectionModelType.Should().Be(typeof(ProjectionModel));
            updateAction.ApplyEventProperties.Should().NotBeNull();
            updateAction.KeyComparissonExpression.Should().NotBeNull();

            deleteAction.EventType.Should().Be(typeof(DeleteEventModel));
            deleteAction.ProjectionModelType.Should().Be(typeof(ProjectionModel));
            deleteAction.KeyComparissonExpression.Should().NotBeNull();

            mergeAction.EventType.Should().Be(typeof(MergeEventModel));
            mergeAction.ProjectionModelType.Should().Be(typeof(ProjectionModel));
            mergeAction.ApplyEventProperties.Should().NotBeNull();
            mergeAction.KeyComparissonExpression.Should().NotBeNull();

        }
    }
}
