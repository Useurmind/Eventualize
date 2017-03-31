using System;
using System.Linq;

namespace Eventualize.Domain
{
    public interface IMemento
    {
        Guid Id { get; set; }

        int Version { get; set; }
    }
}