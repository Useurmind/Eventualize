using System;
using System.Linq;

namespace Eventualize.Interfaces.Materialization
{
    public interface IProjectionModel
    {
        long LastEventNumber { get; set; }

        DateTime LastEventDate { get; set; }

        string LastModifierId { get; set; }
    }

    public interface IReadModel : IProjectionModel
    {
        long Version { get; set; }
    }
}
