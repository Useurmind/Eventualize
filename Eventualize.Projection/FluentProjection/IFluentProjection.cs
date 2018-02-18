using System.Linq;

namespace Eventualize.Projection.FluentProjection
{
    public interface IFluentProjection<TProjectionModel>
    {
        IFluentMergeEventHandler<TProjectionModel, TEvent> MergeOn<TEvent>();

        IFluentInsertEventHandler<TProjectionModel, TEvent> InsertOn<TEvent>();

        IFluentUpdateEventHandler<TProjectionModel, TEvent> UpdateOn<TEvent>();

        IFluentDeleteEventHandler<TProjectionModel, TEvent> DeleteOn<TEvent>();
    }
}