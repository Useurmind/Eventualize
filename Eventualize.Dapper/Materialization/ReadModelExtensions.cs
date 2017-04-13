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

        public static PropertyInfo GetEventNumberProperty(this IReadModel readModel)
        {
            return readModel.GetType().GetProperties().First(p => p.Name == "LastEventNumber");
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
            string versionCheckClause = GetVersionCheckClause(readModel);

            string command = $@"merge {tableName} as target
using (select @{keyPropertyName}) AS source ({keyPropertyName})
on target.{keyPropertyName} = source.{keyPropertyName}
when matched {versionCheckClause}
then {mergeUpdateClause}
when not matched
then {mergeInsertClause};
";

            return command;
        }

        /// <summary>
        /// The returned version check prohibits updates to the table that would restore an older version.
        /// That is good if stuff is replayed and we do not want to apply the replays.
        /// If we want to return to a point in history we need to delete the entries first or turn of this version check.
        /// </summary>
        /// <param name="readModel"></param>
        /// <returns></returns>
        private static string GetVersionCheckClause(IReadModel readModel)
        {
            var eventNumberProperty = readModel.GetEventNumberProperty();
            var eventNumberPropertyName = eventNumberProperty.Name;

            return $"and source.{eventNumberPropertyName} > target.{eventNumberPropertyName}";

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