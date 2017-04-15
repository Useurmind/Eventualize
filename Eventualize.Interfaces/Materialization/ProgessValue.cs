using System.Linq;

namespace Eventualize.Interfaces.Materialization
{
    /// <summary>
    /// This type represents the progess of the materialization process.
    /// It holds values that are only understandable in the context of the event store where the events are stored.
    /// The progess value states which event from the event store was the last successfully handled event in
    /// the materialization process.
    /// </summary>
    public struct ProgessValue
    {
        public ProgessValue(object value)
        {
            this.Value = value;
        }

        public object Value { get; }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}