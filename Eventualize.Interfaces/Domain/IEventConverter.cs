using System;
using System.Linq;

using Eventualize.Interfaces.BaseTypes;

namespace Eventualize.Interfaces.Domain
{
    public interface IEventConverter
    {
        IEventData DeserializeEventData(BoundedContextName boundedContextName, EventTypeName eventTypeName, Guid id, byte[] data);

        byte[] SerializeEventData(IEventData eventData);
    }
}