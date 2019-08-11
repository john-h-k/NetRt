using System.Reflection;

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexerNameAttribute : Attribute
    {
        public IndexerNameAttribute(string indexerName)
        {
            
        }
    }
}