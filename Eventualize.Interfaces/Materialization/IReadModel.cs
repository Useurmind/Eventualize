using System;
using System.Linq;

namespace Eventualize.Interfaces.Materialization
{
    public interface IReadModel
    {
        long Version { get; set; }

        long LastEventNumber { get; set; }

        DateTime LastEventDate { get; set; }

        string LastModifierId { get; set; }
    }
}
