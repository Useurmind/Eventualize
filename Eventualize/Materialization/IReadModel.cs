using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventualize.Materialization
{
    public interface IReadModel
    {
        long LastEventStoreIndex { get; set; }

        DateTime LastEventDate { get; set; }

        string LastModifierId { get; set; }
    }
}
