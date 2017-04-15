using System;
using System.Linq;

namespace Eventualize.Interfaces.Snapshots
{
    public interface ISnapShot
    {
        Guid Id { get; set; }

        long Version { get; set; }
    }
}