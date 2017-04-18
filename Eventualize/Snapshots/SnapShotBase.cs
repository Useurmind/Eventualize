using System;
using System.Linq;

using Eventualize.Interfaces;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Snapshots;

namespace Eventualize.Snapshots
{
    public abstract class SnapShotBase : ISnapShot
    {
        public SnapShotBase( )
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public AggregateVersion Version { get; set; }
    }
}