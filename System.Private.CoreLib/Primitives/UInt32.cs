using System.Runtime.CompilerServices;

namespace System
{
    public readonly struct UInt32
    {
        private readonly uint _value;

        public override int GetHashCode()
        {
            return (int)this;
        }
    }
}