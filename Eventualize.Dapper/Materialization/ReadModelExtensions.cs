using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

using Dapper;

using Eventualize.Materialization;

namespace Eventualize.Dapper.Materialization
{
    public static class ReadModelExtensions
    {
        public static string GetTableName(this IReadModel readModel)
        {
            return readModel.GetType().GetTableName();
        }

        public static string GetTableName(this Type readModelType)
        {
            return readModelType.GetCustomAttribute<TableAttribute>().FullQualifiedName;
        }

        public static PropertyInfo GetKeyProperty(this IReadModel readModel)
        {
            return readModel.GetType().GetProperties().First(p => p.GetCustomAttribute<KeyAttribute>() != null);
        }

        public static int Merge(this IDbConnection connection, IReadModel readModel)
        {
            var command = readModel.GetInsertOrUpdateCommand();
            return connection.Execute(command, readModel);
        }

        public static string GetInsertOrUpdateCommand(this IReadModel readModel)
        {
            var tableName = readModel.GetTableName();
            var keyPropertyName = readModel.GetKeyProperty().Name;
            var keyPropertyValue = readModel.GetKeyProperty().GetValue(readModel);

            string mergeUpdateClause = GetMergeUpdateClause(readModel);
            string mergeInsertClause = GetMergeInsertClause(readModel);


            string command = $@"merge {tableName} as target
using (select @{keyPropertyName}) AS source ({keyPropertyName})
on target.{keyPropertyName} = source.{keyPropertyName}
when matched
then {mergeUpdateClause}
when not matched
then {mergeInsertClause};
";

            return command;
        }

        private static string GetMergeUpdateClause(IReadModel readModel)
        {
            var nonKeyProperties = readModel.GetNonKeyProperties();
            var columnsAndValues = string.Join(", ", nonKeyProperties.Select(x => $"{x.Name} = @{x.Name}"));


            return $"update set {columnsAndValues}";
        }

        private static string GetMergeInsertClause(IReadModel readModel)
        {
            var nonKeyProperties = readModel.GetType().GetProperties();
            var columns = string.Join(", ", nonKeyProperties.Select(x => x.Name));
            var values = string.Join(", ", nonKeyProperties.Select(x => $"@{x.Name}"));


            return $"insert ({columns}) values ({values})";
        }

        private static IEnumerable<PropertyInfo> GetNonKeyProperties(this IReadModel readModel)
        {
            return readModel.GetType().GetProperties().Where(x => x.GetCustomAttribute<KeyAttribute>() == null);
        }
    }
}