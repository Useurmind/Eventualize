using System.Linq;

namespace Eventualize.Interfaces.BaseTypes
{
    public struct UserId
    {
        public UserId(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        public static bool operator ==(UserId obj1, UserId obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(UserId obj1, UserId obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}