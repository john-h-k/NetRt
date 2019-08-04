namespace System
{
    public readonly struct UInt64
    {
        private readonly ulong _value;

        public override int GetHashCode()
        {
            return (int)(this ^ (this << 32));
        }
    }
}