using System;
using System.Linq;

using Eventualize.Interfaces.Domain;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IFluentInsertEventMaterialization<TProjectionModel, TEvent> 
        where TEvent : IEventData
        where TProjectionModel : IProjectionModel
    {
        IFluentProjectionMaterialization<TProjectionModel> Set(Action<TProjectionModel, TEvent> setProperties);
    }
}