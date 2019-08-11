namespace System
{
#nullable enable

    public class ArgumentOutOfRangeException : ArgumentException
    {
        public ArgumentOutOfRangeException() : this(string.Empty)
        {

        }
        public ArgumentOutOfRangeException(string message) : this(message, innerException: null)
        {
        }
        public ArgumentOutOfRangeException(string message, string paramName) : this(message, paramName, innerException: null)
        {
        }
        public ArgumentOutOfRangeException(string message, Exception? innerException) : this(message, paramName: string.Empty, innerException)
        {
        }
        public ArgumentOutOfRangeException(string message, string paramName, Exception? innerException) : base(message, paramName, innerException)
        {
        }

        public ArgumentOutOfRangeException(string paramName, object actualValue, string message) : this(message, paramName)
        {
            ActualValue = actualValue;
        }

        public virtual object? ActualValue { get; }
    }
}