using System;
using System.Linq;

using Eventualize.Domain;

namespace Eventualize.Materialization
{
    public interface IMaterializationEvent
    {
        long EventIndex { get; }

        DateTime EventTimeStamp { get; }

        AggregateIdentity AggregateIdentity { get; }

        IEventData EventData { get; }
    }

    public class MaterializationEvent : IMaterializationEvent
    {
        public MaterializationEvent(long eventIndex, DateTime eventTimeStamp, AggregateIdentity aggregateIdentity, IEventData eventData)
        {
            this.EventIndex = eventIndex;
            this.EventTimeStamp = eventTimeStamp;
            this.AggregateIdentity = aggregateIdentity;
            this.EventData = eventData;
        }

        public long EventIndex { get; }

        public DateTime EventTimeStamp { get; }

        public AggregateIdentity AggregateIdentity { get; }

        public IEventData EventData { get; }
    }

    public interface IMaterializationStrategy
    {
        void HandleEvent(IMaterializationEvent materializationEvent);
    }
}