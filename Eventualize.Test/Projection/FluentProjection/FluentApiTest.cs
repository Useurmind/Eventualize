using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Projection.FluentProjection;
using Eventualize.Projection.ProjectionMetaModel;

using FluentAssertions;

using NSubstitute;

using Xunit;

namespace Eventualize.Test.Projection.FluentProjection
{
    public class FluentApiTest
    {
        private class MyModel2
        {
        }

        private class MyModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class MyInsertEvent
        {
            public Guid EventId { get; set; }

            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class MyUpdateEvent
        {
            public Guid EventId { get; set; }

            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class MyDeleteEvent
        {
            public Guid EventId { get; set; }

            public int Id { get; set; }
            public string Name { get; set; }
        }

        private class MyMergeEvent
        {
            public Guid EventId { get; set; }

            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Fact]
        public void SimpleApiCallWorks()
        {
            var topic = Substitute.For<ITopic>();

            TopicModel topicProjection = topic.ProjectTo<MyModel>(
                p =>
                    {
                        p.InsertOn<MyInsertEvent>().Set(
                            (m, e) =>
                                {
                                    m.Id = e.Id;
                                    m.Name = e.Name;
                                });

                        p.UpdateOn<MyUpdateEvent>()
                         .Set((m, e) => { m.Name = e.Name; })
                         .Where((m, e) => m.Id == e.Id);

                        p.MergeOn<MyMergeEvent>()
                         .Set((m, e) => { m.Name = e.Name; })
                         .Where((m, e) => m.Id == e.Id);

                        p.DeleteOn<MyDeleteEvent>().Where((m, e) => m.Id == e.Id);
                    }).ProjectTo<MyModel2>(p => { }).Build();

            topicProjection.Should().NotBeNull();
            topicProjection.Topic.Should().BeSameAs(topic);
            topicProjection.Projections.Count().Should().Be(2);

            var myModelProjection = topicProjection.Projections.First();
            myModelProjection.ProjectionModelType.Should().Be(typeof(MyModel));
            myModelProjection.EventHandlers.Count().Should().Be(4);

            var insertEventHandler = myModelProjection.EventHandlers.First();
            insertEventHandler.EventType = typeof(MyInsertEvent);
            insertEventHandler.ActionType = ProjectionEventActionType.Insert;
            insertEventHandler.Set.Should().NotBeNull();
            insertEventHandler.Where.Should().BeNull();

            var updateEventHandler = myModelProjection.EventHandlers.Skip(1).First();
            updateEventHandler.EventType = typeof(MyUpdateEvent);
            updateEventHandler.ActionType = ProjectionEventActionType.Update;
            updateEventHandler.Set.Should().NotBeNull();
            updateEventHandler.Where.Should().NotBeNull();

            var mergeEventHandler = myModelProjection.EventHandlers.Skip(2).First();
            mergeEventHandler.EventType = typeof(MyMergeEvent);
            mergeEventHandler.ActionType = ProjectionEventActionType.Merge;
            mergeEventHandler.Set.Should().NotBeNull();
            mergeEventHandler.Where.Should().NotBeNull();

            var deleteEventHandler = myModelProjection.EventHandlers.Skip(3).First();
            deleteEventHandler.EventType = typeof(MyDeleteEvent);
            deleteEventHandler.ActionType = ProjectionEventActionType.Delete;
            deleteEventHandler.Set.Should().BeNull();
            deleteEventHandler.Where.Should().NotBeNull();

            var myModel2Projection = topicProjection.Projections.Skip(1).First();
            myModel2Projection.ProjectionModelType.Should().Be(typeof(MyModel2));
            myModel2Projection.EventHandlers.Count().Should().Be(0);
        }

        [Fact]
        public void ApiThrowsErrorForDuplicateModel()
        {
            var topic = Substitute.For<ITopic>();

            Assert.Throws<Exception>(
                () =>
                    {
                        TopicModel topicProjection =
                            topic.ProjectTo<MyModel>(p => { }).ProjectTo<MyModel>(p => { }).Build();
                    });
        }

        [Fact]
        public void ApiThrowsErrorForDuplicateEventHandler()
        {
            var topic = Substitute.For<ITopic>();

            Assert.Throws<Exception>(
                () =>
                {
                    TopicModel topicProjection =
                        topic.ProjectTo<MyModel>(
                            p =>
                                { p.InsertOn<MyInsertEvent>().Set((m, e) => m.Name = e.Name)
                                    .InsertOn<MyInsertEvent>().Set((m, e) => m.Name = e.Name); }).Build();
                });
        }
    }
}