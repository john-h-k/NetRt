using NetRt.Metadata;
using NetRt.Metadata.MethodData;

namespace NetRt.JitInterfacing
{
    // System.Threading.Interlocked
    // System.Threading.Volatile
    // System.Runtime.CompilerServices.RuntimeHelpers
    // System.Runtime.CompilerServices.Unsafe [internal class]
    public class IlIntrinsicImplementations
    {
        public void GetIlIntrinsicImplementationForMethod(MethodInformation method)
        {
        }

        private void GetIlIntrinsicImplementation_Interlocked(MethodInformation method) { }
        private void GetIlIntrinsicImplementation_Volatile(MethodInformation method) { }
        private void GetIlIntrinsicImplementation_RuntimeHelpers(MethodInformation method) { }

        private void GetIlIntrinsicImplementation_Unsafe(MethodInformation method)
        {

        }
    }
}w