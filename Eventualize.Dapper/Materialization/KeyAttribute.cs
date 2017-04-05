using System;
using System.Linq;

namespace Eventualize.Dapper.Materialization
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class KeyAttribute : Attribute
    {
        // This is a positional argument
        public KeyAttribute()
        {
        }
    }
}