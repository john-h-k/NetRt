namespace System
{
    public readonly struct Int64
    {
        public const long MaxValue = 9223372036854775807L;
        public const long MinValue = -9223372036854775808L;

        private readonly long _value;
        public override int GetHashCode()
        {
            return (int)(this ^ (this << 32));
        }
    }
}