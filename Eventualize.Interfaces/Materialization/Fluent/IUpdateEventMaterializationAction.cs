using System;
using System.Linq;
using System.Linq.Expressions;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IUpdateEventMaterializationAction : IEventMaterializationAction
    {
        Action<object, object> ApplyEventProperties { get; }

        /// <summary>
        /// This lambda has two inputs:
        /// - projection model
        /// - event model
        /// It returns true if the keys of both match and false else. 
        /// </summary>
        LambdaExpression KeyComparissonExpression { get; }
    }
}