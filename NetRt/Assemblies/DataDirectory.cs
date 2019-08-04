using System;

namespace NetRt.Assemblies
{
    using Rva = UInt32;

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