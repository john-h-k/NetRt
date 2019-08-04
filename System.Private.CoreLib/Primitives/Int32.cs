namespace System
{
    public readonly struct Int32
    {
        public const int MaxValue = 2147483647;
        public const int MinValue = -2147483648;

        private readonly int _value;

        public override int GetHashCode()
        {
            return this;
        }
    }
}