namespace System
{
    public readonly struct Int16
    {
        private readonly short _value;

        public override int GetHashCode()
        {
            return this;
        }
    }
}