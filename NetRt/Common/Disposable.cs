using System;

namespace NetRt.Common
{
    public class Disposable<T> : IDisposable where T : class, IDisposable
    {
        public Disposable(T value, bool owned)
        {
            Value = value;
            Owned = owned;
        }

        public T Value { get; }
        public bool Owned { get; private set; }

        public void Dispose()
        {
            if (Owned)
            {
                Value?.Dispose();
                Owned = false;
            }
        }
    }
}