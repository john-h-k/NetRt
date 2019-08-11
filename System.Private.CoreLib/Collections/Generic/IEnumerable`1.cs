namespace System.Collections.Generic
{
    public interface IEnumerable<out T> : IEnumerable
    {
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        new IEnumerator<T> GetEnumerator();
    }
}