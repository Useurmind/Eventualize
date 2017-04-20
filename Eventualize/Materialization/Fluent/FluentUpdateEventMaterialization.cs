using System;
using System.Linq;
using System.Linq.Expressions;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class FluentUpdateEventMaterialization<TProjectionModel, TEvent> : IFluentUpdateEventMaterialization<TProjectionModel, TEvent>
        where TProjectionModel : IProjectionModel
        where TEvent : IEventData
    {
        private UpdateEventMaterializationAction action;
        private FluentMaterializationContext context;

        public FluentUpdateEventMaterialization(FluentMaterializationContext context)
        {
            this.action = new UpdateEventMaterializationAction()
                          {
                              ActionType = EventMaterializationActionType.Update,
                              EventType = typeof(TEvent),
                              ProjectionModelType = typeof(TProjectionModel)
                          };
            this.context = context;
        }

        public IFluentUpdateEventMaterialization<TProjectionModel, TEvent> Set(Action<TProjectionModel, TEvent> setProperties)
        {
            this.action.ApplyEventProperties = (projModel, eventData) => setProperties((TProjectionModel)projModel, (TEvent)eventData);
            return this;
        }

        public IFluentProjectionMaterialization<TProjectionModel> Where(Expression<Func<TProjectionModel, TEvent, bool>> compareKeys)
        {
            this.action.KeyComparissonExpression = compareKeys;
            this.context.RegisterEventMaterializationAction(this.action);
            return new FluentProjectionMaterialization<TProjectionModel>(this.context);
        }
    }
}