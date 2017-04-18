using System.Linq;

namespace Eventualize.Interfaces.BaseTypes
{
    /// <summary>
    /// This struct represents a version number for an aggregate.
    /// </summary>
    public struct AggregateVersion
    {
        private const long LatestValue = -2;
        private const long NotCreatedValue = -1;
        private const long StartValue = 0;

        public AggregateVersion(long value)
        {
            this.Value = value;
        }

        public long Value { get; }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public static bool operator >=(AggregateVersion obj1, AggregateVersion obj2)
        {
            return obj1.Value >= obj2.Value;
        }

        public static bool operator <=(AggregateVersion obj1, AggregateVersion obj2)
        {
            return obj1.Value <= obj2.Value;
        }

        public static bool operator >(AggregateVersion obj1, AggregateVersion obj2)
        {
            return obj1.Value > obj2.Value;
        }

        public static bool operator <(AggregateVersion obj1, AggregateVersion obj2)
        {
            return obj1.Value < obj2.Value;
        }

        public static bool operator ==(AggregateVersion obj1, AggregateVersion obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(AggregateVersion obj1, AggregateVersion obj2)
        {
            return !obj1.Equals(obj2);
        }

        public static AggregateVersion operator +(AggregateVersion obj1, long value)
        {
            return new AggregateVersion(obj1.Value + value);
        }

        public static AggregateVersion operator -(AggregateVersion obj1, long value)
        {
            return new AggregateVersion(obj1.Value - value);
        }

        public static AggregateVersion operator -(AggregateVersion obj1, int value)
        {
            return new AggregateVersion(obj1.Value - value);
        }

        public static long operator -(AggregateVersion obj1, AggregateVersion obj2)
        {
            return obj1.Value - obj2.Value;
        }

        public static AggregateVersion Latest()
        {
            return new AggregateVersion(LatestValue);
        }

        public static AggregateVersion NotCreated()
        {
            return new AggregateVersion(NotCreatedValue);
        }

        public static AggregateVersion Start()
        {
            return new AggregateVersion(StartValue);
        }
    }
}