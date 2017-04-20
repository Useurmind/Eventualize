using System;
using System.Linq;
using System.Linq.Expressions;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class FluentDeleteEventMaterialization<TProjectionModel, TEvent> : IFluentDeleteEventMaterialization<TProjectionModel, TEvent>
        where TProjectionModel : IProjectionModel
        where TEvent : IEventData
    {
        private DeleteEventMaterializationAction action;
        private FluentMaterializationContext context;

        public FluentDeleteEventMaterialization(FluentMaterializationContext context)
        {
            this.action = new DeleteEventMaterializationAction()
                          {
                              ActionType = EventMaterializationActionType.Delete,
                              EventType = typeof(TEvent),
                              ProjectionModelType = typeof(TProjectionModel)
                          };
            this.context = context;
        }

        public IFluentProjectionMaterialization<TProjectionModel> Where(Expression<Func<TProjectionModel, TEvent, bool>> compareKeys)
        {
            this.action.KeyComparissonExpression = compareKeys;
            this.context.RegisterEventMaterializationAction(this.action);
            return new FluentProjectionMaterialization<TProjectionModel>(this.context);
        }
    }
}