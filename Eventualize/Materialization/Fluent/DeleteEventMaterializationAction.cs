using System.Linq;
using System.Linq.Expressions;

using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class DeleteEventMaterializationAction : EventMaterializationAction, IDeleteEventMaterializationAction
    {
        public LambdaExpression KeyComparissonExpression { get; set; }
    }
}