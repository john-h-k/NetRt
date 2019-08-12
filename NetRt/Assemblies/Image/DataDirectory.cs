using System;
using System.IO;
using NetRt.Common;

namespace NetRt.Assemblies.Image
{
    using Rva = UInt32;

    public static class DataDirectoryExtensions
    {
        public static DataDirectory ReadDataDirectory(this Stream stream)
        {
            return new DataDirectory(stream.Read<uint>(), stream.Read<int>());
        }
    }
    public readonly struct DataDirectory
    {
        public DataDirectory(Rva rva, int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            Rva = rva;
            Size = size;
        }

        public Rva Rva { get; }
        public int Size { get; }
    }
}