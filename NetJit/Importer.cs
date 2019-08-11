using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
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
            boundaries.Sort();

            var prevPosition = 0;

            var cur = new BasicBlock(previous: null, next: null, InstructionReader.ReadAllInstructions(_ilMemory), prevPosition, boundaries[0] - prevPosition);
            prevPosition = boundaries[0];
            _first = cur;
            for (var i = 1; i < boundaries.Count; i++)
            {
                cur = new BasicBlock(previous: cur, next: null, InstructionReader.ReadAllInstructions(_ilMemory), prevPosition, boundaries[i] - prevPosition);
                prevPosition = boundaries[i];
            }

            _bbCount = boundaries.Count;
        }

        private List<int> MarkBasicBlockBoundaries()
        {
            static void AddNoDuplicate(List<int> l, int i)
            {
                if (!l.Contains(i)) l.Add(i);
            }

            var boundaries = new List<int>();

            // We mark the boundaries where non-exceptional control flow occurs, and divide the blocks there

            foreach (Instruction instr in _instructionReader)
            {
                if (IsBasicBlockBoundary(instr.OpCode))
                {
                    AddNoDuplicate(boundaries, instr.Position + instr.FullSize);

                    if (instr.OpCode.IsBranch)
                    {
                        int target = instr.ReadBranchTarget();
                        AddNoDuplicate(boundaries, instr.Position + instr.FullSize + target);
                    }
                }
            }

            return boundaries;
        }

        private void CreateExpressionTrees()
        {
            foreach (BasicBlock basicBlock in _first)
            {
                CreateTreeForBlock(basicBlock);
            }
        } 

        private void CreateTreeForBlock(BasicBlock basicBlock)
        {
            // lets just hope the IL is invalid, in which case this is correct
            throw new InvalidProgramException("Invalid IL detected during import");
        }
    }
}