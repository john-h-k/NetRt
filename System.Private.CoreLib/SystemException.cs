namespace System
{
#nullable enable

    public class SystemException : Exception
    {
        public SystemException() : this(string.Empty)
        {
        }
        public SystemException(string message) : this(message, innerException: null)
        {
        }
        public SystemException(string message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}