namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// An interface representing a person name.
    /// </summary>
    public interface IQbPersonName
    {
        /// <summary>
        /// A formal reference, such as Mr. or Dr., that precedes a name.
        /// </summary>
        string Salutation { get; set; }

        /// <summary>
        /// The first name of a customer, vendor, employee, or person on the other names list.
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// The middle name of a customer, vendor, employee, or person on the other names list.
        /// </summary>
        string MiddleName { get; set; }

        /// <summary>
        /// The last name of a customer, vendor, employee, or person on the other names list.
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// The suffix.
        /// </summary>
        string Suffix { get; set; }
    }
}
