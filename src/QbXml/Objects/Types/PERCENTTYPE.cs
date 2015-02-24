namespace QbSync.QbXml.Objects
{
    public partial class PERCENTTYPE : FLOATTYPE
    {
        public PERCENTTYPE()
            : base()
        {
        }

        public PERCENTTYPE(string value)
            : base(value)
        {
        }

        public PERCENTTYPE(decimal value)
            : base (value)
        {
        }

        public static implicit operator PERCENTTYPE(decimal value)
        {
            return new PERCENTTYPE(value);
        }
    }
}
