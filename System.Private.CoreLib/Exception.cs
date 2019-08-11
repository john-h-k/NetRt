namespace System
{
#nullable enable

    public class Exception
    {
        public Exception() : this(string.Empty)
        {

        }
        public Exception(string message) : this(message, innerException: null)
        {
        }
        public Exception(string message, Exception? innerException)
        {
            Message = message;
            InnerException = innerException;
        }

        public virtual string Message { get; }
        public virtual Exception? InnerException { get; }
    }
}