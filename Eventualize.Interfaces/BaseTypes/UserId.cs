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
    }
}