using System;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Persistence
{
    public interface IEventConverter
    {
        IEventData DeserializeEventData(string eventTypeName, Guid id, byte[] data);

        byte[] SerializeEventData(IEventData eventData);
    }
}