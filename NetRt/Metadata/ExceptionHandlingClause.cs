using System.Diagnostics;

namespace NetRt.Metadata
{
    public readonly struct ExceptionHandlingClause
    {
        public ExceptionHandlingClause(EhKind ehKind, uint tryOffset, uint tryLength, uint handlerOffset, uint handlerLength, uint ClassTokenOrFieldOffset)
        {
            EhKind = ehKind;
            TryOffset = tryOffset;
            TryLength = tryLength;
            HandlerOffset = handlerOffset;
            HandlerLength = handlerLength;
            _unionField = ClassTokenOrFieldOffset;
        }

        public EhKind EhKind { get; }
        public uint TryOffset { get; }
        public uint TryLength { get; }
        public uint HandlerOffset { get; }
        public uint HandlerLength { get; }

        private readonly uint _unionField;

        public uint ClassToken
        {
            get
            {
                Debug.Assert(EhKind == EhKind.COR_ILEXCEPTION_CLAUSE_EXCEPTION);
                return _unionField;
            }
        }

        public uint FilterOffset
        {
            get
            {
                Debug.Assert(EhKind == EhKind.COR_ILEXCEPTION_CLAUSE_FILTER);
                return _unionField;
            }
        }
    }
}