using System.Linq;

namespace Eventualize.Interfaces.BaseTypes
{
    public struct BoundedContextName
    {
        public BoundedContextName(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public override string ToString()
        {
            return this.Value;
        }

        public static bool operator ==(BoundedContextName obj1, BoundedContextName obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(BoundedContextName obj1, BoundedContextName obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}