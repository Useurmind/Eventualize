using System;
using System.Linq;

namespace Eventualize.Persistence
{
    public interface ISerializer
    {
        byte[] Serialize(object instance);

        object Deserialize(Type objectType, byte[] data);
    }
}