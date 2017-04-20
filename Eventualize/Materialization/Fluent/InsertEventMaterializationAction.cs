using System;
using System.Linq;

using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class InsertEventMaterializationAction : EventMaterializationAction, IInsertEventMaterializationAction
    {
        public Action<object, object> ApplyEventProperties { get; set; }
    }
}