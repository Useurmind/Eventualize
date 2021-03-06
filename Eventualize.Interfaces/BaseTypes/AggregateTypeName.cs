using System.Linq;

namespace Eventualize.Interfaces.BaseTypes
{
    public struct AggregateTypeName
    {
        public AggregateTypeName(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return this.Value;
        }

        public static bool operator ==(AggregateTypeName obj1, AggregateTypeName obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(AggregateTypeName obj1, AggregateTypeName obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}