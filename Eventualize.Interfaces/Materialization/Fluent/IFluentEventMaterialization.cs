using System;
using System.Linq;
using System.Linq.Expressions;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IFluentEventMaterialization<TThis, TProjectionModel, TEvent>
        where TEvent : IEventData 
        where TProjectionModel : IProjectionModel
    {
        TThis Set(Action<TProjectionModel, TEvent> setProperties);

        IFluentProjectionMaterialization<TProjectionModel> Where(Expression<Func<TProjectionModel, TEvent, bool>> compareKeys);
    }
}