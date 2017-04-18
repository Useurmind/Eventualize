using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Snapshots
{
    public interface ISnapShot
    {
        Guid Id { get; set; }

        AggregateVersion Version { get; set; }
    }
}