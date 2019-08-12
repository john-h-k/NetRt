using System;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Common;
using NetRt.Metadata;
using NetRt.Metadata.MethodData;
using ExceptionHandlingClause = NetRt.Metadata.MethodData.ExceptionHandlingClause;

// ReSharper disable StringLiteralTypo

namespace NetJit.Tools
{
    public class MethodDecompiler
    {
        public MethodInformation MethodInformation { get; }
        public ReadOnlyMemory<byte> Il => MethodInformation.Il;

        public MethodDecompiler(MethodInformation methodInformation)
        {
            if (methodInformation is null) throw new ArgumentNullException(nameof(methodInformation));

            MethodInformation = methodInformation;
        }

        public bool TryFormat(Span<char> buffer, out int charsWritten)
        {
            var builder = new BufferStringBuilder(buffer);
            charsWritten = 0;

            if (!TryFormatHeader(ref builder, ref charsWritten)) return false;

            if (MethodInformation.HasBody)
            {
                if (!TryFormatBody(ref builder, ref charsWritten)) return false;
            }

            return true;
        }

        private bool TryFormatHeader(ref BufferStringBuilder builder, ref int charsWritten)
        {
            if (!TryFormatSignature(ref builder, ref charsWritten)) return false;
            if (!TryFormatReturnAndParams(ref builder, ref charsWritten)) return false;
            if (!TryFormatImplementationSig(ref builder, ref charsWritten)) return false;
            if (!builder.TryAddNewline(ref charsWritten)) return false;

            return true;
        }


        private bool TryFormatBody(ref BufferStringBuilder builder, ref int charsWritten)
        {
            if (!TryFormatLocals(ref builder, ref charsWritten)) return false;
            if (!TryFormatIlAndEh(ref builder, ref charsWritten)) return false;

            return true;
        }


        private static readonly string[] AccessFlagsToString =
        {
            "compilercontrolled", // not available in C#
            "private", // private
            "famandassem", // private protected
            "assembly", // internal
            "family", // protected
            "famorassem", // protected internal
            "public", // public
        };

        private bool TryFormatSignature(ref BufferStringBuilder builder, ref int charsWritten)
        {
            if (!builder.TryAddWithSpace(".method", ref charsWritten)) return false;

            MethodAttributes flags = MethodInformation.Flags;
            MethodAttributes access = flags & MethodAttributes.MemberAccessMask;

            string accessModifier = AccessFlagsToString[(int)access];

            AllBool b = true;

            b += builder.TryAddWithSpace(accessModifier, ref charsWritten);

            b += TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.Final), "final", ref charsWritten);

            b += TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.HideBySig), "hidebysig",
                ref charsWritten);

            string staticModifier = flags.HasFlag(MethodAttributes.Static) ? "static" : "instance";

            b += builder.TryAddWithSpace(staticModifier, ref charsWritten);

            b += TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.Virtual), "virtual", ref charsWritten);

            b += TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.CheckAccessOnOverride), "strict", ref charsWritten);

            b += TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.Abstract), "abstract", ref charsWritten);

            b += TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.SpecialName), "specialname", ref charsWritten);

            b += TryAddIfFlagPresent(ref builder, flags.HasFlag(MethodAttributes.RTSpecialName), "rtspecialname", ref charsWritten);

            return b;
        }

        private bool TryFormatImplementationSig(ref BufferStringBuilder builder, ref int charsWritten)
        {
            return true;
        }

        private bool TryFormatLocals(ref BufferStringBuilder builder, ref int charsWritten)
        {
            return true;
        }

        private bool TryFormatReturnAndParams(ref BufferStringBuilder builder, ref int charsWritten)
        {
            return true;
        }

        [DebuggerDisplay("{" + nameof(ToString) + "()}")]
        private struct EhChange : IComparable<EhChange>
        {
            public EhChange(string name, uint offset, int relativeTabbing)
            {
                Name = name;
                Offset = offset;
                RelativeTabbing = relativeTabbing;
                _baselineTabbing = -1;
            }

            public string Name { get; }
            public uint Offset { get; }
            public int RelativeTabbing { get; }

            private int _baselineTabbing;
            public int BaselineTabbing
            {
                get => _baselineTabbing;
                set
                {
                    if (value < 0 || RelativeTabbing + value < 0) ThrowHelper.ThrowArgumentOutOfRangeException(nameof(value));
                    _baselineTabbing = value;
                }
            }

            public override string ToString()
            {
                static void Tab(ref Span<char> buffer, int count)
                {
                    var i = 0;
                    for (; i < count; i++)
                    {
                        buffer[i] = '\t';
                    }

                    buffer = buffer.Slice(i);
                }



                SpanAction<char, (EhChange eh, int tabs)> action;
                int tabs = BaselineTabbing + RelativeTabbing;
                int len;
                if (Name is null)
                {
                    len = tabs + 1;
                    action = (span, e) =>
                    {
                        Tab(ref span, e.tabs);
                        span[0] = '}';
                    };
                }
                else
                {
                    len = tabs + Name.Length + 1 + tabs + 1;
                    action = (span, e) =>
                    {
                        e.tabs -= 1;
                        Tab(ref span, e.tabs);
                        e.eh.Name.AsSpan().CopyTo(span);
                        span = span.Slice(e.eh.Name.Length);
                        span[0] = '\n';
                        span = span.Slice(1);
                        Tab(ref span, e.tabs);
                        span[0] = '{';
                    };
                }

                return string.Create(len, (this, tabs), action);
            }

            public int CompareTo(EhChange other)
            {
                return Offset.CompareTo(other.Offset);
            }
        }

        private bool TryFormatIlAndEh(ref BufferStringBuilder builder, ref int charsWritten)
        {
            if (Il.IsEmpty) return true;

            List<EhChange> blocks = ParseEhBlocks();
            blocks.Add(new EhChange(null, (uint)Il.Length, 0));

            int firstBlockLen;
            // Dump first block
            if (blocks.Count == 0)
            {
                firstBlockLen = Il.Length;
            }
            else
            {
                EhChange block = blocks[0];
                firstBlockLen = (int)block.Offset;
            }

            var decompiler = new IlDecompiler(Il.Slice(0, firstBlockLen), 0, builder.Tabs);

            if (!decompiler.TryDumpIl(builder.RemainingBuffer, out int newCharsWritten)) return false;

            builder.Advance(newCharsWritten, ref charsWritten);

            for (var i = 0; i < blocks.Count - 1; i++)
            {
                EhChange change = blocks[i];
                change.BaselineTabbing = builder.Tabs;

                builder.Tabs += change.RelativeTabbing;

                var offset = (int)change.Offset;
                var nextOffset = (int)blocks[i + 1].Offset;

                builder.TryAdd(change, ref charsWritten);
                builder.TryAddNewline(ref charsWritten);

                decompiler = new IlDecompiler(Il.Slice(offset, nextOffset - offset), offset, builder.Tabs);

                if (!decompiler.TryDumpIl(builder.RemainingBuffer, out newCharsWritten)) return false;

                builder.Advance(newCharsWritten, ref charsWritten);
            }

            return true;
        }

        private List<EhChange> ParseEhBlocks()
        {
            const string tryStr = ".try";
            const string catchStr = "catch {0}";
            const string filterStr = "filter";
            const string filterHandlerStr = "";
            const string faultStr = "fault";
            const string finallyStr = "finally";

            var ehBlocks = new List<EhChange>();

            foreach (MethodDataSection section in MethodInformation.MethodDataSections)
            {
                foreach (ExceptionHandlingClause ehClause in section.ExceptionHandlingClauses)
                {
                    string handlerStr = ehClause.EhKind switch
                    {
                        EhKind.COR_ILEXCEPTION_CLAUSE_EXCEPTION => string.Format(catchStr, ehClause.ClassToken),
                        EhKind.COR_ILEXCEPTION_CLAUSE_FAULT => faultStr,
                        EhKind.COR_ILEXCEPTION_CLAUSE_FILTER => filterHandlerStr,
                        EhKind.COR_ILEXCEPTION_CLAUSE_FINALLY => finallyStr,
                        _ => string.Empty
                    };

                    ehBlocks.Add(new EhChange(tryStr, ehClause.TryOffset, +1));
                    ehBlocks.Add(new EhChange(null, ehClause.TryOffset + ehClause.TryLength, -1));
                    ehBlocks.Add(new EhChange(handlerStr, ehClause.HandlerOffset, +1));
                    ehBlocks.Add(new EhChange(null, ehClause.HandlerOffset + ehClause.HandlerLength, -1));

                    if (ehClause.IsFilter)
                    {
                        ehBlocks.Add(new EhChange(filterStr, ehClause.FilterOffset, +1));
                        ehBlocks.Add(new EhChange(null, ehClause.HandlerOffset, -1));
                    }
                }
            }

            ehBlocks.Sort();

            return ehBlocks;
        }

        private bool TryAddIfFlagPresent(ref BufferStringBuilder builder, bool flagPresent, string value, ref int charsWritten)
        {
            if (!flagPresent) return true;
            return builder.TryAddWithSpace(value, ref charsWritten);
        }
    }
}