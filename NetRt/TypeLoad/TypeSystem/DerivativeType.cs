namespace NetRt.TypeLoad.TypeSystem
{
    public abstract class DerivativeType : TypeInformation
    {
        public abstract TypeInformation UnderlyingType { get; }
        public override bool IsObject => true;
    }
}