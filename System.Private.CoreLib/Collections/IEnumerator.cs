namespace System.Collections
{
    public interface IEnumerator : IDisposable
    {
        bool MoveNext();
        object Current { get; }
        void Reset();

        void IDisposable.Dispose() { }
    }
}