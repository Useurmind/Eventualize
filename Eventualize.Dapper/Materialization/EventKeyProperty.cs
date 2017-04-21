using System.Linq;
using System.Reflection;

namespace Eventualize.Dapper.Materialization
{
    public class EventKeyProperty
    {
        public PropertyInfo Property { get; set; }

        public string ParameterName { get; set; }
    }
}