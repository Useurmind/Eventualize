using System.Linq;

namespace Eventualize.Interfaces.BaseTypes
{
    public struct EventType
    {
        public EventType(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return this.Value;
        }
    }
}