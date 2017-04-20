using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Dapper.Materialization;
using Eventualize.Interfaces.Infrastructure;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Dapper.Infrastructure
{
    public static class ContainerExtensions
    {
        public static IEventualizeContainerBuilder FluentMaterializationWithDapper(this IEventualizeContainerBuilder containerBuilder)
        {
            return containerBuilder.RegisterSingleInstance<IEventMaterializationActionHandler>(
                                c => new DapperInsertEventMaterializationActionHandler(c.Resolve<Func<IDbConnection>>()))
                            .RegisterSingleInstance<IEventMaterializationActionHandler>(
                                c => new DapperUpdateEventMaterializationActionHandler(c.Resolve<Func<IDbConnection>>()))
                            .RegisterSingleInstance<IEventMaterializationActionHandler>(
                                c => new DapperDeleteEventMaterializationActionHandler(c.Resolve<Func<IDbConnection>>()))
                            .RegisterSingleInstance<IEventMaterializationActionHandler>(
                                c => new DapperMergeEventMaterializationActionHandler(c.Resolve<Func<IDbConnection>>()));
        }

        public static IEventualizeContainerBuilder ConnectDapperToSqlServer(
            this IEventualizeContainerBuilder containerBuilder,
            string connectionString)
        {
            return
                containerBuilder.RegisterSingleInstance<Func<IDbConnection>>(
                    c => new Func<IDbConnection>(() => new SqlConnection(connectionString)));
        }
    }
}
