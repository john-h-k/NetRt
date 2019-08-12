namespace NetRt.TypeLoad
{
    public readonly struct TypeRef
    {
        public TypeRef(ushort resolutionScope, string typeName, string typeNamespace)
        {
            ResolutionScope = resolutionScope;
            TypeName = typeName;
            TypeNamespace = typeNamespace;
        }

        public ushort ResolutionScope { get; }
        public string TypeName { get; }
        public string TypeNamespace { get; }
    }
}