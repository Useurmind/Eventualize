using System;
using System.Linq;

using Eventualize.Projection.ProjectionMetaModel;

namespace Eventualize.Projection.FluentProjection
{
    public class ProjectionContext<TProjectionModel> : IFluentProjection<TProjectionModel>
    {
        private ProjectionModel projectionModel;

        private EventHandlerContext eventHandlerContext;

        public ProjectionContext(ITopic topic)
        {
            this.projectionModel = new ProjectionModel()
            {
                ProjectionModelType = typeof(TProjectionModel)
            };

            this.eventHandlerContext = new EventHandlerContext(
                eh =>
                    {
                        if (this.projectionModel.EventHandlers.Any(e => e.EventType == eh.EventType))
                        {
                            throw new Exception($"The event type {eh.EventType.FullName} was already handled for model {typeof(TProjectionModel).FullName} in topic {topic.Name}");
                        }

                        this.projectionModel.EventHandlers = this.projectionModel.EventHandlers.Union(new[] { eh });
                    });
        }

        /// <inheritdoc />
        public IFluentMergeEventHandler<TProjectionModel, TEvent> MergeOn<TEvent>()
        {
            return new MergeEventHandlerContext<TProjectionModel, TEvent>(this.eventHandlerContext, this);
        }

        /// <inheritdoc />
        public IFluentInsertEventHandler<TProjectionModel, TEvent> InsertOn<TEvent>()
        {
            return new InsertEventHandlerContext<TProjectionModel, TEvent>(this.eventHandlerContext, this);
        }

        /// <inheritdoc />
        public IFluentUpdateEventHandler<TProjectionModel, TEvent> UpdateOn<TEvent>()
        {
            return new UpdateEventHandlerContext<TProjectionModel, TEvent>(this.eventHandlerContext, this);
        }

        /// <inheritdoc />
        public IFluentDeleteEventHandler<TProjectionModel, TEvent> DeleteOn<TEvent>()
        {
            return new DeleteEventHandlerContext<TProjectionModel, TEvent>(this.eventHandlerContext, this);
        }

        public ProjectionModel Build()
        {
            return this.projectionModel;
        }
    }
}