using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using Eventualize.Domain;
using Eventualize.Domain.Core;

namespace Eventualize.Persistence
{
    public class EventConverter : IEventConverter
    {
        private TypeRegister eventTypeRegister;
        private ISerializer serializer;

        public EventConverter(ISerializer serializer)
        {
            this.eventTypeRegister = new TypeRegister();
            this.serializer = serializer;
        }

        public void ScanEventTypes(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                this.ScanEventTypes(assembly);
            }
        }

        public void ScanEventTypes(Assembly assembly)
        {
            this.eventTypeRegister.ScanTypes(assembly, t => t.GetCustomAttribute<EventTypeNameAttribute>() != null, t => t.GetCustomAttribute<EventTypeNameAttribute>().Name);
        }

        public IEventData DeserializeEventData(string eventTypeName, Guid id, byte[] data)
        {
            Type eventType = this.eventTypeRegister.GetType(eventTypeName, () => $"Could not find type for event {eventTypeName} with id {id}");
            return (IEventData)this.serializer.Deserialize(eventType, data);
        }

        public byte[] SerializeEventData(IEventData eventData)
        {
            return this.serializer.Serialize(eventData);
        }
    }
}