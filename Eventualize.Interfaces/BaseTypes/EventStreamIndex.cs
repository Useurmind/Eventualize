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
    }
}