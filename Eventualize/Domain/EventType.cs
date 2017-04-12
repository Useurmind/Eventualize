using System.Linq;

namespace Eventualize.Domain
{
    public struct EventType
    {
        public EventType(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}