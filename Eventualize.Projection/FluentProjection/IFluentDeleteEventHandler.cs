using System;
using System.Linq;
using System.Linq.Expressions;

namespace Eventualize.Projection.FluentProjection
{
    public interface IFluentDeleteEventHandler<TProjectionModel, TEvent> 
    {
        IFluentProjection<TProjectionModel> Where(Expression<Func<TProjectionModel, TEvent, bool>> compareKeys);
    }
}