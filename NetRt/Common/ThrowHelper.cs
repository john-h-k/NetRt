using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

#nullable enable

namespace NetRt.Common
{
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    internal static class ThrowHelper
    {
        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentException(string paramName, Exception inner) => throw new ArgumentException(paramName, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentException(string paramName, string message) => throw new ArgumentException(paramName, message);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentException(string paramName) => throw new ArgumentException(paramName);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string paramName, Exception inner) => throw new ArgumentNullException(paramName, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string paramName, string message) => throw new ArgumentNullException(paramName, message);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string paramName) => throw new ArgumentNullException(paramName);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string paramName, Exception inner) => throw new ArgumentOutOfRangeException(paramName, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string paramName, string message) => throw new ArgumentOutOfRangeException(paramName, message);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArgumentOutOfRangeException(string paramName) => throw new ArgumentOutOfRangeException(paramName);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowArrayTypeMismatchException(string message, Exception? inner = null) => throw new ArrayTypeMismatchException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowInvalidOperationException(string message, Exception? inner = null) => throw new InvalidOperationException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowDivideByZeroException(string message, Exception? inner = null) => throw new DivideByZeroException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowNotFiniteNumberException(string message, Exception? inner = null) => throw new NotFiniteNumberException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowOverflowException(string message, Exception? inner = null) => throw new OverflowException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowInvalidCastException(string message, Exception? inner = null) => throw new InvalidCastException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowObjectDisposedException(string objectName, Exception inner) => throw new ObjectDisposedException(objectName, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowObjectDisposedException(string objectName, string message) => throw new ObjectDisposedException(objectName, message);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowObjectDisposedException(string objectName) => throw new ObjectDisposedException(objectName);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowRankException(string message, Exception? inner = null) => throw new RankException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowTimeoutException(string message, Exception? inner = null) => throw new TimeoutException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowPlatformNotSupportedException(string message, Exception? inner = null) => throw new PlatformNotSupportedException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowKeyNotFoundException(string message, Exception? inner = null) => throw new KeyNotFoundException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowNotSupportedException(string message, Exception? inner = null) => throw new NotSupportedException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowOutOfMemoryException(string message, Exception? inner = null) => throw new OutOfMemoryException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowInsufficientMemoryException(string message, Exception? inner = null) => throw new InsufficientMemoryException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowWin32Exception(string message, Exception? inner = null) => throw new Win32Exception(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowExternalException(string message, Exception? inner = null) => throw new ExternalException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowBadImageFormatException(string message, Exception? inner = null) => throw new BadImageFormatException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowNullReferenceException(string message, Exception? inner = null) => throw new NullReferenceException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowIndexOutOfRangeException(string message, Exception? inner = null) => throw new IndexOutOfRangeException(message, inner);

        [DebuggerHidden]
        [DoesNotReturn]
        public static void ThrowNotImplementedException(string? message = null, Exception? inner = null) => throw new NotImplementedException(message, inner);
    }
}