using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IFluentProjectionMaterialization<TProjectionModel> : IMaterializationFactory
        where TProjectionModel : IProjectionModel
    {
        IFluentMergeEventMaterialization<TProjectionModel, TEvent> MergeOn<TEvent>() 
            where TEvent : IEventData;

        IFluentInsertEventMaterialization<TProjectionModel, TEvent> InsertOn<TEvent>()
            where TEvent : IEventData;

        IFluentUpdateEventMaterialization<TProjectionModel, TEvent> UpdateOn<TEvent>() 
            where TEvent : IEventData;

        IFluentDeleteEventMaterialization<TProjectionModel, TEvent> DeleteOn<TEvent>() 
            where TEvent : IEventData;
    }
}