using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Eventualize.Domain;

namespace Eventualize.Persistence.Snapshots
{
    public interface ISnapShotStore
    {
        IMemento GetSnapshot(AggregateIdentity aggregateIdentity);

        void SaveSnapshot(AggregateIdentity aggregateIdentity, IMemento memento);
    }
}
