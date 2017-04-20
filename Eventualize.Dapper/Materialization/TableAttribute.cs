using System;
using System.Linq;

namespace Eventualize.Dapper.Materialization
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class TableAttribute : Attribute
    {
        public TableAttribute(string schemaName, string tableName)
        {
            this.SchemaName = schemaName;
            this.TableName = tableName;
        }

        public string TableName { get; }

        public string SchemaName { get; }

        public string FullQualifiedName
        {
            get
            {
                return $"[{this.SchemaName}].[{this.TableName}]";
            }
        }
    }
}