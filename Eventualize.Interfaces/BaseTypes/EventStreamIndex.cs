using System.Linq;

namespace Eventualize.Interfaces.BaseTypes
{
    /// <summary>
    /// States the number of the event in the stream.
    /// This number is zero based.
    /// </summary>
    public struct EventStreamIndex
    {
        public EventStreamIndex(long value)
        {
            this.Value = value;
        }

        public long Value { get; }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public static bool operator ==(EventStreamIndex obj1, EventStreamIndex obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(EventStreamIndex obj1, EventStreamIndex obj2)
        {
            return !obj1.Equals(obj2);
        }

        /// <summary>
        /// The event stream index corresponds to the aggregate version if the event is part of an aggregate.
        /// </summary>
        /// <returns></returns>
        public AggregateVersion ToAggregateVersion()
        {
            return new AggregateVersion(this.Value);
        }
    }
}