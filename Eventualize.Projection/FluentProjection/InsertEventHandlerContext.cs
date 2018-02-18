using System;
using System.Linq;

using Eventualize.Projection.ProjectionMetaModel;

namespace Eventualize.Projection.FluentProjection
{
    public class InsertEventHandlerContext<TProjectionModel, TEvent> : IFluentInsertEventHandler<TProjectionModel, TEvent>
    {
        private EventHandlerContext context;

        private IFluentProjection<TProjectionModel> fluentProjection;

        private ProjectionEventHandler projectionEventHandler;

        public InsertEventHandlerContext(EventHandlerContext context, IFluentProjection<TProjectionModel> fluentProjection)
        {
            this.context = context;
            this.fluentProjection = fluentProjection;

            this.projectionEventHandler = new ProjectionEventHandler()
            {
                ActionType = ProjectionEventActionType.Insert,
                EventType = typeof(TEvent)
            };
        }

        public IFluentProjection<TProjectionModel> Set(Action<TProjectionModel, TEvent> setProperties)
        {
            this.projectionEventHandler.Set = (m, e) => setProperties((TProjectionModel)m, (TEvent)e);

            this.context.AddEventHandler(this.projectionEventHandler);

            return this.fluentProjection;
        }
    }
}