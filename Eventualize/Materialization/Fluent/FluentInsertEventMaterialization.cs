using System;
using System.Linq;
using System.Linq.Expressions;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class FluentInsertEventMaterialization<TProjectionModel, TEvent> : IFluentInsertEventMaterialization<TProjectionModel, TEvent>
        where TProjectionModel : IProjectionModel
        where TEvent : IEventData
    {
        private InsertEventMaterializationAction action;
        private FluentMaterializationContext context;

        public FluentInsertEventMaterialization(FluentMaterializationContext context)
        {
            this.action = new InsertEventMaterializationAction()
                          {
                              ActionType = EventMaterializationActionType.Insert,
                              EventType = typeof(TEvent),
                              ProjectionModelType = typeof(TProjectionModel)
                          };
            this.context = context;
        }

        public IFluentProjectionMaterialization<TProjectionModel> Set(Action<TProjectionModel, TEvent> setProperties)
        {
            this.action.ApplyEventProperties = (projModel, eventData) => setProperties((TProjectionModel)projModel, (TEvent)eventData);
            this.context.RegisterEventMaterializationAction(this.action);
            return new FluentProjectionMaterialization<TProjectionModel>(this.context);
        }
    }
}