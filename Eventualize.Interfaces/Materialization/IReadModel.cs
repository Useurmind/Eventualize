using System;
using System.Linq;

namespace Eventualize.Interfaces.Materialization
{
    public interface IReadModel
    {
        long LastEventNumber { get; set; }

        DateTime LastEventDate { get; set; }

        string LastModifierId { get; set; }
    }
}
