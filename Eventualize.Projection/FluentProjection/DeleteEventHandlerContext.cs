using System;
using System.Linq;
using System.Linq.Expressions;

using Eventualize.Projection.ProjectionMetaModel;

namespace Eventualize.Projection.FluentProjection
{
    public class DeleteEventHandlerContext<TProjectionModel, TEvent> : IFluentDeleteEventHandler<TProjectionModel, TEvent>
    {
        private EventHandlerContext context;

        private ProjectionEventHandler projectionEventHandler;

        private IFluentProjection<TProjectionModel> fluentProjection;

        public DeleteEventHandlerContext(EventHandlerContext context, IFluentProjection<TProjectionModel> fluentProjection)
        {
            this.context = context;
            this.fluentProjection = fluentProjection;

            this.projectionEventHandler = new ProjectionEventHandler()
                                              {
                                                  ActionType= ProjectionEventActionType.Delete,
                                                  EventType = typeof(TEvent)
                                              };
        }

        public IFluentProjection<TProjectionModel> Where(Expression<Func<TProjectionModel, TEvent, bool>> compareKeys)
        {
            this.projectionEventHandler.Where = compareKeys;
            
            this.context.AddEventHandler(this.projectionEventHandler);

            return this.fluentProjection;
        }
    }
}