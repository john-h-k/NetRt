namespace System
{
    public readonly struct Char
    {
        private readonly char _value;
        public override int GetHashCode()
        {
            return this;
        }
    }
}