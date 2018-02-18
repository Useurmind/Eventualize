using System;
using System.Linq;
using System.Linq.Expressions;

using Eventualize.Projection.ProjectionMetaModel;

namespace Eventualize.Projection.FluentProjection
{
    public class UpdateEventHandlerContext<TProjectionModel, TEvent> : IFluentUpdateEventHandler<TProjectionModel, TEvent>
    {
        private EventHandlerContext context;

        private IFluentProjection<TProjectionModel> fluentProjection;

        private ProjectionEventHandler projectionEventHandler;

        public UpdateEventHandlerContext(EventHandlerContext context, IFluentProjection<TProjectionModel> fluentProjection)
        {
            this.context = context;
            this.fluentProjection = fluentProjection;

            this.projectionEventHandler = new ProjectionEventHandler()
            {
                ActionType = ProjectionEventActionType.Update,
                EventType = typeof(TEvent)
            };
        }

        public IFluentUpdateEventHandler<TProjectionModel, TEvent> Set(Action<TProjectionModel, TEvent> setProperties)
        {
            this.projectionEventHandler.Set = (m, e) => setProperties((TProjectionModel)m, (TEvent)e);

            return this;
        }

        public IFluentProjection<TProjectionModel> Where(Expression<Func<TProjectionModel, TEvent, bool>> compareKeys)
        {
            this.projectionEventHandler.Where = compareKeys;

            this.context.AddEventHandler(this.projectionEventHandler);

            return this.fluentProjection;
        }
    }
}