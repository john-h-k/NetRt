using System;
using NetJit.Representations;

namespace NetJit
{
    public partial class Compiler
    {
        public BasicBlock CreateBasicBlocks()
        {

        }

        private static Span<byte> TryReadInstruction()
        {

        }

        // Used to verify the IL eventually ends. If it doesn't, the poor JIT will continue building basic blocks confusedly forever. #tragic
        // IL ends in a ret/throw/rethrow/jmp or infinite loop
        private Memory<byte> ScanIlForEnd(ref byte il)
        {

            while (true)
            {

            }
        }
    }
}