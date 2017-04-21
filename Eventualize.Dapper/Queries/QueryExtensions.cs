using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;

using Eventualize.Dapper.Materialization;
using Eventualize.Dapper.Proxies;

namespace Eventualize.Dapper.Queries
{
    public static class QueryExtensions
    {
        public static IEnumerable<TModel> QueryInterface<TModel>(this IDbConnection connection, string sql, object param=null)
        {
            var results = connection.Query(sql, param);

            return results.Select(x => (TModel)ProjectionModelProxyFactory.GenerateProxy(typeof(TModel), x));
        }
    }
}
