using System;
using System.Data;
using System.Linq;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Dapper.Materialization
{
    public class DapperMergeEventMaterializationActionHandler : IEventMaterializationActionHandler<IMergeEventMaterializationAction>
    {
        private Func<IDbConnection> getConnection;

        public EventMaterializationActionType ActionType { get { return EventMaterializationActionType.Merge; } }

        public DapperMergeEventMaterializationActionHandler(Func<IDbConnection> getConnection)
        {
            this.getConnection = getConnection;
        }

        public void Handle(IMergeEventMaterializationAction eventAction, IEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}