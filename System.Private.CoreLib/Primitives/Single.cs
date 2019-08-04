using System.Runtime.CompilerServices;

namespace System
{
    public readonly struct Single
    {
        private readonly float _value;

        public override int GetHashCode()
        { 
            return Unsafe.As<float, int>(ref Unsafe.AsRef(in this));
        }
    }
}