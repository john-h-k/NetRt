namespace System
{
#nullable enable
    public class PlatformNotSupportedException : NotSupportedException
    {
        public PlatformNotSupportedException() : this(string.Empty)
        {
        }
        public PlatformNotSupportedException(string message) : this(message, null)
        {
        }
        public PlatformNotSupportedException(string message, Exception? inner) : base(message, inner)
        {
        }
    }
}