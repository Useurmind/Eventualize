using System;
using System.Linq;
using System.Linq.Expressions;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IFluentDeleteEventMaterialization<TProjectionModel, TEvent> 
        where TEvent : IEventData
        where TProjectionModel : IProjectionModel
    {
        IFluentProjectionMaterialization<TProjectionModel> Where(Expression<Func<TProjectionModel, TEvent, bool>> compareKeys);
    }
}