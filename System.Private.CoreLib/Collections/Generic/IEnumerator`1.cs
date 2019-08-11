namespace System.Collections.Generic
{
    public interface IEnumerator<out T> : IEnumerator
    {
        object IEnumerator.Current => Current;
        new T Current { get; }
    }
}