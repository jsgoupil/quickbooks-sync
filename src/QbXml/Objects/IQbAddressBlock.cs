namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// An interface representing the address block.
    /// Using Addr1, Addr2, Addr3, Addr4, and Addr5 to fully specify the address.
    /// If you use this so called "address block" approach, you cannot use any other address elements, such as City, State, etc. 
    /// </summary>
    public interface IQbAddressBlock
    {
        /// <summary>
        /// The first line of an address.
        /// </summary>
        string Addr1 { get; set; }

        /// <summary>
        /// The second line of an address (if a second line is needed).
        /// </summary>
        string Addr2 { get; set; }

        /// <summary>
        /// The third line of an address (if a third line is needed).
        /// </summary>
        string Addr3 { get; set; }

        /// <summary>
        /// The fourth line of an address (if a fourth line is needed).
        /// </summary>
        string Addr4 { get; set; }

        /// <summary>
        /// The fifth line of an address (if a fifth line is needed).
        /// </summary>
        string Addr5 { get; set; }
    }
}
