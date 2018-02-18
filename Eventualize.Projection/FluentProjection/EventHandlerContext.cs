using System;
using System.Linq;

using Eventualize.Projection.ProjectionMetaModel;

namespace Eventualize.Projection.FluentProjection
{
    public class EventHandlerContext
    {
        private Action<ProjectionEventHandler> onAddEventHandler;

        public EventHandlerContext(Action<ProjectionEventHandler> onAddEventHandler)
        {
            this.onAddEventHandler = onAddEventHandler;
        }

        public void AddEventHandler(ProjectionEventHandler eventHandler)
        {
            this.onAddEventHandler(eventHandler);
        }
    }
}