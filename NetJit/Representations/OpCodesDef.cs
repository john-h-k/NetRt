// Licensed to the.NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/*****************************************************************************
 **                                                                         **
 ** Opcode.def - COM+ Intrinsic Opcodes and Macros.                         **
 **                                                                         **
 ** This is the master table from which all opcode lists                    **
 ** are derived.  New instructions must be added to this                    **
 ** table and generators run to produce the lookup tables                   **
 ** used by the interpreter loop.                                           **
 **                                                                         **
 ** Stack Behaviour is describing the number of 4 byte                      **
 ** slots pushed and Poped.                                                 **
 **                                                                         **
 *****************************************************************************/

using System;
using System.Diagnostics.CodeAnalysis;
using static NetJit.Representations.PopBehaviour;
using static NetJit.Representations.PushBehaviour;
using static NetJit.Representations.OperandParams;
using static NetJit.Representations.OpCodeKind;
using static NetJit.Representations.ControlFlowKind;
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo

namespace NetJit.Representations
{
    [Flags]
    public enum PopBehaviour
    {
        Pop0,
        Pop1,
        Pop1_Pop1,
        PopI,
        PopI_Pop1,
        PopI4,
        PopI8,
        PopR4,
        PopR8,
        PopRef,
        VarPop,
        PopI_PopI,
        PopI_PopRef,
        PopI_PopR4,
        PopI_PopR8,
        PopI_PopI8,
        PopRef_PopI,
        PopRef_PopI_PopI,
        PopRef_PopI_PopR4,
        PopRef_PopI_PopR8,
        PopRef_PopI_PopRef,
        PopRef_PopI_PopI8,
        PopI_PopI_PopI,
        PopRef_Pop1,
        PopRef_PopI_Pop1
    }

    [Flags]
    public enum PushBehaviour
    {
        Push0,
        Push1,
        Push1_Push1,
        PushI,
        PushRef,
        PushI4,
        PushI8,
        PushR4,
        PushR8,
        VarPush,
    }

    public enum OperandParams
    {
        InlineNone = 0,
        ShortInlineVar = 1,
        InlineI = 2,
        ShortInlineI = 3,
        InlineI8 = 4,
        ShortInlineR = 5,
        InlineR = 6,
        InlineMethod = 7,
        InlineSig = 8,
        InlineSwitch = 9,
        InlineBrTarget = 10,
        ShortInlineBrTarget = 11,
        InlineVar = 12,
        InlineType = 13,
        InlineString = 14,
        InlineField = 15,
        InlineTok = 16
    }

    public enum OpCodeKind
    {
        IPrimitive = 0,
        IMacro = 1,
        IInternal = 2,
        IPrefix = 3,
        IObjModel = 4,
    }

    public enum ControlFlowKind
    {
        Next = 0,
        Branch = 1,
        ConditionalBranch = 2,
        Call = 3,
        Return = 4,
        Meta = 5,
        Throw = 6,
        Break = 7
    }

    // ReSharper restore InconsistentNaming

    public static class OpCodesDef
    {
        public static bool IsBasicBlockBoundary(OpCode op) => op.ControlFlowKind == Return || /* op.IsBranch */ op.IsUnconditionalBranch;

        public static int GetSizeForParamKind(OperandParams operandParams)
        {
            const int metadataTokenSz = 4;
            switch (operandParams)
            {
                case InlineNone:
                    return 0;
                case ShortInlineVar:
                    return 1;
                case InlineI:
                    return 4;
                case ShortInlineI:
                    return 2;
                case InlineI8:
                    return 8;
                case ShortInlineR:
                    return 4;
                case InlineR:
                    return 8;
                case InlineSwitch:
                    return 4;
                case InlineBrTarget:
                    return 4;
                case ShortInlineBrTarget:
                    return 1;
                case InlineVar:
                    return 2;
                case InlineSig:
                case InlineMethod:
                case InlineType:
                case InlineString:
                case InlineField:
                case InlineTok:
                    return metadataTokenSz;
                default:
                    throw new ArgumentOutOfRangeException(nameof(operandParams), operandParams, null);
            }
        }

        // If the first byte of the standard encoding is 0xFF, then
        // the second byte can be used as 1 byte encoding.  Otherwise                                                               l   b         b
        // the encoding is two bytes.                                                                                               e   y         y
        //                                                                                                                          n   t         t
        //                                                                                                                          g   e         e
        //                                                                                                           (unused);       t
        //  Canonical Name                    String Name              Stack Behaviour           Operand Params    Opcode Kind      h   1         2    Control Flow
        // -------------------------------------------------------------------------------------------------------------------------------------------------------
        public static readonly OpCode Nop = OpCode.Create("nop", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x00, Next);
        public static readonly OpCode Break = OpCode.Create("break", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x01, ControlFlowKind.Break);
        public static readonly OpCode Ldarg_0 = OpCode.Create("ldarg.0", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x02, Next);
        public static readonly OpCode Ldarg_1 = OpCode.Create("ldarg.1", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x03, Next);
        public static readonly OpCode Ldarg_2 = OpCode.Create("ldarg.2", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x04, Next);
        public static readonly OpCode Ldarg_3 = OpCode.Create("ldarg.3", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x05, Next);
        public static readonly OpCode Ldloc_0 = OpCode.Create("ldloc.0", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x06, Next);
        public static readonly OpCode Ldloc_1 = OpCode.Create("ldloc.1", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x07, Next);
        public static readonly OpCode Ldloc_2 = OpCode.Create("ldloc.2", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x08, Next);
        public static readonly OpCode Ldloc_3 = OpCode.Create("ldloc.3", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x09, Next);
        public static readonly OpCode Stloc_0 = OpCode.Create("stloc.0", Pop1, Push0, InlineNone, IMacro, 1, 0xFF, 0x0A, Next);
        public static readonly OpCode Stloc_1 = OpCode.Create("stloc.1", Pop1, Push0, InlineNone, IMacro, 1, 0xFF, 0x0B, Next);
        public static readonly OpCode Stloc_2 = OpCode.Create("stloc.2", Pop1, Push0, InlineNone, IMacro, 1, 0xFF, 0x0C, Next);
        public static readonly OpCode Stloc_3 = OpCode.Create("stloc.3", Pop1, Push0, InlineNone, IMacro, 1, 0xFF, 0x0D, Next);
        public static readonly OpCode Ldarg_S = OpCode.Create("ldarg.s", Pop0, Push1, ShortInlineVar, IMacro, 1, 0xFF, 0x0E, Next);
        public static readonly OpCode Ldarga_S = OpCode.Create("ldarga.s", Pop0, PushI, ShortInlineVar, IMacro, 1, 0xFF, 0x0F, Next);
        public static readonly OpCode Starg_S = OpCode.Create("starg.s", Pop1, Push0, ShortInlineVar, IMacro, 1, 0xFF, 0x10, Next);
        public static readonly OpCode Ldloc_S = OpCode.Create("ldloc.s", Pop0, Push1, ShortInlineVar, IMacro, 1, 0xFF, 0x11, Next);
        public static readonly OpCode Ldloca_S = OpCode.Create("ldloca.s", Pop0, PushI, ShortInlineVar, IMacro, 1, 0xFF, 0x12, Next);
        public static readonly OpCode Stloc_S = OpCode.Create("stloc.s", Pop1, Push0, ShortInlineVar, IMacro, 1, 0xFF, 0x13, Next);
        public static readonly OpCode Ldnull = OpCode.Create("ldnull", Pop0, PushRef, InlineNone, IPrimitive, 1, 0xFF, 0x14, Next);
        public static readonly OpCode Ldc_I4_M1 = OpCode.Create("ldc.i4.m1", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x15, Next);
        public static readonly OpCode Ldc_I4_0 = OpCode.Create("ldc.i4.0", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x16, Next);
        public static readonly OpCode Ldc_I4_1 = OpCode.Create("ldc.i4.1", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x17, Next);
        public static readonly OpCode Ldc_I4_2 = OpCode.Create("ldc.i4.2", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x18, Next);
        public static readonly OpCode Ldc_I4_3 = OpCode.Create("ldc.i4.3", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x19, Next);
        public static readonly OpCode Ldc_I4_4 = OpCode.Create("ldc.i4.4", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x1A, Next);
        public static readonly OpCode Ldc_I4_5 = OpCode.Create("ldc.i4.5", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x1B, Next);
        public static readonly OpCode Ldc_I4_6 = OpCode.Create("ldc.i4.6", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x1C, Next);
        public static readonly OpCode Ldc_I4_7 = OpCode.Create("ldc.i4.7", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x1D, Next);
        public static readonly OpCode Ldc_I4_8 = OpCode.Create("ldc.i4.8", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x1E, Next);
        public static readonly OpCode Ldc_I4_S = OpCode.Create("ldc.i4.s", Pop0, PushI, ShortInlineI, IMacro, 1, 0xFF, 0x1F, Next);
        public static readonly OpCode Ldc_I4 = OpCode.Create("ldc.i4", Pop0, PushI, InlineI, IPrimitive, 1, 0xFF, 0x20, Next);
        public static readonly OpCode Ldc_I8 = OpCode.Create("ldc.i8", Pop0, PushI8, InlineI8, IPrimitive, 1, 0xFF, 0x21, Next);
        public static readonly OpCode Ldc_R4 = OpCode.Create("ldc.r4", Pop0, PushR4, ShortInlineR, IPrimitive, 1, 0xFF, 0x22, Next);
        public static readonly OpCode Ldc_R8 = OpCode.Create("ldc.r8", Pop0, PushR8, InlineR, IPrimitive, 1, 0xFF, 0x23, Next);
        public static readonly OpCode Unused49 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x24, Next);
        public static readonly OpCode Dup = OpCode.Create("dup", Pop1, Push1_Push1, InlineNone, IPrimitive, 1, 0xFF, 0x25, Next);
        public static readonly OpCode Pop = OpCode.Create("pop", Pop1, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x26, Next);
        public static readonly OpCode Jmp = OpCode.Create("jmp", Pop0, Push0, InlineMethod, IPrimitive, 1, 0xFF, 0x27, ControlFlowKind.Call);
        public static readonly OpCode Call = OpCode.Create("call", VarPop, VarPush, InlineMethod, IPrimitive, 1, 0xFF, 0x28, ControlFlowKind.Call);
        public static readonly OpCode Calli = OpCode.Create("calli", VarPop, VarPush, InlineSig, IPrimitive, 1, 0xFF, 0x29, ControlFlowKind.Call);
        public static readonly OpCode Ret = OpCode.Create("ret", VarPop, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x2A, Return);
        public static readonly OpCode Br_S = OpCode.Create("br.s", Pop0, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x2B, Branch);
        public static readonly OpCode Brfalse_S = OpCode.Create("brfalse.s", PopI, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x2C, ConditionalBranch);
        public static readonly OpCode Brtrue_S = OpCode.Create("brtrue.s", PopI, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x2D, ConditionalBranch);
        public static readonly OpCode Beq_S = OpCode.Create("beq.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x2E, ConditionalBranch);
        public static readonly OpCode Bge_S = OpCode.Create("bge.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x2F, ConditionalBranch);
        public static readonly OpCode Bgt_S = OpCode.Create("bgt.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x30, ConditionalBranch);
        public static readonly OpCode Ble_S = OpCode.Create("ble.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x31, ConditionalBranch);
        public static readonly OpCode Blt_S = OpCode.Create("blt.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x32, ConditionalBranch);
        public static readonly OpCode Bne_Un_S = OpCode.Create("bne.un.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x33, ConditionalBranch);
        public static readonly OpCode Bge_Un_S = OpCode.Create("bge.un.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x34, ConditionalBranch);
        public static readonly OpCode Bgt_Un_S = OpCode.Create("bgt.un.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x35, ConditionalBranch);
        public static readonly OpCode Ble_Un_S = OpCode.Create("ble.un.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x36, ConditionalBranch);
        public static readonly OpCode Blt_Un_S = OpCode.Create("blt.un.s", Pop1_Pop1, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x37, ConditionalBranch);
        public static readonly OpCode Br = OpCode.Create("br", Pop0, Push0, InlineBrTarget, IPrimitive, 1, 0xFF, 0x38, Branch);
        public static readonly OpCode Brfalse = OpCode.Create("brfalse", PopI, Push0, InlineBrTarget, IPrimitive, 1, 0xFF, 0x39, ConditionalBranch);
        public static readonly OpCode Brtrue = OpCode.Create("brtrue", PopI, Push0, InlineBrTarget, IPrimitive, 1, 0xFF, 0x3A, ConditionalBranch);
        public static readonly OpCode Beq = OpCode.Create("beq", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x3B, ConditionalBranch);
        public static readonly OpCode Bge = OpCode.Create("bge", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x3C, ConditionalBranch);
        public static readonly OpCode Bgt = OpCode.Create("bgt", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x3D, ConditionalBranch);
        public static readonly OpCode Ble = OpCode.Create("ble", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x3E, ConditionalBranch);
        public static readonly OpCode Blt = OpCode.Create("blt", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x3F, ConditionalBranch);
        public static readonly OpCode Bne_Un = OpCode.Create("bne.un", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x40, ConditionalBranch);
        public static readonly OpCode Bge_Un = OpCode.Create("bge.un", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x41, ConditionalBranch);
        public static readonly OpCode Bgt_Un = OpCode.Create("bgt.un", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x42, ConditionalBranch);
        public static readonly OpCode Ble_Un = OpCode.Create("ble.un", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x43, ConditionalBranch);
        public static readonly OpCode Blt_Un = OpCode.Create("blt.un", Pop1_Pop1, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x44, ConditionalBranch);
        public static readonly OpCode Switch = OpCode.Create("switch", PopI, Push0, InlineSwitch, IPrimitive, 1, 0xFF, 0x45, ConditionalBranch);
        public static readonly OpCode Ldind_I1 = OpCode.Create("ldind.i1", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x46, Next);
        public static readonly OpCode Ldind_U1 = OpCode.Create("ldind.u1", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x47, Next);
        public static readonly OpCode Ldind_I2 = OpCode.Create("ldind.i2", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x48, Next);
        public static readonly OpCode Ldind_U2 = OpCode.Create("ldind.u2", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x49, Next);
        public static readonly OpCode Ldind_I4 = OpCode.Create("ldind.i4", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x4A, Next);
        public static readonly OpCode Ldind_U4 = OpCode.Create("ldind.u4", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x4B, Next);
        public static readonly OpCode Ldind_I8 = OpCode.Create("ldind.i8", PopI, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0x4C, Next);
        public static readonly OpCode Ldind_I = OpCode.Create("ldind.i", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x4D, Next);
        public static readonly OpCode Ldind_R4 = OpCode.Create("ldind.r4", PopI, PushR4, InlineNone, IPrimitive, 1, 0xFF, 0x4E, Next);
        public static readonly OpCode Ldind_R8 = OpCode.Create("ldind.r8", PopI, PushR8, InlineNone, IPrimitive, 1, 0xFF, 0x4F, Next);
        public static readonly OpCode Ldind_Ref = OpCode.Create("ldind.ref", PopI, PushRef, InlineNone, IPrimitive, 1, 0xFF, 0x50, Next);
        public static readonly OpCode Stind_Ref = OpCode.Create("stind.ref", PopI_PopI, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x51, Next);
        public static readonly OpCode Stind_I1 = OpCode.Create("stind.i1", PopI_PopI, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x52, Next);
        public static readonly OpCode Stind_I2 = OpCode.Create("stind.i2", PopI_PopI, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x53, Next);
        public static readonly OpCode Stind_I4 = OpCode.Create("stind.i4", PopI_PopI, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x54, Next);
        public static readonly OpCode Stind_I8 = OpCode.Create("stind.i8", PopI_PopI8, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x55, Next);
        public static readonly OpCode Stind_R4 = OpCode.Create("stind.r4", PopI_PopR4, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x56, Next);
        public static readonly OpCode Stind_R8 = OpCode.Create("stind.r8", PopI_PopR8, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x57, Next);
        public static readonly OpCode Add = OpCode.Create("add", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x58, Next);
        public static readonly OpCode Sub = OpCode.Create("sub", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x59, Next);
        public static readonly OpCode Mul = OpCode.Create("mul", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5A, Next);
        public static readonly OpCode Div = OpCode.Create("div", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5B, Next);
        public static readonly OpCode Div_Un = OpCode.Create("div.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5C, Next);
        public static readonly OpCode Rem = OpCode.Create("rem", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5D, Next);
        public static readonly OpCode Rem_Un = OpCode.Create("rem.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5E, Next);
        public static readonly OpCode And = OpCode.Create("and", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5F, Next);
        public static readonly OpCode Or = OpCode.Create("or", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x60, Next);
        public static readonly OpCode Xor = OpCode.Create("xor", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x61, Next);
        public static readonly OpCode Shl = OpCode.Create("shl", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x62, Next);
        public static readonly OpCode Shr = OpCode.Create("shr", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x63, Next);
        public static readonly OpCode Shr_Un = OpCode.Create("shr.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x64, Next);
        public static readonly OpCode Neg = OpCode.Create("neg", Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x65, Next);
        public static readonly OpCode Not = OpCode.Create("not", Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x66, Next);
        public static readonly OpCode Conv_I1 = OpCode.Create("conv.i1", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x67, Next);
        public static readonly OpCode Conv_I2 = OpCode.Create("conv.i2", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x68, Next);
        public static readonly OpCode Conv_I4 = OpCode.Create("conv.i4", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x69, Next);
        public static readonly OpCode Conv_I8 = OpCode.Create("conv.i8", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0x6A, Next);
        public static readonly OpCode Conv_R4 = OpCode.Create("conv.r4", Pop1, PushR4, InlineNone, IPrimitive, 1, 0xFF, 0x6B, Next);
        public static readonly OpCode Conv_R8 = OpCode.Create("conv.r8", Pop1, PushR8, InlineNone, IPrimitive, 1, 0xFF, 0x6C, Next);
        public static readonly OpCode Conv_U4 = OpCode.Create("conv.u4", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x6D, Next);
        public static readonly OpCode Conv_U8 = OpCode.Create("conv.u8", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0x6E, Next);
        public static readonly OpCode Callvirt = OpCode.Create("callvirt", VarPop, VarPush, InlineMethod, IObjModel, 1, 0xFF, 0x6F, ControlFlowKind.Call);
        public static readonly OpCode Cpobj = OpCode.Create("cpobj", PopI_PopI, Push0, InlineType, IObjModel, 1, 0xFF, 0x70, Next);
        public static readonly OpCode Ldobj = OpCode.Create("ldobj", PopI, Push1, InlineType, IObjModel, 1, 0xFF, 0x71, Next);
        public static readonly OpCode Ldstr = OpCode.Create("ldstr", Pop0, PushRef, InlineString, IObjModel, 1, 0xFF, 0x72, Next);
        public static readonly OpCode Newobj = OpCode.Create("newobj", VarPop, PushRef, InlineMethod, IObjModel, 1, 0xFF, 0x73, ControlFlowKind.Call);
        public static readonly OpCode Castclass = OpCode.Create("castclass", PopRef, PushRef, InlineType, IObjModel, 1, 0xFF, 0x74, Next);
        public static readonly OpCode Isinst = OpCode.Create("isinst", PopRef, PushI, InlineType, IObjModel, 1, 0xFF, 0x75, Next);
        public static readonly OpCode Conv_R_Un = OpCode.Create("conv.r.un", Pop1, PushR8, InlineNone, IPrimitive, 1, 0xFF, 0x76, Next);
        public static readonly OpCode Unused58 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x77, Next);
        public static readonly OpCode Unused1 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x78, Next);
        public static readonly OpCode Unbox = OpCode.Create("unbox", PopRef, PushI, InlineType, IPrimitive, 1, 0xFF, 0x79, Next);
        public static readonly OpCode Throw = OpCode.Create("throw", PopRef, Push0, InlineNone, IObjModel, 1, 0xFF, 0x7A, ControlFlowKind.Throw);
        public static readonly OpCode Ldfld = OpCode.Create("ldfld", PopRef, Push1, InlineField, IObjModel, 1, 0xFF, 0x7B, Next);
        public static readonly OpCode Ldflda = OpCode.Create("ldflda", PopRef, PushI, InlineField, IObjModel, 1, 0xFF, 0x7C, Next);
        public static readonly OpCode Stfld = OpCode.Create("stfld", PopRef_Pop1, Push0, InlineField, IObjModel, 1, 0xFF, 0x7D, Next);
        public static readonly OpCode Ldsfld = OpCode.Create("ldsfld", Pop0, Push1, InlineField, IObjModel, 1, 0xFF, 0x7E, Next);
        public static readonly OpCode Ldsflda = OpCode.Create("ldsflda", Pop0, PushI, InlineField, IObjModel, 1, 0xFF, 0x7F, Next);
        public static readonly OpCode Stsfld = OpCode.Create("stsfld", Pop1, Push0, InlineField, IObjModel, 1, 0xFF, 0x80, Next);
        public static readonly OpCode Stobj = OpCode.Create("stobj", PopI_Pop1, Push0, InlineType, IPrimitive, 1, 0xFF, 0x81, Next);
        public static readonly OpCode Conv_Ovf_I1_Un = OpCode.Create("conv.ovf.i1.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x82, Next);
        public static readonly OpCode Conv_Ovf_I2_Un = OpCode.Create("conv.ovf.i2.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x83, Next);
        public static readonly OpCode Conv_Ovf_I4_Un = OpCode.Create("conv.ovf.i4.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x84, Next);
        public static readonly OpCode Conv_Ovf_I8_Un = OpCode.Create("conv.ovf.i8.un", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0x85, Next);
        public static readonly OpCode Conv_Ovf_U1_Un = OpCode.Create("conv.ovf.u1.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x86, Next);
        public static readonly OpCode Conv_Ovf_U2_Un = OpCode.Create("conv.ovf.u2.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x87, Next);
        public static readonly OpCode Conv_Ovf_U4_Un = OpCode.Create("conv.ovf.u4.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x88, Next);
        public static readonly OpCode Conv_Ovf_U8_Un = OpCode.Create("conv.ovf.u8.un", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0x89, Next);
        public static readonly OpCode Conv_Ovf_I_Un = OpCode.Create("conv.ovf.i.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x8A, Next);
        public static readonly OpCode Conv_Ovf_U_Un = OpCode.Create("conv.ovf.u.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x8B, Next);
        public static readonly OpCode Box = OpCode.Create("box", Pop1, PushRef, InlineType, IPrimitive, 1, 0xFF, 0x8C, Next);
        public static readonly OpCode Newarr = OpCode.Create("newarr", PopI, PushRef, InlineType, IObjModel, 1, 0xFF, 0x8D, Next);
        public static readonly OpCode Ldlen = OpCode.Create("ldlen", PopRef, PushI, InlineNone, IObjModel, 1, 0xFF, 0x8E, Next);
        public static readonly OpCode Ldelema = OpCode.Create("ldelema", PopRef_PopI, PushI, InlineType, IObjModel, 1, 0xFF, 0x8F, Next);
        public static readonly OpCode Ldelem_I1 = OpCode.Create("ldelem.i1", PopRef_PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x90, Next);
        public static readonly OpCode Ldelem_U1 = OpCode.Create("ldelem.u1", PopRef_PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x91, Next);
        public static readonly OpCode Ldelem_I2 = OpCode.Create("ldelem.i2", PopRef_PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x92, Next);
        public static readonly OpCode Ldelem_U2 = OpCode.Create("ldelem.u2", PopRef_PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x93, Next);
        public static readonly OpCode Ldelem_I4 = OpCode.Create("ldelem.i4", PopRef_PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x94, Next);
        public static readonly OpCode Ldelem_U4 = OpCode.Create("ldelem.u4", PopRef_PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x95, Next);
        public static readonly OpCode Ldelem_I8 = OpCode.Create("ldelem.i8", PopRef_PopI, PushI8, InlineNone, IObjModel, 1, 0xFF, 0x96, Next);
        public static readonly OpCode Ldelem_I = OpCode.Create("ldelem.i", PopRef_PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x97, Next);
        public static readonly OpCode Ldelem_R4 = OpCode.Create("ldelem.r4", PopRef_PopI, PushR4, InlineNone, IObjModel, 1, 0xFF, 0x98, Next);
        public static readonly OpCode Ldelem_R8 = OpCode.Create("ldelem.r8", PopRef_PopI, PushR8, InlineNone, IObjModel, 1, 0xFF, 0x99, Next);
        public static readonly OpCode Ldelem_Ref = OpCode.Create("ldelem.ref", PopRef_PopI, PushRef, InlineNone, IObjModel, 1, 0xFF, 0x9A, Next);
        public static readonly OpCode Stelem_I = OpCode.Create("stelem.i", PopRef_PopI_PopI, Push0, InlineNone, IObjModel, 1, 0xFF, 0x9B, Next);
        public static readonly OpCode Stelem_I1 = OpCode.Create("stelem.i1", PopRef_PopI_PopI, Push0, InlineNone, IObjModel, 1, 0xFF, 0x9C, Next);
        public static readonly OpCode Stelem_I2 = OpCode.Create("stelem.i2", PopRef_PopI_PopI, Push0, InlineNone, IObjModel, 1, 0xFF, 0x9D, Next);
        public static readonly OpCode Stelem_I4 = OpCode.Create("stelem.i4", PopRef_PopI_PopI, Push0, InlineNone, IObjModel, 1, 0xFF, 0x9E, Next);
        public static readonly OpCode Stelem_I8 = OpCode.Create("stelem.i8", PopRef_PopI_PopI8, Push0, InlineNone, IObjModel, 1, 0xFF, 0x9F, Next);
        public static readonly OpCode Stelem_R4 = OpCode.Create("stelem.r4", PopRef_PopI_PopR4, Push0, InlineNone, IObjModel, 1, 0xFF, 0xA0, Next);
        public static readonly OpCode Stelem_R8 = OpCode.Create("stelem.r8", PopRef_PopI_PopR8, Push0, InlineNone, IObjModel, 1, 0xFF, 0xA1, Next);
        public static readonly OpCode Stelem_Ref = OpCode.Create("stelem.ref", PopRef_PopI_PopRef, Push0, InlineNone, IObjModel, 1, 0xFF, 0xA2, Next);
        public static readonly OpCode Ldelem = OpCode.Create("ldelem", PopRef_PopI, Push1, InlineType, IObjModel, 1, 0xFF, 0xA3, Next);
        public static readonly OpCode Stelem = OpCode.Create("stelem", PopRef_PopI_Pop1, Push0, InlineType, IObjModel, 1, 0xFF, 0xA4, Next);
        public static readonly OpCode Unbox_Any = OpCode.Create("unbox.any", PopRef, Push1, InlineType, IObjModel, 1, 0xFF, 0xA5, Next);
        public static readonly OpCode Unused5 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xA6, Next);
        public static readonly OpCode Unused6 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xA7, Next);
        public static readonly OpCode Unused7 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xA8, Next);
        public static readonly OpCode Unused8 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xA9, Next);
        public static readonly OpCode Unused9 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAA, Next);
        public static readonly OpCode Unused10 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAB, Next);
        public static readonly OpCode Unused11 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAC, Next);
        public static readonly OpCode Unused12 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAD, Next);
        public static readonly OpCode Unused13 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAE, Next);
        public static readonly OpCode Unused14 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAF, Next);
        public static readonly OpCode Unused15 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xB0, Next);
        public static readonly OpCode Unused16 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xB1, Next);
        public static readonly OpCode Unused17 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xB2, Next);
        public static readonly OpCode Conv_Ovf_I1 = OpCode.Create("conv.ovf.i1", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB3, Next);
        public static readonly OpCode Conv_Ovf_U1 = OpCode.Create("conv.ovf.u1", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB4, Next);
        public static readonly OpCode Conv_Ovf_I2 = OpCode.Create("conv.ovf.i2", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB5, Next);
        public static readonly OpCode Conv_Ovf_U2 = OpCode.Create("conv.ovf.u2", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB6, Next);
        public static readonly OpCode Conv_Ovf_I4 = OpCode.Create("conv.ovf.i4", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB7, Next);
        public static readonly OpCode Conv_Ovf_U4 = OpCode.Create("conv.ovf.u4", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB8, Next);
        public static readonly OpCode Conv_Ovf_I8 = OpCode.Create("conv.ovf.i8", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0xB9, Next);
        public static readonly OpCode Conv_Ovf_U8 = OpCode.Create("conv.ovf.u8", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0xBA, Next);
        public static readonly OpCode Unused50 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xBB, Next);
        public static readonly OpCode Unused18 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xBC, Next);
        public static readonly OpCode Unused19 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xBD, Next);
        public static readonly OpCode Unused20 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xBE, Next);
        public static readonly OpCode Unused21 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xBF, Next);
        public static readonly OpCode Unused22 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC0, Next);
        public static readonly OpCode Unused23 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC1, Next);
        public static readonly OpCode Refanyval = OpCode.Create("refanyval", Pop1, PushI, InlineType, IPrimitive, 1, 0xFF, 0xC2, Next);
        public static readonly OpCode Ckfinite = OpCode.Create("ckfinite", Pop1, PushR8, InlineNone, IPrimitive, 1, 0xFF, 0xC3, Next);
        public static readonly OpCode Unused24 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC4, Next);
        public static readonly OpCode Unused25 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC5, Next);
        public static readonly OpCode Mkrefany = OpCode.Create("mkrefany", PopI, Push1, InlineType, IPrimitive, 1, 0xFF, 0xC6, Next);
        public static readonly OpCode Unused59 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC7, Next);
        public static readonly OpCode Unused60 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC8, Next);
        public static readonly OpCode Unused61 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC9, Next);
        public static readonly OpCode Unused62 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCA, Next);
        public static readonly OpCode Unused63 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCB, Next);
        public static readonly OpCode Unused64 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCC, Next);
        public static readonly OpCode Unused65 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCD, Next);
        public static readonly OpCode Unused66 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCE, Next);
        public static readonly OpCode Unused67 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCF, Next);
        public static readonly OpCode Ldtoken = OpCode.Create("ldtoken", Pop0, PushI, InlineTok, IPrimitive, 1, 0xFF, 0xD0, Next);
        public static readonly OpCode Conv_U2 = OpCode.Create("conv.u2", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xD1, Next);
        public static readonly OpCode Conv_U1 = OpCode.Create("conv.u1", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xD2, Next);
        public static readonly OpCode Conv_I = OpCode.Create("conv.i", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xD3, Next);
        public static readonly OpCode Conv_Ovf_I = OpCode.Create("conv.ovf.i", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xD4, Next);
        public static readonly OpCode Conv_Ovf_U = OpCode.Create("conv.ovf.u", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xD5, Next);
        public static readonly OpCode Add_Ovf = OpCode.Create("add.ovf", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xD6, Next);
        public static readonly OpCode Add_Ovf_Un = OpCode.Create("add.ovf.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xD7, Next);
        public static readonly OpCode Mul_Ovf = OpCode.Create("mul.ovf", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xD8, Next);
        public static readonly OpCode Mul_Ovf_Un = OpCode.Create("mul.ovf.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xD9, Next);
        public static readonly OpCode Sub_Ovf = OpCode.Create("sub.ovf", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xDA, Next);
        public static readonly OpCode Sub_Ovf_Un = OpCode.Create("sub.ovf.un", Pop1_Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xDB, Next);
        public static readonly OpCode Endfinally = OpCode.Create("endfinally", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xDC, Return);
        public static readonly OpCode Leave = OpCode.Create("leave", Pop0, Push0, InlineBrTarget, IPrimitive, 1, 0xFF, 0xDD, Branch);
        public static readonly OpCode Leave_S = OpCode.Create("leave.s", Pop0, Push0, ShortInlineBrTarget, IPrimitive, 1, 0xFF, 0xDE, Branch);
        public static readonly OpCode Stind_I = OpCode.Create("stind.i", PopI_PopI, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xDF, Next);
        public static readonly OpCode Conv_U = OpCode.Create("conv.u", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xE0, Next);
        public static readonly OpCode Unused26 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE1, Next);
        public static readonly OpCode Unused27 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE2, Next);
        public static readonly OpCode Unused28 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE3, Next);
        public static readonly OpCode Unused29 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE4, Next);
        public static readonly OpCode Unused30 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE5, Next);
        public static readonly OpCode Unused31 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE6, Next);
        public static readonly OpCode Unused32 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE7, Next);
        public static readonly OpCode Unused33 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE8, Next);
        public static readonly OpCode Unused34 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE9, Next);
        public static readonly OpCode Unused35 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xEA, Next);
        public static readonly OpCode Unused36 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xEB, Next);
        public static readonly OpCode Unused37 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xEC, Next);
        public static readonly OpCode Unused38 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xED, Next);
        public static readonly OpCode Unused39 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xEE, Next);
        public static readonly OpCode Unused40 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xEF, Next);
        public static readonly OpCode Unused41 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF0, Next);
        public static readonly OpCode Unused42 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF1, Next);
        public static readonly OpCode Unused43 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF2, Next);
        public static readonly OpCode Unused44 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF3, Next);
        public static readonly OpCode Unused45 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF4, Next);
        public static readonly OpCode Unused46 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF5, Next);
        public static readonly OpCode Unused47 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF6, Next);
        public static readonly OpCode Unused48 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF7, Next);
        public static readonly OpCode Prefix7 = OpCode.Create("prefix7", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xF8, Meta);
        public static readonly OpCode Prefix6 = OpCode.Create("prefix6", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xF9, Meta);
        public static readonly OpCode Prefix5 = OpCode.Create("prefix5", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFA, Meta);
        public static readonly OpCode Prefix4 = OpCode.Create("prefix4", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFB, Meta);
        public static readonly OpCode Prefix3 = OpCode.Create("prefix3", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFC, Meta);
        public static readonly OpCode Prefix2 = OpCode.Create("prefix2", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFD, Meta);
        public static readonly OpCode Prefix1 = OpCode.Create("prefix1", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFE, Meta);
        public static readonly OpCode Prefixref = OpCode.Create("prefixref", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFF, Meta);

        public static readonly OpCode Arglist = OpCode.Create("arglist", Pop0, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x00, Next);
        public static readonly OpCode Ceq = OpCode.Create("ceq", Pop1_Pop1, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x01, Next);
        public static readonly OpCode Cgt = OpCode.Create("cgt", Pop1_Pop1, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x02, Next);
        public static readonly OpCode Cgt_Un = OpCode.Create("cgt.un", Pop1_Pop1, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x03, Next);
        public static readonly OpCode Clt = OpCode.Create("clt", Pop1_Pop1, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x04, Next);
        public static readonly OpCode Clt_Un = OpCode.Create("clt.un", Pop1_Pop1, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x05, Next);
        public static readonly OpCode Ldftn = OpCode.Create("ldftn", Pop0, PushI, InlineMethod, IPrimitive, 2, 0xFE, 0x06, Next);
        public static readonly OpCode Ldvirtftn = OpCode.Create("ldvirtftn", PopRef, PushI, InlineMethod, IPrimitive, 2, 0xFE, 0x07, Next);
        public static readonly OpCode Unused56 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x08, Next);
        public static readonly OpCode Ldarg = OpCode.Create("ldarg", Pop0, Push1, InlineVar, IPrimitive, 2, 0xFE, 0x09, Next);
        public static readonly OpCode Ldarga = OpCode.Create("ldarga", Pop0, PushI, InlineVar, IPrimitive, 2, 0xFE, 0x0A, Next);
        public static readonly OpCode Starg = OpCode.Create("starg", Pop1, Push0, InlineVar, IPrimitive, 2, 0xFE, 0x0B, Next);
        public static readonly OpCode Ldloc = OpCode.Create("ldloc", Pop0, Push1, InlineVar, IPrimitive, 2, 0xFE, 0x0C, Next);
        public static readonly OpCode Ldloca = OpCode.Create("ldloca", Pop0, PushI, InlineVar, IPrimitive, 2, 0xFE, 0x0D, Next);
        public static readonly OpCode Stloc = OpCode.Create("stloc", Pop1, Push0, InlineVar, IPrimitive, 2, 0xFE, 0x0E, Next);
        public static readonly OpCode Localloc = OpCode.Create("localloc", PopI, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x0F, Next);
        public static readonly OpCode Unused57 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x10, Next);
        public static readonly OpCode Endfilter = OpCode.Create("endfilter", PopI, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x11, Return);
        public static readonly OpCode Unaligned = OpCode.Create("unaligned.", Pop0, Push0, ShortInlineI, IPrefix, 2, 0xFE, 0x12, Meta);
        public static readonly OpCode Volatile = OpCode.Create("volatile.", Pop0, Push0, InlineNone, IPrefix, 2, 0xFE, 0x13, Meta);
        public static readonly OpCode Tailcall = OpCode.Create("tail.", Pop0, Push0, InlineNone, IPrefix, 2, 0xFE, 0x14, Meta);
        public static readonly OpCode Initobj = OpCode.Create("initobj", PopI, Push0, InlineType, IObjModel, 2, 0xFE, 0x15, Next);
        public static readonly OpCode Constrained = OpCode.Create("constrained.", Pop0, Push0, InlineType, IPrefix, 2, 0xFE, 0x16, Meta);
        public static readonly OpCode Cpblk = OpCode.Create("cpblk", PopI_PopI_PopI, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x17, Next);
        public static readonly OpCode Initblk = OpCode.Create("initblk", PopI_PopI_PopI, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x18, Next);
        public static readonly OpCode Unused69 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x19, Next);
        public static readonly OpCode Rethrow = OpCode.Create("rethrow", Pop0, Push0, InlineNone, IObjModel, 2, 0xFE, 0x1A, ControlFlowKind.Throw);
        public static readonly OpCode Unused51 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x1B, Next);
        public static readonly OpCode Sizeof = OpCode.Create("sizeof", Pop0, PushI, InlineType, IPrimitive, 2, 0xFE, 0x1C, Next);
        public static readonly OpCode Refanytype = OpCode.Create("refanytype", Pop1, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x1D, Next);
        public static readonly OpCode Readonly = OpCode.Create("readonly.", Pop0, Push0, InlineNone, IPrefix, 2, 0xFE, 0x1E, Meta);
        public static readonly OpCode Unused53 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x1F, Next);
        public static readonly OpCode Unused54 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x20, Next);
        public static readonly OpCode Unused55 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x21, Next);
        public static readonly OpCode Unused70 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x22, Next);

        //// These are not real opcodes, but they are handy internally in the EE

        //            //public static readonly OpCode ILLEGAL = OpCode.Create("illegal", Pop0, Push0, InlineNone, IInternal, 0, MOOT, MOOT, Meta);
        //            //public static readonly OpCode MACRO_END = OpCode.Create("endmac", Pop0, Push0, InlineNone, IInternal, 0, MOOT, MOOT, Meta);
        //            //public static readonly OpCode CODE_LABEL = OpCode.Create("codelabel", Pop0, Push0, InlineNone, IInternal, 0, MOOT, MOOT, Meta);

        //            OPALIAS(BRNULL, "brnull", BRFALSE);
        //            OPALIAS(BRNULL_S, "brnull.s", BRFALSE_S);
        //            OPALIAS(BRZERO, "brzero", BRFALSE);
        //            OPALIAS(BRZERO_S, "brzero.s", BRFALSE_S);
        //            OPALIAS(BRINST, "brinst", BRTRUE);
        //            OPALIAS(BRINST_S, "brinst.s", BRTRUE_S);
        //            OPALIAS(LDIND_U8, "ldind.u8", LDIND_I8);
        //            OPALIAS(LDELEM_U8, "ldelem.u8", LDELEM_I8);
        //            OPALIAS(LDELEM_ANY, "ldelem.any", LDELEM);
        //            OPALIAS(STELEM_ANY, "stelem.any", STELEM);
        //            OPALIAS(LDC_I4_M1x, "ldc.i4.M1", LDC_I4_M1);
        //            OPALIAS(ENDFAULT, "endfault", ENDFINALLY);
    }
}