namespace System
{
    public struct Nullable<T> where T : struct
    {
        internal T _value;

        public T Value
        {
            get
            {
                if (!HasValue)
                    throw null;

                return _value;
            }
        }

        private readonly bool _hasValue;
        public bool HasValue { get; }
    }
}