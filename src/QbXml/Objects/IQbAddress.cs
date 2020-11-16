namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// An interface representing an address.
    /// </summary>
    public interface IQbAddress : IQbAddressBlock
    {
        /// <summary>
        /// The city name in an address.
        /// </summary>
        string City { get; set; }

        /// <summary>
        /// The state name in an address.
        /// </summary>
        string State { get; set; }

        /// <summary>
        /// The postal code in an address.
        /// </summary>
        string PostalCode { get; set; }

        /// <summary>
        /// The country name in an address, or, in returned Host information (HostRet or HostInfo),
        /// the country for which this edition of QuickBooks was designed. (Possible values are US, CA, UK, and AU.).
        /// </summary>
        string Country { get; set; }
    }
}
