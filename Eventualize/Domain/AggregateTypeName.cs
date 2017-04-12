using System.Linq;

namespace Eventualize.Domain
{
    public struct AggregateTypeName
    {
        public AggregateTypeName(string value)
        {
            this.Value = value;
        }

        public string Value { get; }
    }
}