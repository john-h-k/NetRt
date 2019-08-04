namespace System
{
    public readonly struct Int32
    {
        private readonly int _value;

        public override int GetHashCode()
        {
            return this;
        }
    }
}