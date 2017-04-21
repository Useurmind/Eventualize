using System;
using System.Data;
using System.Linq;

using Dapper;

using Eventualize.Dapper.Proxies;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Dapper.Materialization
{
    public class DapperInsertEventMaterializationActionHandler : IEventMaterializationActionHandler<IInsertEventMaterializationAction>
    {
        private Func<IDbConnection> getConnection;

        public EventMaterializationActionType ActionType { get { return EventMaterializationActionType.Insert; } }

        public DapperInsertEventMaterializationActionHandler(Func<IDbConnection> getConnection)
        {
            this.getConnection = getConnection;
        }

        public void Handle(IInsertEventMaterializationAction eventAction, IEvent @event)
        {
            var tableName = eventAction.GetTableName();
            ProjectionPropertyModificationInterceptor interceptor;
            var projectionModel = ProjectionModelProxyFactory.GenerateProxy(eventAction.ProjectionModelType, out interceptor);
            projectionModel.ApplyKnownProperties(@event);
            eventAction.ApplyEventProperties(projectionModel, @event.EventData);
            
            var columnsAndValues = ReadModelExtensions.GetInsertColumnsAndValues(interceptor.ModifiedProperties);
            var keyProperties = projectionModel.GetKeyProperties();
            var allKeyPropertyNames = string.Join(", ", keyProperties.Select(x => x.Name));
            var allKeyPropertyParams = string.Join(", ", keyProperties.Select(x => $"@{x.Name}"));
            var allKeyCompare = string.Join(" and ", keyProperties.Select(x => $"target.{x.Name} = source.{x.Name}"));

            var statement = $@"merge {tableName} as target
using (select {allKeyPropertyParams}) AS source ({allKeyPropertyNames})
on {allKeyCompare}
when not matched
then insert {columnsAndValues};";

            this.getConnection().Execute(statement, projectionModel);
        }
    }
}