using System.Diagnostics.CodeAnalysis;

namespace System
{
    internal static class ThrowHelper
    {
        [DoesNotReturn]
        public static void ThrowArgumentException(string message, string paramName = null, Exception inner = null)
        {
            throw new ArgumentException(message, paramName, inner);
        }

        [DoesNotReturn]
        public static void ThrowArgumentNullException(string message, string paramName = null, Exception inner = null)
        {
            throw new ArgumentNullException(message, paramName, inner);
        }

        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string message, string paramName = null, Exception inner = null)
        {
            throw new ArgumentOutOfRangeException(message, paramName, inner);
        }

        [DoesNotReturn]
        public static void ThrowIndexOutOfRangeException(string message, Exception inner = null)
        {
            throw new IndexOutOfRangeException(message, inner);
        }

        [DoesNotReturn]
        public static void ThrowNotSupportedException(string message, Exception inner = null)
        {
            throw new NotSupportedException(message, inner);
        }
    }
}