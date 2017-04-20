using System;
using System.Linq;
using System.Linq.Expressions;

using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Materialization.Fluent
{
    public class EventMaterializationAction : IEventMaterializationAction
    {
        public EventMaterializationActionType ActionType { get; set; }

        public Type ProjectionModelType { get; set; }

        public Type EventType { get; set; }
    }
}