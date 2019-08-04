using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using NetInterface;
using NetRt.Common;
using NetRt.TypeLoad;
using static NetRt.Assemblies.Heaps.TableHeap;

namespace NetRt.Assemblies
{
    using Rva = UInt32;

    public sealed class MetadataReader
    {
        private readonly CliImage _image;
        private readonly Stream _stream;

        private TableInfo GetTableInfo(Table table) => _image.TableHeap[table];

        public MetadataReader(CliImage image, Stream stream)
        {
            if (image is null) throw new ArgumentNullException(nameof(image));
            if (stream is null) throw new ArgumentNullException(nameof(stream));
            _image = image;
            _stream = stream;
        }

        public static Rva TokenToRid(Rva rva) => rva & 0x00ffffff;

        private void GotoTable(Table table)
        {
            TableInfo tableInfo = _image.TableHeap[table];

            _stream.Position = TableStart + tableInfo.Offset;
        }

        private uint TableStart => _image.TableHeapOffset + _image.MetadataSection.PointerToRawData;

        private void GotoTable(Table table, Rva row)
        {
            TableInfo tableInfo = _image.TableHeap[table];

            _stream.Position = TableStart + (tableInfo.Offset + (tableInfo.RowSize * (row - 1)));
        }

        private uint ReadStrIndex() => _image.StringHeap.IndexSize == 4 ? _stream.Read<uint>() : _stream.Read<ushort>();
        private uint ReadBlobIndex() => _image.BlobHeap.IndexSize == 4 ? _stream.Read<uint>() : _stream.Read<ushort>();
        private uint ReadGuidIndex() => _image.GuidHeap.IndexSize == 4 ? _stream.Read<uint>() : _stream.Read<ushort>();

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