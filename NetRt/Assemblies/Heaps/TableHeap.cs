using System;

namespace NetRt.Assemblies.Heaps
{
    using CodedIndex = TableHeap.CodedIndex;

    public class TableHeap : Heap
    {
        public const int TableCount = 58;

        public TableHeap(Memory<byte> data) : base(data)
        {
        }

        public long ValidTables { get; set; }
        public long Sorted { get; set; }

        public TableInfo[] Tables { get; } = new TableInfo[TableCount];

        public TableInfo this[Table table] => Tables[(byte)table];

        public bool HasTable(Table table) => (ValidTables & (1L << (byte)table)) != 0;

        public enum Table : byte
        {
            Module = 0x00,
            TypeRef = 0x01,
            TypeDef = 0x02,
            FieldPtr = 0x03,
            Field = 0x04,
            MethodPtr = 0x05,
            Method = 0x06,
            ParamPtr = 0x07,
            Param = 0x08,
            InterfaceImpl = 0x09,
            MemberRef = 0x0a,
            Constant = 0x0b,
            CustomAttribute = 0x0c,
            FieldMarshal = 0x0d,
            DeclSecurity = 0x0e,
            ClassLayout = 0x0f,
            FieldLayout = 0x10,
            StandAloneSig = 0x11,
            EventMap = 0x12,
            EventPtr = 0x13,
            Event = 0x14,
            PropertyMap = 0x15,
            PropertyPtr = 0x16,
            Property = 0x17,
            MethodSemantics = 0x18,
            MethodImpl = 0x19,
            ModuleRef = 0x1a,
            TypeSpec = 0x1b,
            ImplMap = 0x1c,
            FieldRva = 0x1d,
            EncLog = 0x1e,
            EncMap = 0x1f,
            Assembly = 0x20,
            AssemblyProcessor = 0x21,
            AssemblyOS = 0x22,
            AssemblyRef = 0x23,
            AssemblyRefProcessor = 0x24,
            AssemblyRefOS = 0x25,
            File = 0x26,
            ExportedType = 0x27,
            ManifestResource = 0x28,
            NestedClass = 0x29,
            GenericParam = 0x2a,
            MethodSpec = 0x2b,
            GenericParamConstraint = 0x2c,

            Document = 0x30,
            MethodDebugInformation = 0x31,
            LocalScope = 0x32,
            LocalVariable = 0x33,
            LocalConstant = 0x34,
            ImportScope = 0x35,
            StateMachineMethod = 0x36,
            CustomDebugInformation = 0x37,
        }

        public struct TableInfo
        {
            public uint Offset { get; set; }
            public uint Length { get; set; }
            public uint RowSize { get; set; }

            public bool IsLarge => Length > ushort.MaxValue;
        }

        public enum CodedIndex
        {
            TypeDefOrRef,
            HasConstant,
            HasCustomAttribute,
            HasFieldMarshal,
            HasDeclSecurity,
            MemberRefParent,
            HasSemantics,
            MethodDefOrRef,
            MemberForwarded,
            Implementation,
            CustomAttributeType,
            ResolutionScope,
            TypeOrMethodDef,
            HasCustomDebugInformation,
        }
    }

    public static class CodedIndexExtensions
    {
        public static int GetSize(this CodedIndex self, Func<TableHeap.Table, int> counter)
        {
            int bits;
            TableHeap.Table[] tables;

            switch (self)
            {
                case CodedIndex.TypeDefOrRef:
                    bits = 2;
                    tables = new[] { TableHeap.Table.TypeDef, TableHeap.Table.TypeRef, TableHeap.Table.TypeSpec };
                    break;
                case CodedIndex.HasConstant:
                    bits = 2;
                    tables = new[] { TableHeap.Table.Field, TableHeap.Table.Param, TableHeap.Table.Property };
                    break;
                case CodedIndex.HasCustomAttribute:
                    bits = 5;
                    tables = new[] {
                    TableHeap.Table.Method, TableHeap.Table.Field, TableHeap.Table.TypeRef, TableHeap.Table.TypeDef, TableHeap.Table.Param, TableHeap.Table.InterfaceImpl, TableHeap.Table.MemberRef,
                    TableHeap.Table.Module, TableHeap.Table.DeclSecurity, TableHeap.Table.Property, TableHeap.Table.Event, TableHeap.Table.StandAloneSig, TableHeap.Table.ModuleRef,
                    TableHeap.Table.TypeSpec, TableHeap.Table.Assembly, TableHeap.Table.AssemblyRef, TableHeap.Table.File, TableHeap.Table.ExportedType,
                    TableHeap.Table.ManifestResource, TableHeap.Table.GenericParam, TableHeap.Table.GenericParamConstraint, TableHeap.Table.MethodSpec,
                };
                    break;
                case CodedIndex.HasFieldMarshal:
                    bits = 1;
                    tables = new[] { TableHeap.Table.Field, TableHeap.Table.Param };
                    break;
                case CodedIndex.HasDeclSecurity:
                    bits = 2;
                    tables = new[] { TableHeap.Table.TypeDef, TableHeap.Table.Method, TableHeap.Table.Assembly };
                    break;
                case CodedIndex.MemberRefParent:
                    bits = 3;
                    tables = new[] { TableHeap.Table.TypeDef, TableHeap.Table.TypeRef, TableHeap.Table.ModuleRef, TableHeap.Table.Method, TableHeap.Table.TypeSpec };
                    break;
                case CodedIndex.HasSemantics:
                    bits = 1;
                    tables = new[] { TableHeap.Table.Event, TableHeap.Table.Property };
                    break;
                case CodedIndex.MethodDefOrRef:
                    bits = 1;
                    tables = new[] { TableHeap.Table.Method, TableHeap.Table.MemberRef };
                    break;
                case CodedIndex.MemberForwarded:
                    bits = 1;
                    tables = new[] { TableHeap.Table.Field, TableHeap.Table.Method };
                    break;
                case CodedIndex.Implementation:
                    bits = 2;
                    tables = new[] { TableHeap.Table.File, TableHeap.Table.AssemblyRef, TableHeap.Table.ExportedType };
                    break;
                case CodedIndex.CustomAttributeType:
                    bits = 3;
                    tables = new[] { TableHeap.Table.Method, TableHeap.Table.MemberRef };
                    break;
                case CodedIndex.ResolutionScope:
                    bits = 2;
                    tables = new[] { TableHeap.Table.Module, TableHeap.Table.ModuleRef, TableHeap.Table.AssemblyRef, TableHeap.Table.TypeRef };
                    break;
                case CodedIndex.TypeOrMethodDef:
                    bits = 1;
                    tables = new[] { TableHeap.Table.TypeDef, TableHeap.Table.Method };
                    break;
                case CodedIndex.HasCustomDebugInformation:
                    bits = 5;
                    tables = new[] {
                    TableHeap.Table.Method, TableHeap.Table.Field, TableHeap.Table.TypeRef, TableHeap.Table.TypeDef, TableHeap.Table.Param, TableHeap.Table.InterfaceImpl, TableHeap.Table.MemberRef,
                    TableHeap.Table.Module, TableHeap.Table.DeclSecurity, TableHeap.Table.Property, TableHeap.Table.Event, TableHeap.Table.StandAloneSig, TableHeap.Table.ModuleRef,
                    TableHeap.Table.TypeSpec, TableHeap.Table.Assembly, TableHeap.Table.AssemblyRef, TableHeap.Table.File, TableHeap.Table.ExportedType,
                    TableHeap.Table.ManifestResource, TableHeap.Table.GenericParam, TableHeap.Table.GenericParamConstraint, TableHeap.Table.MethodSpec,
                    TableHeap.Table.Document, TableHeap.Table.LocalScope, TableHeap.Table.LocalVariable, TableHeap.Table.LocalConstant, TableHeap.Table.ImportScope,
                };
                    break;
                default:
                    throw new ArgumentException();
            }

            int max = 0;

            foreach (TableHeap.Table table in tables)
            {
                max = Math.Max(counter(table), max);
            }

            return max < (1 << (16 - bits)) ? 2 : 4;
        }
    }
}