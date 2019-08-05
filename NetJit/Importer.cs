using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetJit.Representations;
using static NetJit.Representations.OpCodesDef;

namespace NetJit
{
    public partial class Compiler
    {
        private BasicBlock _first;
        private InstructionReader _instructionReader;

        public void CreateBasicBlocks()
        {
            List<int> boundaries = MarkBasicBlockBoundaries();

            var prevPosition = 0;

            var cur = new BasicBlock(previous: null, next: null, _ilMemory.Slice(prevPosition, boundaries[0]));
            _first = cur;
            for (var i = 1; i < boundaries.Count; i++)
            {
                cur = new BasicBlock(previous: cur, next: null, _ilMemory.Slice(prevPosition, boundaries[i]));
                prevPosition = boundaries[i];
            }
        }

        private List<int> MarkBasicBlockBoundaries()
        {
            var boundaries = new List<int>();
            // We mark the boundaries where non-exceptional control flow occurs, and divide the blocks there

            while (true)
            {
                Instruction instr = _instructionReader.ReadInstruction();
                if (IsBasicBlockBoundary(instr.OpCode))
                {
                    boundaries.Add(_instructionReader.Position);
                    if (instr.OpCode.IsBranch)
                    {
                        //_instructionReader.FollowBranch(instr);
                    }
                    else if (instr.OpCode.IsEndOfMethod)
                    {
                        break;
                    }
                }
            }

            return boundaries;
        }
    }
}