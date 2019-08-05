using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using NetJit.Representations;
using static NetJit.Representations.OpCodesDef;

namespace NetJit
{
    public partial class Compiler
    {
        private BasicBlock _first;
        private InstructionReader _instructionReader = new InstructionReader();

        public void CreateBasicBlocks()
        {
            // We mark the boundaries where non-exceptional control flow occurs, and divide the blocks there
            Instruction instr = _instructionReader.ReadInstruction();
            if (IsBasicBlockBoundary(instr.OpCode))
            {

            }
        }

    }
}