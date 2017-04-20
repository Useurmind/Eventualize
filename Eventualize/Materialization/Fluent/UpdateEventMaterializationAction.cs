using System;
using System.Linq;
using System.Linq.Expressions;

using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class UpdateEventMaterializationAction : EventMaterializationAction, IUpdateEventMaterializationAction
    {
        public Action<object, object> ApplyEventProperties { get; set; }

        public LambdaExpression KeyComparissonExpression { get; set; }
    }
}