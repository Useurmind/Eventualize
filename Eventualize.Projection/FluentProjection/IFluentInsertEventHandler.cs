using System;
using System.Linq;

namespace Eventualize.Projection.FluentProjection
{
    public interface IFluentInsertEventHandler<TProjectionModel, TEvent> 
    {
        IFluentProjection<TProjectionModel> Set(Action<TProjectionModel, TEvent> setProperties);
    }
}