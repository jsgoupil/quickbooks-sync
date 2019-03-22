namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Represents a percentage. Handled as a FLOATYPE.
    /// </summary>
    public partial class PERCENTTYPE : FLOATTYPE
    {
        /// <summary>
        /// Creates a PERCENTTYPE class.
        /// </summary>
        public PERCENTTYPE()
            : base()
        {
        }

        /// <summary>
        /// Creates an PERCENTTYPE class and parse the value as a decimal.
        /// </summary>
        /// <param name="value">A decimal.</param>
        public PERCENTTYPE(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Creates an PERCENTTYPE class with a value as a decimal.
        /// </summary>
        /// <param name="value">A decimal.</param>
        public PERCENTTYPE(decimal value)
            : base (value)
        {
        }

        /// <summary>
        /// Converts a decimal to PERCENTTYPE automatically.
        /// </summary>
        /// <param name="value">A PERCENTTYPE.</param>
        public static implicit operator PERCENTTYPE(decimal value)
        {
            return new PERCENTTYPE(value);
        }
    }
}
