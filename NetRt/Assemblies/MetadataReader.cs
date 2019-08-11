using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetInterface;
using NetRt.Common;
using NetRt.Metadata;
using NetRt.TypeLoad;
using static NetRt.Assemblies.Heaps.TableHeap;
using ExceptionHandlingClause = NetRt.Metadata.ExceptionHandlingClause;

namespace NetRt.Assemblies
{
    using Rva = UInt32;

    public sealed class MetadataReader : BinaryReader
    {
        private readonly CliImage _image;
        private readonly Stream _stream;

        private TableInfo GetTableInfo(Table table) => _image.TableHeap[table];

        public MetadataReader(CliImage image, Stream stream) : base(stream)
        {
            if (image is null) throw new ArgumentNullException(nameof(image));
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            _image = image;
            _stream = stream;
        }

        public static Rva TokenToRid(Rva rva) => rva & 0x00ffffff;

        public uint RvaToFileAddress(Rva rva)
        {
            Section section = GetSectionAtVirtualAddress(rva);

            return (rva + section.PointerToRawData) - section.VirtualAddress;
        }

        private Section GetSectionAtVirtualAddress(Rva rva)
        {
            Section[] sections = _image.Sections;
            foreach (Section section in sections)
            {
                if (rva >= section.VirtualAddress && rva < section.VirtualAddress + section.SizeOfRawData)
                    return section;
            }

            throw new ArgumentOutOfRangeException(nameof(rva));
        }

        public void GotoTable(Table table)
        {
            TableInfo tableInfo = _image.TableHeap[table];

            _stream.Position = TableStart + tableInfo.Offset;
        }

        private uint TableStart => _image.TableHeapOffset + _image.MetadataSection.PointerToRawData;

        public void GotoRva(Rva rva)
        {
            _stream.Position = RvaToFileAddress(rva);
        }

        public void GotoTable(Table table, Rva row)
        {
            TableInfo tableInfo = _image.TableHeap[table];

            _stream.Position = TableStart + (tableInfo.Offset + (tableInfo.RowSize * (row - 1)));
        }

        private uint ReadStrIndex()
        {
            Debug.Assert(_image.StringHeap.IndexSize == 4 || _image.StringHeap.IndexSize == 2);
            return _image.StringHeap.IndexSize == 4 ? _stream.Read<uint>() : _stream.Read<ushort>();
        }

        private uint ReadBlobIndex()
        {
            Debug.Assert(_image.BlobHeap.IndexSize == 4 || _image.BlobHeap.IndexSize == 2);
            return _image.BlobHeap.IndexSize == 4 ? _stream.Read<uint>() : _stream.Read<ushort>();
        }

        private uint ReadGuidIndex()
        {
            Debug.Assert(_image.GuidHeap.IndexSize == 4 || _image.GuidHeap.IndexSize == 2);
            return _image.GuidHeap.IndexSize == 4 ? _stream.Read<uint>() : _stream.Read<ushort>();
        }

        public uint LastField(TypeDef type)
        {
            if (type.TypeIndex == _image.TableHeap[Table.TypeDef].Length)
                return _image.TableHeap[Table.Field].Length;

            return ReadTypeDef(type.TypeIndex + 1U).FieldList - 1U;
        }

        public uint LastMethod(TypeDef type)
        {
            if (type.TypeIndex == _image.TableHeap[Table.TypeDef].Length)
                return _image.TableHeap[Table.Method].Length;

            return ReadTypeDef(type.TypeIndex + 1U).MethodList - 1U;
        }

        public MethodDef ReadMethodDef(Rva methodToken)
        {
            GotoTable(Table.Method, TokenToRid(methodToken));

            var rva = _stream.Read<Rva>();
            var implFlags = (MethodImplOptions)_stream.Read<ushort>();
            var flags = (MethodAttributes)_stream.Read<ushort>();
            uint nameIndex = ReadStrIndex();
            string name = _image.StringHeap.GetString(nameIndex);
            uint signature = ReadBlobIndex();
            var paramList = _stream.Read<ushort>();

            return new MethodDef(rva, implFlags, flags, name, signature, paramList);
        }

        public TypeDef ReadTypeDef(Rva typeToken)
        {
            uint rid = TokenToRid(typeToken);
            GotoTable(Table.TypeDef, rid);

            var flags = (TypeAttributes)_stream.Read<uint>();
            string typeName = _image.StringHeap.GetString(ReadStrIndex());
            string typeNamespace = _image.StringHeap.GetString(ReadStrIndex());
            var extends = _stream.Read<ushort>();
            var fieldList = _stream.Read<ushort>();
            var methodList = _stream.Read<ushort>();

            return new TypeDef(flags, typeName, typeNamespace, extends, fieldList, methodList, rid);
        }

        public TypeRef ReadTypeRef(Rva refToken)
        {
            GotoTable(Table.TypeRef, TokenToRid(refToken));

            var resolutionScope = _stream.Read<ushort>();
            string typeName = _image.StringHeap.GetString(ReadStrIndex());
            string typeNamespace = _image.StringHeap.GetString(ReadStrIndex());

            return new TypeRef(resolutionScope, typeName, typeNamespace);
        }

        public uint Read3Bytes()
        {
            Span<byte> buff = stackalloc byte[4];
            Read(buff.Slice(0, 3));
            return MemoryMarshal.Read<uint>(buff);
        }

        public T Read<T>() where T : unmanaged
        {
            return _stream.Read<T>();
        }

        // TODO cache methods by RVA etc

        public MethodInformation ReadMethod(MethodDef methodToken)
        {
            GotoRva(methodToken.Rva);
            MethodHeader header = ReadMethodHeader();

            int size = (int)header.CodeSize;
            IMemoryOwner<byte> il = MemoryPool<byte>.Shared.Rent(size);
            Read(il.Memory.Span.Slice(0, size));
            _stream.Position = (_stream.Position + 3) & ~3;

            var sections = new List<MethodDataSection>();
            if (header.Flags.HasFlag(MethodHeaderFlags.CorILMethod_MoreSects))
            {
                while (true)
                {
                    MethodDataSection section = ReadMethodDataSection();
                    sections.Add(section);
                    if (section.IsFinalSection) break;
                }
            }


            return new MethodInformation(methodToken, header, il, sections.ToArray());
        }

        private MethodHeader ReadMethodHeader()
        {
            byte header = ReadByte();

            const byte flagsMask = 0b_0000_0011;
            const byte sizeMask = 0b_1111_1100;

            var flags = (MethodHeaderFlags)(header & flagsMask);
            if (flags == MethodHeaderFlags.CorILMethod_TinyFormat)
            {
                uint size = (uint)header & sizeMask;
                return new MethodHeader(flags, size);
            }
            else
            {
                _stream.Position--;
                var flagsAndSize = Read<ushort>();
                const ushort fatFlagsMask = 0b_0000_1111_1111_1111;
                const ushort fatSizeMask = 0b_1111_0000_0000_0000;

                flags = (MethodHeaderFlags)(flagsAndSize & fatFlagsMask);
                uint size = (uint)flagsAndSize & fatSizeMask;

                var maxStack = Read<ushort>();
                var codeSize = Read<uint>();
                var localVarSigTok = Read<uint>();

                return new MethodHeader(flags, (byte)(size * 3), maxStack, codeSize, localVarSigTok);
            }
        }

        private MethodDataSection ReadMethodDataSection()
        {
            var kind = Read<SectionKind>();
            bool isFat = kind.HasFlag(SectionKind.CorILMethod_Sect_FatFormat);
            uint dataSize = isFat ? Read3Bytes() : ReadByte();

            if (!isFat) _ = /* Reserved */ Read<ushort>();

            //var kindAndSize = Read<uint>();

            //const uint kindMask = 0b_0000_0000__0000_0000__0000_0000__1111_1111;
            //const uint sizeMask = ~kindMask;
            //const uint thinSizeMask = 0b_0000_0000__0000_0000__1111_1111__0000_0000;

            //var kind = (SectionKind)(kindAndSize & kindMask);
            //bool isFat = kind.HasFlag(SectionKind.CorILMethod_Sect_FatFormat);
            //uint dataSize = isFat ? kindAndSize & sizeMask : kindAndSize & thinSizeMask;

            // Can dataSize be 0? if so - bad
            uint numClauses = isFat ? (dataSize - 4) / 24 : (dataSize - 4) / 12;

            ImmutableArray<ExceptionHandlingClause> eh = ReadEhHandlingClauses(numClauses, isFat);

            return new MethodDataSection(kind, dataSize, eh);
        }

        private ImmutableArray<ExceptionHandlingClause> ReadEhHandlingClauses(uint numClauses, bool isFat)
        {
            ImmutableArray<ExceptionHandlingClause>.Builder builder =
                ImmutableArray.CreateBuilder<ExceptionHandlingClause>((int)numClauses);

            // Hoist this out of the loop as the JIT doesn't hoist it for us
            if (isFat)
            {
                for (var i = 0; i < numClauses; i++)
                {
                    builder.Add(ReadFatEhClause());
                }
            }
            else
            {
                for (var i = 0; i < numClauses; i++)
                {
                    builder.Add(ReadThinEhClause());
                }
            }

            /* Un-hoisted traditional version
             *
             *  for (var i = 0; i < numClauses; i++)
             *  {
             *      builder.Add(isFat ? ReadFatEhClause() : ReadThinEhClause());
             *  }
             *
             * but this leads to a check for every single iteration :(
             */

            return builder.MoveToImmutable();
        }

        private ExceptionHandlingClause ReadFatEhClause()
        {
            var ehKind = (EhKind)Read<uint>();
            var tryOffset = Read<uint>();
            var tryLength = Read<uint>();
            var handlerOffset = Read<uint>();
            var handlerLength = Read<uint>();
            var classTokenOrFilterOffset = Read<uint>();

            return new ExceptionHandlingClause(ehKind, tryOffset, tryLength, handlerOffset, handlerLength, classTokenOrFilterOffset);
        }

        private ExceptionHandlingClause ReadThinEhClause()
        {
            var ehKind = (EhKind)Read<ushort>();
            var tryOffset = Read<ushort>();
            var tryLength = Read<byte>();
            var handlerOffset = Read<ushort>();
            var handlerLength = Read<byte>();
            var classTokenOrFilterOffset = Read<uint>();

            return new ExceptionHandlingClause(ehKind, tryOffset, tryLength, handlerOffset, handlerLength, classTokenOrFilterOffset);
        }

        public TypeSpec ReadTypeSpec(Rva specToken)
        {
            GotoTable(Table.TypeSpec, TokenToRid(specToken));

            return new TypeSpec(ReadBlobIndex());
        }

        public Field ReadField(Rva fieldToken)
        {
            GotoTable(Table.Field, TokenToRid(fieldToken));

            var flags = (FieldAttributes)_stream.Read<ushort>();
            string name = _image.StringHeap.GetString(ReadStrIndex());
            uint signature = ReadBlobIndex();

            return new Field(flags, name, signature);
        }

        public IEnumerable<TypeDef> EnumerateTypeDefs()
        {
            for (var i = 0U; i < GetTableInfo(Table.TypeDef).Length; i++)
            {
                yield return ReadTypeDef(i + 1);
            }
        }

        public IEnumerable<TypeRef> EnumerateTypeRefs()
        {
            for (var i = 0U; i < GetTableInfo(Table.TypeRef).Length; i++)
            {
                yield return ReadTypeRef(i + 1);
            }
        }

        public IEnumerable<TypeSpec> EnumerateTypeSpecs()
        {
            for (var i = 0U; i < GetTableInfo(Table.TypeSpec).Length; i++)
            {
                yield return ReadTypeSpec(i + 1);
            }
        }

        public IEnumerable<Field> EnumerateFields(TypeDef typeDef)
        {
            for (var i = typeDef.FieldList - 1U; i < LastField(typeDef); i++)
            {
                yield return ReadField(i + 1U);
            }
        }

        public IEnumerable<MethodDef> EnumerateMethods(TypeDef typeDef)
        {
            for (var i = typeDef.MethodList - 1U; i < LastMethod(typeDef); i++)
            {
                yield return ReadMethodDef(i + 1U);
            }
        }
    }
}