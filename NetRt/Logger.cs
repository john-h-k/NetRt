using System;
using System.Diagnostics;

namespace NetRt
{
    internal static class Logger
    {
        [Conditional("DEBUG")]
        public static void DebugLog(string message)
            => Debug.WriteLine(message);

        [Conditional("TRACE")]
        public static void TraceLog(string message)
            => Trace.WriteLine(message);

        public static void Log(string message)
            => Console.WriteLine(message);
    }
}