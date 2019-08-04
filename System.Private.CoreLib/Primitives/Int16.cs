namespace System
{
    public readonly struct Int16
    {
        public const short MaxValue = 32767;
        public const short MinValue = -32768;

        private readonly short _value;

        public override int GetHashCode()
        {
            return this;
        }
    }
}