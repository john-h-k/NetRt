using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NetRt
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args is null || args.Length == 0)
                throw new ArgumentException(
                    "At least 1 argument (the file name) must be provided"); // TODO

            Logger.DebugLog($"Executing exe {args[0]}");


        }
    }
}
