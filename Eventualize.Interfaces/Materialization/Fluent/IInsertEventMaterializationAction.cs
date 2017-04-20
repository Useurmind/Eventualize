using System;
using System.Linq;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IInsertEventMaterializationAction : IEventMaterializationAction
    {
        Action<object, object> ApplyEventProperties { get; }
    }
}