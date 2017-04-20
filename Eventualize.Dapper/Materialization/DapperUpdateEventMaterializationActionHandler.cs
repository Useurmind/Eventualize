using System;
using System.Data;
using System.Linq;

using Dapper;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Dapper.Materialization
{
    public class DapperUpdateEventMaterializationActionHandler : IEventMaterializationActionHandler<IUpdateEventMaterializationAction>
    {
        private Func<IDbConnection> getConnection;

        public EventMaterializationActionType ActionType { get { return EventMaterializationActionType.Update; } }

        public DapperUpdateEventMaterializationActionHandler(Func<IDbConnection> getConnection)
        {
            this.getConnection = getConnection;
        }

        public void Handle(IUpdateEventMaterializationAction eventAction, IEvent @event)
        {
            var tableName = eventAction.GetTableName();
            ProjectionPropertyModificationInterceptor interceptor;
            var projectionModel = ProjectionModelProxyFactory.GenerateProxy(eventAction.ProjectionModelType, out interceptor);
            projectionModel.ApplyKnownProperties(@event);
            eventAction.ApplyEventProperties(projectionModel, @event.EventData);

            var keyCompare = new KeyCompareExpressionVisitor(eventAction.ProjectionModelType, eventAction.EventType).ComputeKeyComparision(eventAction.KeyComparissonExpression);
            var columnsAndValues = ReadModelExtensions.GetUpdateColumnsAndValues(interceptor.ModifiedProperties);

            var parameters = new DynamicParameters(projectionModel);
            foreach (var eventKeys in keyCompare.EventKeyProperties)
            {
                parameters.Add(eventKeys.ParameterName, eventKeys.Property.GetValue(@event.EventData));
            }

            this.getConnection().Execute($"update {tableName} set {columnsAndValues} where {keyCompare.KeyCompareClause}", parameters);
        }
    }
}