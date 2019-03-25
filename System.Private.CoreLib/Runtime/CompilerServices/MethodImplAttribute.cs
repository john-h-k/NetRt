using System.Reflection;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited = false)]
    public class MethodImplAttribute : Attribute
    {
        public MethodImplOptions Value { get; }

        public MethodImplAttribute(MethodImplOptions options)
            => Value = options;
    }
}