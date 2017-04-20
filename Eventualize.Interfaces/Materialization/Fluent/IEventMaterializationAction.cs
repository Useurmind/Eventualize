using System;
using System.Linq;
using System.Linq.Expressions;

namespace Eventualize.Interfaces.Materialization.Fluent
{
    public interface IEventMaterializationAction
    {
        EventMaterializationActionType ActionType { get; }

        Type ProjectionModelType { get; }

        Type EventType { get; }
    }
}