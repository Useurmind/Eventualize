using System.Linq;

namespace Eventualize.Interfaces.BaseTypes
{
    public struct BoundedContext
    {
        public BoundedContext(string value)
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