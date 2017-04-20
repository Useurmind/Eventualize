using System.Linq;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class FluentProjectionMaterialization<TProjectionModel> : IFluentProjectionMaterialization<TProjectionModel>
        where TProjectionModel : IProjectionModel
    {
        private FluentMaterializationContext context;

        public FluentProjectionMaterialization(FluentMaterializationContext context)
        {
            this.context = context;
        }

        public IFluentMergeEventMaterialization<TProjectionModel, TEvent> MergeOn<TEvent>() where TEvent : IEventData
        {
            return new FluentMergeEventMaterialization<TProjectionModel, TEvent>(this.context);
        }

        public IFluentInsertEventMaterialization<TProjectionModel, TEvent> InsertOn<TEvent>() where TEvent : IEventData
        {
            return new FluentInsertEventMaterialization<TProjectionModel, TEvent>(this.context);
        }

        public IFluentUpdateEventMaterialization<TProjectionModel, TEvent> UpdateOn<TEvent>() where TEvent : IEventData
        {
            return new FluentUpdateEventMaterialization<TProjectionModel, TEvent>(this.context);
        }

        public IFluentDeleteEventMaterialization<TProjectionModel, TEvent> DeleteOn<TEvent>() where TEvent : IEventData
        {
            return new FluentDeleteEventMaterialization<TProjectionModel, TEvent>(this.context);
        }

        public IFluentProjectionMaterialization<TProjectionModel1> Model<TProjectionModel1>() where TProjectionModel1 : IProjectionModel
        {
            return new FluentProjectionMaterialization<TProjectionModel1>(this.context);
        }
    }
}