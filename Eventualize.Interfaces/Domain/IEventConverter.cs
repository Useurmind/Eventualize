using System;
using System.Linq;

namespace Eventualize.Interfaces.Domain
{
    public interface IEventConverter
    {
        IEventData DeserializeEventData(string eventTypeName, Guid id, byte[] data);

        byte[] SerializeEventData(IEventData eventData);
    }
}