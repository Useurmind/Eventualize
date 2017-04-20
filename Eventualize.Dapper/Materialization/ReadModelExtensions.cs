using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Dapper;

using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Materialization;
using Eventualize.Interfaces.Materialization.Fluent;

namespace Eventualize.Dapper.Materialization
{
    public static class ReadModelExtensions
    {
        public static string GetTableName(this IReadModel readModel)
        {
            return readModel.GetType().GetTableName();
        }

        public static string GetTableName(this IEventMaterializationAction eventMaterializationAction)
        {
            return eventMaterializationAction.ProjectionModelType.GetTableName();
        }

        public static string GetTableName(this Type readModelType)
        {
            return readModelType.GetCustomAttribute<TableAttribute>().FullQualifiedName;
        }

        public static void ApplyKnownProperties(this object projectionModel, IEvent @event)
        {
            var asProjectionModel = projectionModel as IProjectionModel;
            if (asProjectionModel != null)
            {
                asProjectionModel.LastEventDate = @event.CreationTime;
                asProjectionModel.LastEventNumber = @event.EventStreamIndex.Value;
                asProjectionModel.LastModifierId = @event.CreatorId.Value;
            }

            var asReadModel = projectionModel as IReadModel;
            var asAggregateEvent = @event as IAggregateEvent;
            if (asReadModel != null)
            {
                asReadModel.Version = asAggregateEvent != null ? asAggregateEvent.EventStreamIndex.ToAggregateVersion().Value : -1;
            }
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
using (select @{keyPropertyName}, @LastEventNumber) AS source ({keyPropertyName}, LastEventNumber)
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
            var columnsAndValues = GetInsertColumnsAndValues(readModel);
            
            return $"insert {columnsAndValues}";
        }

        public static string GetUpdateColumnsAndValues(this object projectionModel)
        {
            var nonKeyProperties = projectionModel.GetType().GetProperties();
            return GetUpdateColumnsAndValues(nonKeyProperties);
        }

        public static string GetInsertColumnsAndValues(this object projectionModel)
        {
            var nonKeyProperties = projectionModel.GetType().GetProperties();
            return GetInsertColumnsAndValues(nonKeyProperties);
        }

        public static string GetInsertColumnsAndValues(IEnumerable<PropertyInfo> properties)
        {
            var columns = string.Join(", ", properties.Select(x => x.Name));
            var values = string.Join(", ", properties.Select(x => $"@{x.Name}"));


            return $"({columns}) values ({values})";
        }

        public static string GetUpdateColumnsAndValues(IEnumerable<PropertyInfo> properties)
        {
            return string.Join(", ", properties.Select(x => $"{x.Name} = @{x.Name}"));
        }

        private static IEnumerable<PropertyInfo> GetNonKeyProperties(this IReadModel readModel)
        {
            return readModel.GetType().GetProperties().Where(x => x.GetCustomAttribute<KeyAttribute>() == null);
        }
    }
}