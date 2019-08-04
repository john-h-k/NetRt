using System.Runtime.CompilerServices;

namespace System
{
    public class Object
    {
        public Object()
        {

        }

        // Let's not have a finalizer so no one else can override them :when:

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Type GetType();

        public virtual bool Equals(object obj)
        {
            return this == obj;
        }

        public virtual int GetHashCode()
        {
            return RuntimeHelpers.GetHashCode(this);
        }

        public virtual string ToString()
        {
            return GetType().ToString();
        }

        public static bool ReferenceEquals(object left, object right)
        {
            return left == right;
        }


        [Intrinsic]
        protected extern object MemberwiseClone();
    }
        
}
