namespace QbSync.QbXml.Objects
{
    public partial class AMTTYPE : FLOATTYPE
    {
        public AMTTYPE()
            : base()
        {
        }

        public AMTTYPE(string value)
            : base(value)
        {
        }

        public AMTTYPE(decimal value)
            : base (value)
        {
        }

        public static implicit operator AMTTYPE(decimal value)
        {
            return new AMTTYPE(value);
        }
    }
}
