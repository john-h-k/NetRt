using Microsoft.VisualBasic.CompilerServices;

namespace NetJit.Tools
{
    // True if all given bools are true
    public struct AllBool
    {
        private bool _value;

        public void Add(bool b)
        {
            if (!b) _value = false;
        }

        public static AllBool operator +(AllBool l, bool r)
        {
            l.Add(r);
            return l;
        }

        public static implicit operator bool(AllBool b) => b._value;
        public static implicit operator AllBool(bool b) => new AllBool { _value = b };
    }
}