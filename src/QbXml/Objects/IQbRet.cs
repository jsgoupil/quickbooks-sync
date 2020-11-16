namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// An interface for common properties to all QuickBooks ret objects.
    /// </summary>
    public interface IQbRet
    {
        /// <summary>
        /// Time the object was created.
        /// </summary>
        DATETIMETYPE TimeCreated { get; }

        /// <summary>
        /// Time the object was last modified.
        /// </summary>
        DATETIMETYPE TimeModified { get; }

        /// <summary>
        /// A number that the server generates and assigns to this object.
        /// </summary>
        string EditSequence { get; }
    }
}