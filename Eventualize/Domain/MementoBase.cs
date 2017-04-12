using System;
using System.Linq;

namespace Eventualize.Domain
{
    public abstract class MementoBase : IMemento
    {
        public MementoBase( )
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public long Version { get; set; }
    }
}