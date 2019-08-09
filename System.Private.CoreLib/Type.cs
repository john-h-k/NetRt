namespace System
{
    public abstract class Type
    {
        public string FullName => null; // TODO

        public sealed override string ToString()
        {
            return FullName;
        }

        public static Type GetTypeFromHandle(RuntimeTypeHandle handle)
        {
            throw null;
        }
    }
}