using System.Collections.Generic;
using NetRt.Assemblies;
using NetRt.Metadata;
using NetRt.Metadata.TableElements;

namespace NetRt.TypeLoad.TypeSystem
{
    public abstract class TypeInformation
    {
        public static readonly IDictionary<uint, TypeInformation> TokenToTypeInformation =
            new Dictionary<uint, TypeInformation>();

        protected TypeInformation(uint token)
        {
            TokenToTypeInformation[token] = this; // TODO is this right?
        }

        public abstract int Size { get; }
        public abstract Field[] Fields { get; }
        public abstract MethodDef[] Methods { get; }

        public abstract bool IsObject { get; } // false if ByRef/Pointer/not derived from object
    }
}