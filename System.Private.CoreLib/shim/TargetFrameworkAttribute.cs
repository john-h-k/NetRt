using System.Reflection;

namespace System.Runtime.Versioning
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class TargetFrameworkAttribute : Attribute
    {
        public TargetFrameworkAttribute(string _)
        {
            
        }
        public string FrameworkDisplayName { get; set; }
    }
}