namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// An interface representing the communication data.
    /// </summary>
    public interface IQbCommInfo
    {
        /// <summary>
        /// The telephone number.
        /// </summary>
        string Phone { get; set; }

        /// <summary>
        /// A mobile number.
        /// </summary>
        string Mobile { get; set; }

        /// <summary>
        /// A pager number.
        /// </summary>
        string Pager { get; set; }

        /// <summary>
        /// A telephone number given as an alternative to Phone.
        /// </summary>
        string AltPhone { get; set; }

        /// <summary>
        /// Fax number.
        /// </summary>
        string Fax { get; set; }

        /// <summary>
        /// E-mail address.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// The carbon copy for an E-mail address.
        /// </summary>
        string Cc { get; set; }

        /// <summary>
        /// The name of a contact person for a customer or vendor.
        /// </summary>
        string Contact { get; set; }

        /// <summary>
        /// The name of an alternate contact person for a vendor, customer, or other name entry.
        /// </summary>
        string AltContact { get; set; }
    }
}
