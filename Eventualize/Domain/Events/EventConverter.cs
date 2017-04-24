using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Eventualize.Domain.Aggregates;
using Eventualize.Infrastructure;
using Eventualize.Interfaces.BaseTypes;
using Eventualize.Interfaces.Domain;
using Eventualize.Interfaces.Domain.MetaModel;
using Eventualize.Interfaces.Infrastructure;
using Eventualize.Persistence;

namespace Eventualize.Domain.Events
{
    public class EventConverter : IEventConverter
    {
        private ISerializer serializer;

        private IDomainMetaModel domainMetaModel;

        public EventConverter(ISerializer serializer, IDomainMetaModel domainMetaModel)
        {
            this.domainMetaModel = domainMetaModel;
            this.serializer = serializer;
        }

        public IEventData DeserializeEventData(BoundedContextName boundedContextName, EventTypeName eventTypeName, Guid id, byte[] data)
        {
            var eventMetaModel = this.domainMetaModel.GetBoundedContext(boundedContextName).GetEventType(eventTypeName);
            
            return (IEventData)this.serializer.Deserialize(eventMetaModel.ModelType, data);
        }

        public byte[] SerializeEventData(IEventData eventData)
        {
            return this.serializer.Serialize(eventData);
        }
    }
}