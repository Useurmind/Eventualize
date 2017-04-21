using System.Collections.Generic;
using System.Linq;

namespace Eventualize.Dapper.Materialization
{
    public class KeyComparer
    {
        public string KeyCompareClause { get; set; }

        public IList<EventKeyProperty> EventKeyProperties { get; set; }

        public KeyComparer()
        {
            this.EventKeyProperties = new List<EventKeyProperty>();
        }
    }
}