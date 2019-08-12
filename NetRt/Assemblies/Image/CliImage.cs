using System;
using NetRt.Assemblies.Heaps;

namespace NetRt.Assemblies.Image
{

#nullable enable
    public sealed partial class CliImage
    {
        private CliImage(string name)
        {
            Name = name;
        }


        // All these fields are inited by CliImageReader
        public string Name { get; }

        public ushort NumSections { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public Characteristics Characteristics { get; private set; }
        public Subsystem Subsystem { get; private set; }
        public DllFlags DllFlags { get; private set; }
        public DataDirectory BaseRelocationTable { get; private set; }
        public DataDirectory Cli { get; private set; }
        public Section[] Sections { get; private set; } = default!;
        public Version RuntimeVersion { get; private set; } = default!;
        public DataDirectory Metadata { get; private set; }
        public RuntimeFlags Flags { get; private set; }
        public uint EntryPointToken { get; private set; }
        public DataDirectory Resources { get; private set; }
        public DataDirectory StrongNameSignature { get; private set; }
        public string MetadataVersion { get; private set; } = default!;
        public Section MetadataSection { get; private set; } = default!;
        public TableHeap TableHeap { get; private set; } = default!;
        public StringHeap StringHeap { get; private set; } = default!;
        public BlobHeap BlobHeap { get; private set; } = default!;
        public GuidHeap GuidHeap { get; private set; } = default!;
        public UserStringHeap UserStringHeap { get; private set; } = default!;
        public ModuleType ModuleType { get; private set; }
        public uint TableHeapOffset { get; private set; }

        private readonly int[] _codedIndexSizes = new int[14];
    }
}