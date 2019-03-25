using System.Reflection;

namespace System
{

    public sealed class AttributeUsageAttribute : Attribute
    {
        public AttributeUsageAttribute(AttributeTargets validOn)
        {
            ValidOn = validOn;
        }
        internal AttributeUsageAttribute(AttributeTargets validOn, bool allowMultiple, bool inherited)
        {
            ValidOn = validOn;
            AllowMultiple = allowMultiple;
            Inherited = inherited;
        }

        public AttributeTargets ValidOn { get; }
        public bool AllowMultiple { get; set; }
        public bool Inherited { get; set; }
    }
}