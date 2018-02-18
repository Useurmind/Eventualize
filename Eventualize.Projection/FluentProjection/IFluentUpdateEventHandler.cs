using System.Linq;

namespace Eventualize.Projection.FluentProjection
{
    public interface IFluentUpdateEventHandler<TProjectionModel, TEvent> : IFluentEventHandler<IFluentUpdateEventHandler<TProjectionModel, TEvent>, TProjectionModel, TEvent>
    {
    }
}