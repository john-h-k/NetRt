namespace System
{
#nullable enable
    public class IndexOutOfRangeException : SystemException
    {
        public IndexOutOfRangeException() : this(string.Empty)
        {
        }
        public IndexOutOfRangeException(string message) : this(message, null)
        {
        }
        public IndexOutOfRangeException(string message, Exception? inner) : base(message, inner)
        {
        }
    }
}