using System;
using System.Linq;
using System.Linq.Expressions;

namespace Eventualize.Projection.FluentProjection
{
    public interface IFluentEventHandler<TThis, TProjectionModel, TEvent>
    {
        TThis Set(Action<TProjectionModel, TEvent> setProperties);

        IFluentProjection<TProjectionModel> Where(Expression<Func<TProjectionModel, TEvent, bool>> compareKeys);
    }
}