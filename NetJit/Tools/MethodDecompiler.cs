using System;
using NetRt.Metadata;

namespace NetJit.Tools
{
    public readonly struct MethodDecompiler
    {
        public MethodInformation MethodInformation { get; }
        private readonly IlDecompiler _decompiler;

        public MethodDecompiler(MethodInformation methodInformation)
        {
            if (methodInformation is null) throw new ArgumentNullException(nameof(methodInformation));

            MethodInformation = methodInformation;
            _decompiler = new IlDecompiler(methodInformation);
        }

        public bool TryFormat(Span<char> buffer, out int charsWritten)
        {
            var builder = new BufferStringBuilder(buffer);
        }
    }
}