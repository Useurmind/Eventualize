using System.Linq;

namespace Eventualize.Interfaces.BaseTypes
{
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
    }
}