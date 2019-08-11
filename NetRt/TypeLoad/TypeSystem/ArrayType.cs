namespace NetRt.TypeLoad.TypeSystem
{
    public abstract class ArrayType : DerivativeType
    {
        protected ArrayType(TypeInformation underlyingType, uint token) : base(token) => UnderlyingType = underlyingType;

        public override TypeInformation UnderlyingType { get; }
        public override bool IsObject => true;
    }
}