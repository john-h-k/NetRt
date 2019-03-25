using System.Runtime.CompilerServices;

namespace System
{
    public class Object
    {
        public Object()
        {

        }

        ~Object()
        {

        }


        public Type GetType()
        {
            throw null;
        }

        public bool Equals(object obj)
        {
            return this == obj;
        }

        public virtual int GetHashCode()
        {
            throw null; // TODO
        }

        public virtual string ToString()
        {
            return GetType().ToString();
        }

        public static bool ReferenceEquals(object left, object right)
        {
            return left == right;
        }

        protected object MemberwiseClone()
        {
            throw null;
        }
    }
        
}
