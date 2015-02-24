namespace QbSync.QbXml.Objects
{
    public partial class QUANTYPE : FLOATTYPE
    {
        public QUANTYPE()
            : base()
        {
        }

        public QUANTYPE(string value)
            : base(value)
        {
        }

        public QUANTYPE(decimal value)
            : base (value)
        {
        }

        public static implicit operator QUANTYPE(decimal value)
        {
            return new QUANTYPE(value);
        }
    }
}
