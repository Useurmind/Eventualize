using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IFluentUpdateEventMaterialization<TProjectionModel, TEvent> : IFluentEventMaterialization<IFluentUpdateEventMaterialization<TProjectionModel, TEvent>, TProjectionModel, TEvent>
        where TEvent : IEventData
        where TProjectionModel : IProjectionModel
    {
    }
}