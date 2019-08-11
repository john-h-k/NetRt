namespace System.Runtime.CompilerServices
{
    public static partial class RuntimeFeature
    {
        /// <summary>
        /// Name of the Portable PDB feature.
        /// </summary>
        public const string PortablePdb = nameof(PortablePdb);

        /// <summary>
        /// Indicates that this version of runtime supports default interface method implementations.
        /// </summary>
        public const string DefaultImplementationsOfInterfaces = nameof(DefaultImplementationsOfInterfaces);

        /// <summary>
        /// Checks whether a certain feature is supported by the Runtime.
        /// </summary>
        public static bool IsSupported(string feature)
        {
            switch (feature)
            {
                case PortablePdb:
                case DefaultImplementationsOfInterfaces:
                    return true;
                case nameof(IsDynamicCodeSupported):
                    return IsDynamicCodeSupported;
                case nameof(IsDynamicCodeCompiled):
                    return IsDynamicCodeCompiled;
            }

            return false;
        }

        public static bool IsDynamicCodeSupported => true;
        public static bool IsDynamicCodeCompiled => true;
    }
}