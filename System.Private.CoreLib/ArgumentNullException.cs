namespace System
{
#nullable enable

    public class ArgumentNullException : ArgumentException
    {
        public ArgumentNullException() : this(string.Empty)
        {
        }
        public ArgumentNullException(string message) : this(message, innerException: null)
        {
        }
        public ArgumentNullException(string message, string paramName) : this(message, paramName, innerException: null)
        {
        }
        public ArgumentNullException(string message, Exception? innerException) : this(message, string.Empty, innerException)
        {
        }
        public ArgumentNullException(string message, string paramName, Exception? innerException) : base(message, paramName, innerException)
        {
        }
    }
}