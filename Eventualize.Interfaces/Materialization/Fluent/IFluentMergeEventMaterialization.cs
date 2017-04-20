using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IFluentMergeEventMaterialization<TProjectionModel, TEvent> : IFluentEventMaterialization<IFluentMergeEventMaterialization<TProjectionModel, TEvent>, TProjectionModel, TEvent>
        where TEvent : IEventData
        where TProjectionModel : IProjectionModel
    {
    }
}