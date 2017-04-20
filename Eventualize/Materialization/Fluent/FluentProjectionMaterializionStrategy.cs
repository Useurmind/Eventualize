using System;
using System.Collections.Generic;
using System.Linq;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class FluentProjectionMaterializionStrategy : IMaterializationStrategy
    {
        private IDictionary<Type, IEnumerable<IEventMaterializationAction>> actionsByEventType;
        private IDictionary<EventMaterializationActionType, IEnumerable<IEventMaterializationActionHandler>> handlersByActionType;

        public FluentProjectionMaterializionStrategy(IEnumerable<IEventMaterializationAction> materializationActions, IEnumerable<IEventMaterializationActionHandler> eventHandlers)
        {
            this.actionsByEventType = materializationActions.GroupBy(x => x.EventType).ToDictionary(x => x.Key, x => x.AsEnumerable());
            this.handlersByActionType = eventHandlers.GroupBy(x => x.ActionType).ToDictionary(x => x.Key, x => x.AsEnumerable());
        }

        public void HandleEvent(IEvent @event)
        {
            IEnumerable<IEventMaterializationAction> eventActions;
            if (!this.actionsByEventType.TryGetValue(@event.EventData.GetType(), out eventActions))
            {
                eventActions = Enumerable.Empty<IEventMaterializationAction>();
            }

            foreach (var eventAction in eventActions)
            {
                this.ApplyEventAction(eventAction, @event);
            }

        }

        private void ApplyEventAction(IEventMaterializationAction eventAction, IEvent @event)
        {
            IEnumerable<IEventMaterializationActionHandler> eventHandlers;
            if (!this.handlersByActionType.TryGetValue(eventAction.ActionType, out eventHandlers))
            {
                eventHandlers = Enumerable.Empty<IEventMaterializationActionHandler>();
            }

            foreach (var eventHandler in eventHandlers)
            {
                eventHandler.GetType().GetMethod("Handle").Invoke(eventHandler, new object[] { eventAction, @event});
            }
        }
    }
}