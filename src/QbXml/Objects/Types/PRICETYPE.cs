namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Represents a price. Handled as a FLOATYPE.
    /// </summary>
    public partial class PRICETYPE : FLOATTYPE
    {
        /// <summary>
        /// Creates a PRICETYPE class.
        /// </summary>
        public PRICETYPE()
            : base()
        {
        }

        /// <summary>
        /// Creates an PRICETYPE class and parse the value as a decimal.
        /// </summary>
        /// <param name="value">A decimal.</param>
        public PRICETYPE(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Creates an PRICETYPE class with a value as a decimal.
        /// </summary>
        /// <param name="value">A decimal.</param>
        public PRICETYPE(decimal value)
            : base (value)
        {
        }

        /// <summary>
        /// Converts a decimal to PRICETYPE automatically.
        /// </summary>
        /// <param name="value">A PRICETYPE.</param>
        public static implicit operator PRICETYPE(decimal value)
        {
            return new PRICETYPE(value);
        }
    }
}
