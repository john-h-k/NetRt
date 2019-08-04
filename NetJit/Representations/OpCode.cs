using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace NetJit.Representations
{
    public unsafe struct OpCode
    {
        public static OpCode Create(ref byte first)
        {
            OpCode opCode = default;
            if (first == 0xFE)
            {
                opCode.Bytes[0] = first;
                opCode.Bytes[1] = Unsafe.Add(ref first, 1);
            }
        }

        public ushort Instruction { get; set; }

#if DEBUG
        public string Alias { get; set; }
#endif
        public PopBehaviour PopBehaviour { get; set; }
        public PushBehaviour PushBehaviour { get; set; }

    }
}