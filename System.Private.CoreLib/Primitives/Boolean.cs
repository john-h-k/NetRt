namespace System
{
    public readonly struct Boolean
    {
        private readonly bool _value;

        public override int GetHashCode()
        {
            return this ? 1 : 0;
        }
    }
}