namespace QbSync.QbXml.Objects
{
    public partial class PRICETYPE : FLOATTYPE
    {
        public PRICETYPE()
            : base()
        {
        }

        public PRICETYPE(string value)
            : base(value)
        {
        }

        public PRICETYPE(decimal value)
            : base (value)
        {
        }

        public static implicit operator PRICETYPE(decimal value)
        {
            return new PRICETYPE(value);
        }
    }
}
