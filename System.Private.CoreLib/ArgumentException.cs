namespace System
{
#nullable enable


    public class ArgumentException : SystemException
    {
        public ArgumentException() : this(string.Empty)
        {

        }
        public ArgumentException(string message) : this(message, innerException: null)
        {
        }
        public ArgumentException(string message, string paramName) : this(message, paramName, innerException: null)
        {
        }
        public ArgumentException(string message, Exception? innerException) : this(message, paramName: string.Empty, innerException)
        {
        }
        public ArgumentException(string message, string paramName, Exception? innerException) : base(message, innerException)
        {
            ParamName = paramName;
        }

        public virtual string ParamName { get; }
    }
}