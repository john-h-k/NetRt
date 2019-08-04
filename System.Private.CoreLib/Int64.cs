namespace System
{
    public readonly struct Int64
    {
        private readonly long _value;
        public override int GetHashCode()
        {
            return (int)(this ^ (this << 32));
        }
    }
}