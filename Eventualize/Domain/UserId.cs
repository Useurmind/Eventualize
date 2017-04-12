using System.Linq;

namespace Eventualize.Domain
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