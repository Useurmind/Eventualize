using System.Linq;

namespace Eventualize.Interfaces.BaseTypes
{
    public struct EventTypeName
    {
        public EventTypeName(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return this.Value;
        }

        public static bool operator ==(EventTypeName obj1, EventTypeName obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(EventTypeName obj1, EventTypeName obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}