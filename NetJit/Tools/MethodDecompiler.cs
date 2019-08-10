using System;
using System.Buffers;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetRt.Metadata;
using ExceptionHandlingClause = NetRt.Metadata.ExceptionHandlingClause;

// ReSharper disable StringLiteralTypo

namespace NetJit.Tools
{
    public readonly struct MethodDecompiler
    {
        public MethodInformation MethodInformation { get; }
        private readonly IlDecompiler _decompiler;

        public MethodDecompiler(MethodInformation methodInformation)
        {
            if (methodInformation is null) throw new ArgumentNullException(nameof(methodInformation));

            MethodInformation = methodInformation;
            _decompiler = new IlDecompiler(methodInformation);
        }

        public bool TryFormat(Span<char> buffer, out int charsWritten)
        {
            var builder = new BufferStringBuilder(buffer);
            charsWritten = 0;

            if (!TryFormatHeader(builder, ref charsWritten)) return false;

            if (MethodInformation.HasBody)
            {
                if (!TryFormatBody(builder, ref charsWritten)) return false;
            }

            return true;
        }

        private bool TryFormatHeader(BufferStringBuilder builder, ref int charsWritten)
        {
            if (!TryFormatSignature(builder, ref charsWritten)) return false;
            if (!TryFormatReturnAndParams(builder, ref charsWritten)) return false;
            if (!TryFormatImplementationSig(builder, ref charsWritten)) return false;

            return true;
        }


        private bool TryFormatBody(BufferStringBuilder builder, ref int charsWritten)
        {
            if (!TryFormatLocals(builder, ref charsWritten)) return false;
            if (!TryFormatIlAndEh(builder, ref charsWritten)) return false;

            return true;
        }


        private bool TryFormatSignature(BufferStringBuilder builder, ref int charsWritten)
        {
            if (!builder.TryAddWithSpace(".method", ref charsWritten)) return false;

            MethodAttributes flags = MethodInformation.Flags;
            MethodAttributes access = flags & MethodAttributes.MemberAccessMask;

            string accessModifier = access switch
            {
                MethodAttributes.PrivateScope => "compilercontrolled", // not available in C#
                MethodAttributes.Private => "private", // private
                MethodAttributes.FamANDAssem => "famandassem", // private protected
                MethodAttributes.Family => "family", // protected
                MethodAttributes.FamORAssem => "famorassem", // protected internal
                MethodAttributes.Assembly => "assembly", // internal
                MethodAttributes.Public => "public", // public
                _ => string.Empty
            };

            if (!builder.TryAddWithSpace(accessModifier, ref charsWritten)) return false;

            if (!TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.Final), "final", ref charsWritten))
                return false;

            if (!TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.HideBySig), "hidebysig", ref charsWritten))
                return false;

            string staticModifier = flags.HasFlag(MethodAttributes.Static) ? "static" : "instance";

            if (!builder.TryAddWithSpace(staticModifier, ref charsWritten)) return false;

            if (!TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.Virtual), "virtual", ref charsWritten))
                return false;

            if (!TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.CheckAccessOnOverride), "strict", ref charsWritten))
                return false;

            if (!TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.Abstract), "abstract", ref charsWritten))
                return false;

            if (!TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.SpecialName), "specialname", ref charsWritten))
                return false;

            if (!TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.RTSpecialName), "rtspecialname", ref charsWritten))
                return false;

            return true;
        }

        private bool TryFormatImplementationSig(BufferStringBuilder builder, ref int charsWritten)
        {
            return true;
        }

        private bool TryFormatLocals(BufferStringBuilder builder, ref int charsWritten)
        {
            return true;
        }

        private bool TryFormatReturnAndParams(BufferStringBuilder builder, ref int charsWritten)
        {
            return true;
        }

        private bool TryFormatIlAndEh(BufferStringBuilder builder, ref int charsWritten)
        {
            var ehSignificantOffsets = new List<uint>();

            foreach (MethodDataSection section in MethodInformation.MethodDataSections)
            {
                foreach (ExceptionHandlingClause ehClause in section.ExceptionHandlingClauses)
                {
                    ehSignificantOffsets.Add(ehClause.TryOffset);
                    ehSignificantOffsets.Add(ehClause.TryOffset + ehClause.TryLength);
                    ehSignificantOffsets.Add(ehClause.HandlerOffset);
                    ehSignificantOffsets.Add(ehClause.HandlerOffset + ehClause.HandlerLength);
                }
            }

            return true;
        }

        private bool TryAddIfFlagPresent(ref BufferStringBuilder builder, bool flagPresent, string value, ref int charsWritten)
        {
            if (!flagPresent) return true;
            return builder.TryAddWithSpace(value, ref charsWritten);
        }
    }
}