using System.Globalization;

namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Represents an amount.
    /// </summary>
    public partial class AMTTYPE : FLOATTYPE
    {
        /// <summary>
        /// Creates an AMTTYPE class.
        /// </summary>
        public AMTTYPE()
            : base()
        {
        }

        /// <summary>
        /// Creates an AMTTYPE class and parse the value as money.
        /// </summary>
        /// <param name="value">Money.</param>
        public AMTTYPE(string value)
            : base(value)
        {
        }

        /// <summary>
        /// Creates an AMTTYPE class with a value as money.
        /// </summary>
        /// <param name="value">Money.</param>
        public AMTTYPE(decimal value)
            : base (value)
        {
        }

        /// <summary>
        /// Converts a decimal to AMTTYPE automatically.
        /// </summary>
        /// <param name="value">An AMTTYPE.</param>
        public static implicit operator AMTTYPE(decimal value)
        {
            return new AMTTYPE(value);
        }

        /// <summary>
        /// A string representation of the decimal.
        /// </summary>
        /// <returns>Decimal in F2 format.</returns>
        public override string ToString()
        {
            return _value.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}
