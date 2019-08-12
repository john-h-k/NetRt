using NetRt.TypeLoad.TypeSystem;

namespace NetRt.Metadata.MethodData
{
    // Combined info from a MethodDef / MemberRef method and a Param
    // Can also be a return type
    public class ParamInformation
    {
        public ParamInformation(string name, bool isReturn, int paramIndex, TypeInformation type)
        {
            Name = name;
            IsReturn = isReturn;
            ParamIndex = paramIndex;
            Type = type;
        }

        public string Name { get; }
        public bool IsReturn { get; }
        public int ParamIndex { get; }
        public TypeInformation Type { get; }
    }
}