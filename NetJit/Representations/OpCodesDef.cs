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
using System.Net;
using static NetJit.Representations.PopBehaviour;
using static NetJit.Representations.PushBehaviour;
using static NetJit.Representations.OperandParams;
using static NetJit.Representations.OpCodeKind;
using static NetJit.Representations.ControlFlowKind;

namespace NetJit.Representations
{
    [Flags]
    public enum PopBehaviour 
    {
        Pop0 = 1 << 0,
        Pop1 = 1 << 1,
        Pop2 = 1 << 2,
        PopI = 1 << 3,
        PopI4 = 1 << 4, 
        PopI8 = 1 << 5,
        PopR4 = 1 << 6,
        PopR8 = 1 << 7,
        PopRef = 1 << 8,
        VarPop = 1 << 9,
        Pop2I = 1 << 10
    }

    [Flags]
    public enum PushBehaviour
    {
        Push0 = 1 << 0,
        Push1 = 1 << 1,
        Push2 = 1 << 2,
        PushI = 1 << 3,
        PushRef = 1 << 4,
        PushI4 = 1 << 5,
        PushI8 = 1 << 6,
        PushR4 = 1 << 7,
        PushR8 = 1 << 8,
        VarPush = 1 << 9,
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
        NEXT = 0,
        BRANCH = 1,
        COND_BRANCH = 2,
        CALL = 3,
        RETURN = 4,
        META = 5,
        THROW = 6,
        BREAK = 7
    }

    public static class OpCodesDef
    {
        public static void OPDEF(params dynamic[] f)
        {
        }

        // TODO not yet working, how to have multiple pops/pushes?
        static OpCodesDef()
        {

            // If the first byte of the standard encoding is 0xFF, then
            // the second byte can be used as 1 byte encoding.  Otherwise                                                               l   b         b
            // the encoding is two bytes.                                                                                               e   y         y
            //                                                                                                                          n   t         t
            //                                                                                                                          g   e         e
            //                                                                                                           (unused);       t
            //  Canonical Name                    String Name              Stack Behaviour           Operand Params    Opcode Kind      h   1         2    Control Flow
            // -------------------------------------------------------------------------------------------------------------------------------------------------------
            OpCode CEE_NOP = OpCode.Create("nop", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x00, NEXT);
            OpCode CEE_BREAK = OpCode.Create("break", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x01, BREAK);
            OpCode CEE_LDARG_0 = OpCode.Create("ldarg.0", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x02, NEXT);
            OpCode CEE_LDARG_1 = OpCode.Create("ldarg.1", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x03, NEXT);
            OpCode CEE_LDARG_2 = OpCode.Create("ldarg.2", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x04, NEXT);
            OpCode CEE_LDARG_3 = OpCode.Create("ldarg.3", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x05, NEXT);
            OpCode CEE_LDLOC_0 = OpCode.Create("ldloc.0", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x06, NEXT);
            OpCode CEE_LDLOC_1 = OpCode.Create("ldloc.1", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x07, NEXT);
            OpCode CEE_LDLOC_2 = OpCode.Create("ldloc.2", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x08, NEXT);
            OpCode CEE_LDLOC_3 = OpCode.Create("ldloc.3", Pop0, Push1, InlineNone, IMacro, 1, 0xFF, 0x09, NEXT);
            OpCode CEE_STLOC_0 = OpCode.Create("stloc.0", Pop1, Push0, InlineNone, IMacro, 1, 0xFF, 0x0A, NEXT);
            OpCode CEE_STLOC_1 = OpCode.Create("stloc.1", Pop1, Push0, InlineNone, IMacro, 1, 0xFF, 0x0B, NEXT);
            OpCode CEE_STLOC_2 = OpCode.Create("stloc.2", Pop1, Push0, InlineNone, IMacro, 1, 0xFF, 0x0C, NEXT);
            OpCode CEE_STLOC_3 = OpCode.Create("stloc.3", Pop1, Push0, InlineNone, IMacro, 1, 0xFF, 0x0D, NEXT);
            OpCode CEE_LDARG_S = OpCode.Create("ldarg.s", Pop0, Push1, ShortInlineVar, IMacro, 1, 0xFF, 0x0E, NEXT);
            OpCode CEE_LDARGA_S = OpCode.Create("ldarga.s", Pop0, PushI, ShortInlineVar, IMacro, 1, 0xFF, 0x0F, NEXT);
            OpCode CEE_STARG_S = OpCode.Create("starg.s", Pop1, Push0, ShortInlineVar, IMacro, 1, 0xFF, 0x10, NEXT);
            OpCode CEE_LDLOC_S = OpCode.Create("ldloc.s", Pop0, Push1, ShortInlineVar, IMacro, 1, 0xFF, 0x11, NEXT);
            OpCode CEE_LDLOCA_S = OpCode.Create("ldloca.s", Pop0, PushI, ShortInlineVar, IMacro, 1, 0xFF, 0x12, NEXT);
            OpCode CEE_STLOC_S = OpCode.Create("stloc.s", Pop1, Push0, ShortInlineVar, IMacro, 1, 0xFF, 0x13, NEXT);
            OpCode CEE_LDNULL = OpCode.Create("ldnull", Pop0, PushRef, InlineNone, IPrimitive, 1, 0xFF, 0x14, NEXT);
            OpCode CEE_LDC_I4_M1 = OpCode.Create("ldc.i4.m1", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x15, NEXT);
            OpCode CEE_LDC_I4_0 = OpCode.Create("ldc.i4.0", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x16, NEXT);
            OpCode CEE_LDC_I4_1 = OpCode.Create("ldc.i4.1", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x17, NEXT);
            OpCode CEE_LDC_I4_2 = OpCode.Create("ldc.i4.2", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x18, NEXT);
            OpCode CEE_LDC_I4_3 = OpCode.Create("ldc.i4.3", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x19, NEXT);
            OpCode CEE_LDC_I4_4 = OpCode.Create("ldc.i4.4", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x1A, NEXT);
            OpCode CEE_LDC_I4_5 = OpCode.Create("ldc.i4.5", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x1B, NEXT);
            OpCode CEE_LDC_I4_6 = OpCode.Create("ldc.i4.6", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x1C, NEXT);
            OpCode CEE_LDC_I4_7 = OpCode.Create("ldc.i4.7", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x1D, NEXT);
            OpCode CEE_LDC_I4_8 = OpCode.Create("ldc.i4.8", Pop0, PushI, InlineNone, IMacro, 1, 0xFF, 0x1E, NEXT);
            OpCode CEE_LDC_I4_S = OpCode.Create("ldc.i4.s", Pop0, PushI, ShortInlineI, IMacro, 1, 0xFF, 0x1F, NEXT);
            OpCode CEE_LDC_I4 = OpCode.Create("ldc.i4", Pop0, PushI, InlineI, IPrimitive, 1, 0xFF, 0x20, NEXT);
            OpCode CEE_LDC_I8 = OpCode.Create("ldc.i8", Pop0, PushI8, InlineI8, IPrimitive, 1, 0xFF, 0x21, NEXT);
            OpCode CEE_LDC_R4 = OpCode.Create("ldc.r4", Pop0, PushR4, ShortInlineR, IPrimitive, 1, 0xFF, 0x22, NEXT);
            OpCode CEE_LDC_R8 = OpCode.Create("ldc.r8", Pop0, PushR8, InlineR, IPrimitive, 1, 0xFF, 0x23, NEXT);
            OpCode CEE_UNUSED49 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x24, NEXT);
            OpCode CEE_DUP = OpCode.Create("dup", Pop1, Push2, InlineNone, IPrimitive, 1, 0xFF, 0x25, NEXT);
            OpCode CEE_POP = OpCode.Create("pop", Pop1, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x26, NEXT);
            OpCode CEE_JMP = OpCode.Create("jmp", Pop0, Push0, InlineMethod, IPrimitive, 1, 0xFF, 0x27, CALL);
            OpCode CEE_CALL = OpCode.Create("call", VarPop, VarPush, InlineMethod, IPrimitive, 1, 0xFF, 0x28, CALL);
            OpCode CEE_CALLI = OpCode.Create("calli", VarPop, VarPush, InlineSig, IPrimitive, 1, 0xFF, 0x29, CALL);
            OpCode CEE_RET = OpCode.Create("ret", VarPop, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x2A, RETURN);
            OpCode CEE_BR_S = OpCode.Create("br.s", Pop0, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x2B, BRANCH);
            OpCode CEE_BRFALSE_S = OpCode.Create("brfalse.s", PopI, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x2C, COND_BRANCH);
            OpCode CEE_BRTRUE_S = OpCode.Create("brtrue.s", PopI, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x2D, COND_BRANCH);
            OpCode CEE_BEQ_S = OpCode.Create("beq.s", Pop2, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x2E, COND_BRANCH);
            OpCode CEE_BGE_S = OpCode.Create("bge.s", Pop2, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x2F, COND_BRANCH);
            OpCode CEE_BGT_S = OpCode.Create("bgt.s", Pop2, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x30, COND_BRANCH);
            OpCode CEE_BLE_S = OpCode.Create("ble.s", Pop2, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x31, COND_BRANCH);
            OpCode CEE_BLT_S = OpCode.Create("blt.s", Pop2, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x32, COND_BRANCH);
            OpCode CEE_BNE_UN_S = OpCode.Create("bne.un.s", Pop2, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x33, COND_BRANCH);
            OpCode CEE_BGE_UN_S = OpCode.Create("bge.un.s", Pop2, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x34, COND_BRANCH);
            OpCode CEE_BGT_UN_S = OpCode.Create("bgt.un.s", Pop2, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x35, COND_BRANCH);
            OpCode CEE_BLE_UN_S = OpCode.Create("ble.un.s", Pop2, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x36, COND_BRANCH);
            OpCode CEE_BLT_UN_S = OpCode.Create("blt.un.s", Pop2, Push0, ShortInlineBrTarget, IMacro, 1, 0xFF, 0x37, COND_BRANCH);
            OpCode CEE_BR = OpCode.Create("br", Pop0, Push0, InlineBrTarget, IPrimitive, 1, 0xFF, 0x38, BRANCH);
            OpCode CEE_BRFALSE = OpCode.Create("brfalse", PopI, Push0, InlineBrTarget, IPrimitive, 1, 0xFF, 0x39, COND_BRANCH);
            OpCode CEE_BRTRUE = OpCode.Create("brtrue", PopI, Push0, InlineBrTarget, IPrimitive, 1, 0xFF, 0x3A, COND_BRANCH);
            OpCode CEE_BEQ = OpCode.Create("beq", Pop2, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x3B, COND_BRANCH);
            OpCode CEE_BGE = OpCode.Create("bge", Pop2, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x3C, COND_BRANCH);
            OpCode CEE_BGT = OpCode.Create("bgt", Pop2, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x3D, COND_BRANCH);
            OpCode CEE_BLE = OpCode.Create("ble", Pop2, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x3E, COND_BRANCH);
            OpCode CEE_BLT = OpCode.Create("blt", Pop2, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x3F, COND_BRANCH);
            OpCode CEE_BNE_UN = OpCode.Create("bne.un", Pop2, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x40, COND_BRANCH);
            OpCode CEE_BGE_UN = OpCode.Create("bge.un", Pop2, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x41, COND_BRANCH);
            OpCode CEE_BGT_UN = OpCode.Create("bgt.un", Pop2, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x42, COND_BRANCH);
            OpCode CEE_BLE_UN = OpCode.Create("ble.un", Pop2, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x43, COND_BRANCH);
            OpCode CEE_BLT_UN = OpCode.Create("blt.un", Pop2, Push0, InlineBrTarget, IMacro, 1, 0xFF, 0x44, COND_BRANCH);
            OpCode CEE_SWITCH = OpCode.Create("switch", PopI, Push0, InlineSwitch, IPrimitive, 1, 0xFF, 0x45, COND_BRANCH);
            OpCode CEE_LDIND_I1 = OpCode.Create("ldind.i1", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x46, NEXT);
            OpCode CEE_LDIND_U1 = OpCode.Create("ldind.u1", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x47, NEXT);
            OpCode CEE_LDIND_I2 = OpCode.Create("ldind.i2", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x48, NEXT);
            OpCode CEE_LDIND_U2 = OpCode.Create("ldind.u2", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x49, NEXT);
            OpCode CEE_LDIND_I4 = OpCode.Create("ldind.i4", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x4A, NEXT);
            OpCode CEE_LDIND_U4 = OpCode.Create("ldind.u4", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x4B, NEXT);
            OpCode CEE_LDIND_I8 = OpCode.Create("ldind.i8", PopI, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0x4C, NEXT);
            OpCode CEE_LDIND_I = OpCode.Create("ldind.i", PopI, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x4D, NEXT);
            OpCode CEE_LDIND_R4 = OpCode.Create("ldind.r4", PopI, PushR4, InlineNone, IPrimitive, 1, 0xFF, 0x4E, NEXT);
            OpCode CEE_LDIND_R8 = OpCode.Create("ldind.r8", PopI, PushR8, InlineNone, IPrimitive, 1, 0xFF, 0x4F, NEXT);
            OpCode CEE_LDIND_REF = OpCode.Create("ldind.ref", PopI, PushRef, InlineNone, IPrimitive, 1, 0xFF, 0x50, NEXT);
            OpCode CEE_STIND_REF = OpCode.Create("stind.ref", PopI + PopI, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x51, NEXT);
            OpCode CEE_STIND_I1 = OpCode.Create("stind.i1", PopI + PopI, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x52, NEXT);
            OpCode CEE_STIND_I2 = OpCode.Create("stind.i2", PopI + PopI, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x53, NEXT);
            OpCode CEE_STIND_I4 = OpCode.Create("stind.i4", PopI + PopI, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x54, NEXT);
            OpCode CEE_STIND_I8 = OpCode.Create("stind.i8", PopI + PopI8, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x55, NEXT);
            OpCode CEE_STIND_R4 = OpCode.Create("stind.r4", PopI + PopR4, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x56, NEXT);
            OpCode CEE_STIND_R8 = OpCode.Create("stind.r8", PopI + PopR8, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x57, NEXT);
            OpCode CEE_ADD = OpCode.Create("add", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x58, NEXT);
            OpCode CEE_SUB = OpCode.Create("sub", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x59, NEXT);
            OpCode CEE_MUL = OpCode.Create("mul", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5A, NEXT);
            OpCode CEE_DIV = OpCode.Create("div", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5B, NEXT);
            OpCode CEE_DIV_UN = OpCode.Create("div.un", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5C, NEXT);
            OpCode CEE_REM = OpCode.Create("rem", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5D, NEXT);
            OpCode CEE_REM_UN = OpCode.Create("rem.un", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5E, NEXT);
            OpCode CEE_AND = OpCode.Create("and", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x5F, NEXT);
            OpCode CEE_OR = OpCode.Create("or", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x60, NEXT);
            OpCode CEE_XOR = OpCode.Create("xor", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x61, NEXT);
            OpCode CEE_SHL = OpCode.Create("shl", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x62, NEXT);
            OpCode CEE_SHR = OpCode.Create("shr", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x63, NEXT);
            OpCode CEE_SHR_UN = OpCode.Create("shr.un", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x64, NEXT);
            OpCode CEE_NEG = OpCode.Create("neg", Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x65, NEXT);
            OpCode CEE_NOT = OpCode.Create("not", Pop1, Push1, InlineNone, IPrimitive, 1, 0xFF, 0x66, NEXT);
            OpCode CEE_CONV_I1 = OpCode.Create("conv.i1", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x67, NEXT);
            OpCode CEE_CONV_I2 = OpCode.Create("conv.i2", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x68, NEXT);
            OpCode CEE_CONV_I4 = OpCode.Create("conv.i4", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x69, NEXT);
            OpCode CEE_CONV_I8 = OpCode.Create("conv.i8", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0x6A, NEXT);
            OpCode CEE_CONV_R4 = OpCode.Create("conv.r4", Pop1, PushR4, InlineNone, IPrimitive, 1, 0xFF, 0x6B, NEXT);
            OpCode CEE_CONV_R8 = OpCode.Create("conv.r8", Pop1, PushR8, InlineNone, IPrimitive, 1, 0xFF, 0x6C, NEXT);
            OpCode CEE_CONV_U4 = OpCode.Create("conv.u4", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x6D, NEXT);
            OpCode CEE_CONV_U8 = OpCode.Create("conv.u8", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0x6E, NEXT);
            OpCode CEE_CALLVIRT = OpCode.Create("callvirt", VarPop, VarPush, InlineMethod, IObjModel, 1, 0xFF, 0x6F, CALL);
            OpCode CEE_CPOBJ = OpCode.Create("cpobj", PopI + PopI, Push0, InlineType, IObjModel, 1, 0xFF, 0x70, NEXT);
            OpCode CEE_LDOBJ = OpCode.Create("ldobj", PopI, Push1, InlineType, IObjModel, 1, 0xFF, 0x71, NEXT);
            OpCode CEE_LDSTR = OpCode.Create("ldstr", Pop0, PushRef, InlineString, IObjModel, 1, 0xFF, 0x72, NEXT);
            OpCode CEE_NEWOBJ = OpCode.Create("newobj", VarPop, PushRef, InlineMethod, IObjModel, 1, 0xFF, 0x73, CALL);
            OpCode CEE_CASTCLASS = OpCode.Create("castclass", PopRef, PushRef, InlineType, IObjModel, 1, 0xFF, 0x74, NEXT);
            OpCode CEE_ISINST = OpCode.Create("isinst", PopRef, PushI, InlineType, IObjModel, 1, 0xFF, 0x75, NEXT);
            OpCode CEE_CONV_R_UN = OpCode.Create("conv.r.un", Pop1, PushR8, InlineNone, IPrimitive, 1, 0xFF, 0x76, NEXT);
            OpCode CEE_UNUSED58 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x77, NEXT);
            OpCode CEE_UNUSED1 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0x78, NEXT);
            OpCode CEE_UNBOX = OpCode.Create("unbox", PopRef, PushI, InlineType, IPrimitive, 1, 0xFF, 0x79, NEXT);
            OpCode CEE_THROW = OpCode.Create("throw", PopRef, Push0, InlineNone, IObjModel, 1, 0xFF, 0x7A, THROW);
            OpCode CEE_LDFLD = OpCode.Create("ldfld", PopRef, Push1, InlineField, IObjModel, 1, 0xFF, 0x7B, NEXT);
            OpCode CEE_LDFLDA = OpCode.Create("ldflda", PopRef, PushI, InlineField, IObjModel, 1, 0xFF, 0x7C, NEXT);
            OpCode CEE_STFLD = OpCode.Create("stfld", PopRef + Pop1, Push0, InlineField, IObjModel, 1, 0xFF, 0x7D, NEXT);
            OpCode CEE_LDSFLD = OpCode.Create("ldsfld", Pop0, Push1, InlineField, IObjModel, 1, 0xFF, 0x7E, NEXT);
            OpCode CEE_LDSFLDA = OpCode.Create("ldsflda", Pop0, PushI, InlineField, IObjModel, 1, 0xFF, 0x7F, NEXT);
            OpCode CEE_STSFLD = OpCode.Create("stsfld", Pop1, Push0, InlineField, IObjModel, 1, 0xFF, 0x80, NEXT);
            OpCode CEE_STOBJ = OpCode.Create("stobj", PopI + Pop1, Push0, InlineType, IPrimitive, 1, 0xFF, 0x81, NEXT);
            OpCode CEE_CONV_OVF_I1_UN = OpCode.Create("conv.ovf.i1.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x82, NEXT);
            OpCode CEE_CONV_OVF_I2_UN = OpCode.Create("conv.ovf.i2.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x83, NEXT);
            OpCode CEE_CONV_OVF_I4_UN = OpCode.Create("conv.ovf.i4.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x84, NEXT);
            OpCode CEE_CONV_OVF_I8_UN = OpCode.Create("conv.ovf.i8.un", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0x85, NEXT);
            OpCode CEE_CONV_OVF_U1_UN = OpCode.Create("conv.ovf.u1.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x86, NEXT);
            OpCode CEE_CONV_OVF_U2_UN = OpCode.Create("conv.ovf.u2.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x87, NEXT);
            OpCode CEE_CONV_OVF_U4_UN = OpCode.Create("conv.ovf.u4.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x88, NEXT);
            OpCode CEE_CONV_OVF_U8_UN = OpCode.Create("conv.ovf.u8.un", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0x89, NEXT);
            OpCode CEE_CONV_OVF_I_UN = OpCode.Create("conv.ovf.i.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x8A, NEXT);
            OpCode CEE_CONV_OVF_U_UN = OpCode.Create("conv.ovf.u.un", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0x8B, NEXT);
            OpCode CEE_BOX = OpCode.Create("box", Pop1, PushRef, InlineType, IPrimitive, 1, 0xFF, 0x8C, NEXT);
            OpCode CEE_NEWARR = OpCode.Create("newarr", PopI, PushRef, InlineType, IObjModel, 1, 0xFF, 0x8D, NEXT);
            OpCode CEE_LDLEN = OpCode.Create("ldlen", PopRef, PushI, InlineNone, IObjModel, 1, 0xFF, 0x8E, NEXT);
            OpCode CEE_LDELEMA = OpCode.Create("ldelema", PopRef + PopI, PushI, InlineType, IObjModel, 1, 0xFF, 0x8F, NEXT);
            OpCode CEE_LDELEM_I1 = OpCode.Create("ldelem.i1", PopRef + PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x90, NEXT);
            OpCode CEE_LDELEM_U1 = OpCode.Create("ldelem.u1", PopRef + PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x91, NEXT);
            OpCode CEE_LDELEM_I2 = OpCode.Create("ldelem.i2", PopRef + PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x92, NEXT);
            OpCode CEE_LDELEM_U2 = OpCode.Create("ldelem.u2", PopRef + PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x93, NEXT);
            OpCode CEE_LDELEM_I4 = OpCode.Create("ldelem.i4", PopRef + PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x94, NEXT);
            OpCode CEE_LDELEM_U4 = OpCode.Create("ldelem.u4", PopRef + PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x95, NEXT);
            OpCode CEE_LDELEM_I8 = OpCode.Create("ldelem.i8", PopRef + PopI, PushI8, InlineNone, IObjModel, 1, 0xFF, 0x96, NEXT);
            OpCode CEE_LDELEM_I = OpCode.Create("ldelem.i", PopRef + PopI, PushI, InlineNone, IObjModel, 1, 0xFF, 0x97, NEXT);
            OpCode CEE_LDELEM_R4 = OpCode.Create("ldelem.r4", PopRef + PopI, PushR4, InlineNone, IObjModel, 1, 0xFF, 0x98, NEXT);
            OpCode CEE_LDELEM_R8 = OpCode.Create("ldelem.r8", PopRef + PopI, PushR8, InlineNone, IObjModel, 1, 0xFF, 0x99, NEXT);
            OpCode CEE_LDELEM_REF = OpCode.Create("ldelem.ref", PopRef + PopI, PushRef, InlineNone, IObjModel, 1, 0xFF, 0x9A, NEXT);
            OpCode CEE_STELEM_I = OpCode.Create("stelem.i", PopRef + PopI + PopI, Push0, InlineNone, IObjModel, 1, 0xFF, 0x9B, NEXT);
            OpCode CEE_STELEM_I1 = OpCode.Create("stelem.i1", PopRef + PopI + PopI, Push0, InlineNone, IObjModel, 1, 0xFF, 0x9C, NEXT);
            OpCode CEE_STELEM_I2 = OpCode.Create("stelem.i2", PopRef + PopI + PopI, Push0, InlineNone, IObjModel, 1, 0xFF, 0x9D, NEXT);
            OpCode CEE_STELEM_I4 = OpCode.Create("stelem.i4", PopRef + PopI + PopI, Push0, InlineNone, IObjModel, 1, 0xFF, 0x9E, NEXT);
            OpCode CEE_STELEM_I8 = OpCode.Create("stelem.i8", PopRef + PopI + PopI8, Push0, InlineNone, IObjModel, 1, 0xFF, 0x9F, NEXT);
            OpCode CEE_STELEM_R4 = OpCode.Create("stelem.r4", PopRef + PopI + PopR4, Push0, InlineNone, IObjModel, 1, 0xFF, 0xA0, NEXT);
            OpCode CEE_STELEM_R8 = OpCode.Create("stelem.r8", PopRef + PopI + PopR8, Push0, InlineNone, IObjModel, 1, 0xFF, 0xA1, NEXT);
            OpCode CEE_STELEM_REF = OpCode.Create("stelem.ref", PopRef + PopI + PopRef, Push0, InlineNone, IObjModel, 1, 0xFF, 0xA2,
                NEXT);
            OpCode CEE_LDELEM = OpCode.Create("ldelem", PopRef + PopI, Push1, InlineType, IObjModel, 1, 0xFF, 0xA3, NEXT);
            OpCode CEE_STELEM = OpCode.Create("stelem", PopRef + PopI + Pop1, Push0, InlineType, IObjModel, 1, 0xFF, 0xA4, NEXT);
            OpCode CEE_UNBOX_ANY = OpCode.Create("unbox.any", PopRef, Push1, InlineType, IObjModel, 1, 0xFF, 0xA5, NEXT);
            OpCode CEE_UNUSED5 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xA6, NEXT);
            OpCode CEE_UNUSED6 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xA7, NEXT);
            OpCode CEE_UNUSED7 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xA8, NEXT);
            OpCode CEE_UNUSED8 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xA9, NEXT);
            OpCode CEE_UNUSED9 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAA, NEXT);
            OpCode CEE_UNUSED10 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAB, NEXT);
            OpCode CEE_UNUSED11 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAC, NEXT);
            OpCode CEE_UNUSED12 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAD, NEXT);
            OpCode CEE_UNUSED13 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAE, NEXT);
            OpCode CEE_UNUSED14 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xAF, NEXT);
            OpCode CEE_UNUSED15 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xB0, NEXT);
            OpCode CEE_UNUSED16 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xB1, NEXT);
            OpCode CEE_UNUSED17 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xB2, NEXT);
            OpCode CEE_CONV_OVF_I1 = OpCode.Create("conv.ovf.i1", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB3, NEXT);
            OpCode CEE_CONV_OVF_U1 = OpCode.Create("conv.ovf.u1", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB4, NEXT);
            OpCode CEE_CONV_OVF_I2 = OpCode.Create("conv.ovf.i2", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB5, NEXT);
            OpCode CEE_CONV_OVF_U2 = OpCode.Create("conv.ovf.u2", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB6, NEXT);
            OpCode CEE_CONV_OVF_I4 = OpCode.Create("conv.ovf.i4", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB7, NEXT);
            OpCode CEE_CONV_OVF_U4 = OpCode.Create("conv.ovf.u4", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xB8, NEXT);
            OpCode CEE_CONV_OVF_I8 = OpCode.Create("conv.ovf.i8", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0xB9, NEXT);
            OpCode CEE_CONV_OVF_U8 = OpCode.Create("conv.ovf.u8", Pop1, PushI8, InlineNone, IPrimitive, 1, 0xFF, 0xBA, NEXT);
            OpCode CEE_UNUSED50 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xBB, NEXT);
            OpCode CEE_UNUSED18 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xBC, NEXT);
            OpCode CEE_UNUSED19 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xBD, NEXT);
            OpCode CEE_UNUSED20 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xBE, NEXT);
            OpCode CEE_UNUSED21 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xBF, NEXT);
            OpCode CEE_UNUSED22 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC0, NEXT);
            OpCode CEE_UNUSED23 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC1, NEXT);
            OpCode CEE_REFANYVAL = OpCode.Create("refanyval", Pop1, PushI, InlineType, IPrimitive, 1, 0xFF, 0xC2, NEXT);
            OpCode CEE_CKFINITE = OpCode.Create("ckfinite", Pop1, PushR8, InlineNone, IPrimitive, 1, 0xFF, 0xC3, NEXT);
            OpCode CEE_UNUSED24 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC4, NEXT);
            OpCode CEE_UNUSED25 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC5, NEXT);
            OpCode CEE_MKREFANY = OpCode.Create("mkrefany", PopI, Push1, InlineType, IPrimitive, 1, 0xFF, 0xC6, NEXT);
            OpCode CEE_UNUSED59 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC7, NEXT);
            OpCode CEE_UNUSED60 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC8, NEXT);
            OpCode CEE_UNUSED61 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xC9, NEXT);
            OpCode CEE_UNUSED62 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCA, NEXT);
            OpCode CEE_UNUSED63 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCB, NEXT);
            OpCode CEE_UNUSED64 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCC, NEXT);
            OpCode CEE_UNUSED65 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCD, NEXT);
            OpCode CEE_UNUSED66 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCE, NEXT);
            OpCode CEE_UNUSED67 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xCF, NEXT);
            OpCode CEE_LDTOKEN = OpCode.Create("ldtoken", Pop0, PushI, InlineTok, IPrimitive, 1, 0xFF, 0xD0, NEXT);
            OpCode CEE_CONV_U2 = OpCode.Create("conv.u2", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xD1, NEXT);
            OpCode CEE_CONV_U1 = OpCode.Create("conv.u1", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xD2, NEXT);
            OpCode CEE_CONV_I = OpCode.Create("conv.i", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xD3, NEXT);
            OpCode CEE_CONV_OVF_I = OpCode.Create("conv.ovf.i", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xD4, NEXT);
            OpCode CEE_CONV_OVF_U = OpCode.Create("conv.ovf.u", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xD5, NEXT);
            OpCode CEE_ADD_OVF = OpCode.Create("add.ovf", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xD6, NEXT);
            OpCode CEE_ADD_OVF_UN = OpCode.Create("add.ovf.un", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xD7, NEXT);
            OpCode CEE_MUL_OVF = OpCode.Create("mul.ovf", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xD8, NEXT);
            OpCode CEE_MUL_OVF_UN = OpCode.Create("mul.ovf.un", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xD9, NEXT);
            OpCode CEE_SUB_OVF = OpCode.Create("sub.ovf", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xDA, NEXT);
            OpCode CEE_SUB_OVF_UN = OpCode.Create("sub.ovf.un", Pop2, Push1, InlineNone, IPrimitive, 1, 0xFF, 0xDB, NEXT);
            OpCode CEE_ENDFINALLY = OpCode.Create("endfinally", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xDC, RETURN);
            OpCode CEE_LEAVE = OpCode.Create("leave", Pop0, Push0, InlineBrTarget, IPrimitive, 1, 0xFF, 0xDD, BRANCH);
            OpCode CEE_LEAVE_S = OpCode.Create("leave.s", Pop0, Push0, ShortInlineBrTarget, IPrimitive, 1, 0xFF, 0xDE, BRANCH);
            OpCode CEE_STIND_I = OpCode.Create("stind.i", PopI + PopI, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xDF, NEXT);
            OpCode CEE_CONV_U = OpCode.Create("conv.u", Pop1, PushI, InlineNone, IPrimitive, 1, 0xFF, 0xE0, NEXT);
            OpCode CEE_UNUSED26 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE1, NEXT);
            OpCode CEE_UNUSED27 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE2, NEXT);
            OpCode CEE_UNUSED28 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE3, NEXT);
            OpCode CEE_UNUSED29 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE4, NEXT);
            OpCode CEE_UNUSED30 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE5, NEXT);
            OpCode CEE_UNUSED31 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE6, NEXT);
            OpCode CEE_UNUSED32 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE7, NEXT);
            OpCode CEE_UNUSED33 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE8, NEXT);
            OpCode CEE_UNUSED34 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xE9, NEXT);
            OpCode CEE_UNUSED35 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xEA, NEXT);
            OpCode CEE_UNUSED36 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xEB, NEXT);
            OpCode CEE_UNUSED37 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xEC, NEXT);
            OpCode CEE_UNUSED38 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xED, NEXT);
            OpCode CEE_UNUSED39 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xEE, NEXT);
            OpCode CEE_UNUSED40 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xEF, NEXT);
            OpCode CEE_UNUSED41 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF0, NEXT);
            OpCode CEE_UNUSED42 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF1, NEXT);
            OpCode CEE_UNUSED43 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF2, NEXT);
            OpCode CEE_UNUSED44 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF3, NEXT);
            OpCode CEE_UNUSED45 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF4, NEXT);
            OpCode CEE_UNUSED46 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF5, NEXT);
            OpCode CEE_UNUSED47 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF6, NEXT);
            OpCode CEE_UNUSED48 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 1, 0xFF, 0xF7, NEXT);
            OpCode CEE_PREFIX7 = OpCode.Create("prefix7", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xF8, META);
            OpCode CEE_PREFIX6 = OpCode.Create("prefix6", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xF9, META);
            OpCode CEE_PREFIX5 = OpCode.Create("prefix5", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFA, META);
            OpCode CEE_PREFIX4 = OpCode.Create("prefix4", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFB, META);
            OpCode CEE_PREFIX3 = OpCode.Create("prefix3", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFC, META);
            OpCode CEE_PREFIX2 = OpCode.Create("prefix2", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFD, META);
            OpCode CEE_PREFIX1 = OpCode.Create("prefix1", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFE, META);
            OpCode CEE_PREFIXREF = OpCode.Create("prefixref", Pop0, Push0, InlineNone, IInternal, 1, 0xFF, 0xFF, META);

            OpCode CEE_ARGLIST = OpCode.Create("arglist", Pop0, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x00, NEXT);
            OpCode CEE_CEQ = OpCode.Create("ceq", Pop2, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x01, NEXT);
            OpCode CEE_CGT = OpCode.Create("cgt", Pop2, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x02, NEXT);
            OpCode CEE_CGT_UN = OpCode.Create("cgt.un", Pop2, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x03, NEXT);
            OpCode CEE_CLT = OpCode.Create("clt", Pop2, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x04, NEXT);
            OpCode CEE_CLT_UN = OpCode.Create("clt.un", Pop2, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x05, NEXT);
            OpCode CEE_LDFTN = OpCode.Create("ldftn", Pop0, PushI, InlineMethod, IPrimitive, 2, 0xFE, 0x06, NEXT);
            OpCode CEE_LDVIRTFTN = OpCode.Create("ldvirtftn", PopRef, PushI, InlineMethod, IPrimitive, 2, 0xFE, 0x07, NEXT);
            OpCode CEE_UNUSED56 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x08, NEXT);
            OpCode CEE_LDARG = OpCode.Create("ldarg", Pop0, Push1, InlineVar, IPrimitive, 2, 0xFE, 0x09, NEXT);
            OpCode CEE_LDARGA = OpCode.Create("ldarga", Pop0, PushI, InlineVar, IPrimitive, 2, 0xFE, 0x0A, NEXT);
            OpCode CEE_STARG = OpCode.Create("starg", Pop1, Push0, InlineVar, IPrimitive, 2, 0xFE, 0x0B, NEXT);
            OpCode CEE_LDLOC = OpCode.Create("ldloc", Pop0, Push1, InlineVar, IPrimitive, 2, 0xFE, 0x0C, NEXT);
            OpCode CEE_LDLOCA = OpCode.Create("ldloca", Pop0, PushI, InlineVar, IPrimitive, 2, 0xFE, 0x0D, NEXT);
            OpCode CEE_STLOC = OpCode.Create("stloc", Pop1, Push0, InlineVar, IPrimitive, 2, 0xFE, 0x0E, NEXT);
            OpCode CEE_LOCALLOC = OpCode.Create("localloc", PopI, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x0F, NEXT);
            OpCode CEE_UNUSED57 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x10, NEXT);
            OpCode CEE_ENDFILTER = OpCode.Create("endfilter", PopI, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x11, RETURN);
            OpCode CEE_UNALIGNED = OpCode.Create("unaligned.", Pop0, Push0, ShortInlineI, IPrefix, 2, 0xFE, 0x12, META);
            OpCode CEE_VOLATILE = OpCode.Create("volatile.", Pop0, Push0, InlineNone, IPrefix, 2, 0xFE, 0x13, META);
            OpCode CEE_TAILCALL = OpCode.Create("tail.", Pop0, Push0, InlineNone, IPrefix, 2, 0xFE, 0x14, META);
            OpCode CEE_INITOBJ = OpCode.Create("initobj", PopI, Push0, InlineType, IObjModel, 2, 0xFE, 0x15, NEXT);
            OpCode CEE_CONSTRAINED = OpCode.Create("constrained.", Pop0, Push0, InlineType, IPrefix, 2, 0xFE, 0x16, META);
            OpCode CEE_CPBLK = OpCode.Create("cpblk", PopI + PopI + PopI, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x17, NEXT);
            OpCode CEE_INITBLK = OpCode.Create("initblk", PopI + PopI + PopI, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x18, NEXT);
            OpCode CEE_UNUSED69 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x19, NEXT);
            OpCode CEE_RETHROW = OpCode.Create("rethrow", Pop0, Push0, InlineNone, IObjModel, 2, 0xFE, 0x1A, THROW);
            OpCode CEE_UNUSED51 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x1B, NEXT);
            OpCode CEE_SIZEOF = OpCode.Create("sizeof", Pop0, PushI, InlineType, IPrimitive, 2, 0xFE, 0x1C, NEXT);
            OpCode CEE_REFANYTYPE = OpCode.Create("refanytype", Pop1, PushI, InlineNone, IPrimitive, 2, 0xFE, 0x1D, NEXT);
            OpCode CEE_READONLY = OpCode.Create("readonly.", Pop0, Push0, InlineNone, IPrefix, 2, 0xFE, 0x1E, META);
            OpCode CEE_UNUSED53 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x1F, NEXT);
            OpCode CEE_UNUSED54 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x20, NEXT);
            OpCode CEE_UNUSED55 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x21, NEXT);
            OpCode CEE_UNUSED70 = OpCode.Create("unused", Pop0, Push0, InlineNone, IPrimitive, 2, 0xFE, 0x22, NEXT);

// These are not real opcodes, but they are handy internally in the EE

            //OpCode CEE_ILLEGAL = OpCode.Create("illegal", Pop0, Push0, InlineNone, IInternal, 0, MOOT, MOOT, META);
            //OpCode CEE_MACRO_END = OpCode.Create("endmac", Pop0, Push0, InlineNone, IInternal, 0, MOOT, MOOT, META);
            //OpCode CEE_CODE_LABEL = OpCode.Create("codelabel", Pop0, Push0, InlineNone, IInternal, 0, MOOT, MOOT, META);

            OPALIAS(CEE_BRNULL, "brnull", CEE_BRFALSE);
            OPALIAS(CEE_BRNULL_S, "brnull.s", CEE_BRFALSE_S);
            OPALIAS(CEE_BRZERO, "brzero", CEE_BRFALSE);
            OPALIAS(CEE_BRZERO_S, "brzero.s", CEE_BRFALSE_S);
            OPALIAS(CEE_BRINST, "brinst", CEE_BRTRUE);
            OPALIAS(CEE_BRINST_S, "brinst.s", CEE_BRTRUE_S);
            OPALIAS(CEE_LDIND_U8, "ldind.u8", CEE_LDIND_I8);
            OPALIAS(CEE_LDELEM_U8, "ldelem.u8", CEE_LDELEM_I8);
            OPALIAS(CEE_LDELEM_ANY, "ldelem.any", CEE_LDELEM);
            OPALIAS(CEE_STELEM_ANY, "stelem.any", CEE_STELEM);
            OPALIAS(CEE_LDC_I4_M1x, "ldc.i4.M1", CEE_LDC_I4_M1);
            OPALIAS(CEE_ENDFAULT, "endfault", CEE_ENDFINALLY);
        }
    }
}