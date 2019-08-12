using System.Reflection;

namespace NetRt.TypeLoad
{
    public readonly struct TypeDef
    {
        public TypeDef(TypeAttributes flags, string typeName, string typeNamespace, ushort extends, ushort fieldList, ushort methodList, uint typeIndex)
        {
            Flags = flags;
            TypeName = typeName;
            TypeNamespace = typeNamespace;
            Extends = extends;
            FieldList = fieldList;
            MethodList = methodList;
            TypeIndex = typeIndex;
        }

        public TypeAttributes Flags { get; }
        public string TypeName { get; }
        public string TypeNamespace { get; }
        public ushort Extends { get; }
        public ushort FieldList { get; }
        public ushort MethodList { get; }
        public uint TypeIndex { get; }
    }
}