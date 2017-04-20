using System;
using System.Data;
using System.Linq;

using Dapper;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Dapper.Materialization
{
    public class DapperDeleteEventMaterializationActionHandler : IEventMaterializationActionHandler<IDeleteEventMaterializationAction>
    {
        private Func<IDbConnection> getConnection;

        public EventMaterializationActionType ActionType { get { return EventMaterializationActionType.Delete; } }

        public DapperDeleteEventMaterializationActionHandler(Func<IDbConnection> getConnection)
        {
            this.getConnection = getConnection;
        }

        public void Handle(IDeleteEventMaterializationAction eventAction, IEvent @event)
        {
            var keyCompare = new KeyCompareExpressionVisitor(eventAction.ProjectionModelType, eventAction.EventType).ComputeKeyComparision(eventAction.KeyComparissonExpression);
            var tableName = eventAction.GetTableName();

            this.getConnection().Execute($"delete from ${tableName} where ${keyCompare}");
        }
    }
}