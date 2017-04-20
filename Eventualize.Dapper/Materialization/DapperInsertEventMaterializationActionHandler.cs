using System;
using System.Data;
using System.Linq;

using Dapper;

using Eventualize.Interfaces.Domain;
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

            this.getConnection().Execute($"insert into {tableName} {columnsAndValues}", projectionModel);
        }
    }
}