namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Represents a quantity. Handled as a FLOATYPE.
    /// </summary>
    public partial class QUANTYPE : FLOATTYPE
    {
        /// <summary>
        /// Creates a QUANTYPE class.
        /// </summary>
        public QUANTYPE()
            : base()
        {
        }

        /// <summary>
        /// Creates an QUANTYPE class and parse the value as a decimal.
        /// </summary>
        /// <param name="value">A decimal.</param>
        public QUANTYPE(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Creates an QUANTYPE class with a value as a decimal.
        /// </summary>
        /// <param name="value">A decimal.</param>
        public QUANTYPE(decimal value)
            : base (value)
        {
        }

        /// <summary>
        /// Converts a decimal to QUANTYPE automatically.
        /// </summary>
        /// <param name="value">A QUANTYPE.</param>
        public static implicit operator QUANTYPE(decimal value)
        {
            return new QUANTYPE(value);
        }
    }
}
