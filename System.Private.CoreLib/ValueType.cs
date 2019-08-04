#nullable enable

namespace System
{
    public abstract class ValueType
    {
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            Type thisType = GetType();
            Type otherType = obj.GetType();

            if (thisType != otherType) return false;

            // TODO write once Type has proper reflection, fast path if bitwise comparable

            throw null;
        }

        public override int GetHashCode()
        {
            throw null;
        }
    }
}