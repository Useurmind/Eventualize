using System.Linq;

namespace Eventualize.Projection.FluentProjection
{
    public interface IFluentMergeEventHandler<TProjectionModel, TEvent> : IFluentEventHandler<IFluentMergeEventHandler<TProjectionModel, TEvent>, TProjectionModel, TEvent>
    {
    }
}