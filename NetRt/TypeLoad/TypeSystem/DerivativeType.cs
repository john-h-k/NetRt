namespace NetRt.TypeLoad.TypeSystem
{
    public abstract class DerivativeType : TypeDefinition
    {
        public abstract TypeDefinition UnderlyingType { get; }
        public override bool IsObject => true;
    }
}