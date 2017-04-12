using System.Linq;

namespace Eventualize.Domain
{
    public struct EventNamespace
    {
        public EventNamespace(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}