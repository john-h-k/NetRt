namespace System
{
#nullable enable
    public class NotSupportedException : SystemException
    {
        public NotSupportedException() : this(string.Empty)
        {
        }
        public NotSupportedException(string message) : this(message, null)
        {
        }
        public NotSupportedException(string message, Exception? inner) : base(message, inner)
        {
        }
    }
}