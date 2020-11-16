namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// An interface for common properties to all QuickBooks List ret objects.
    /// </summary>
    public interface IQbListRet : IQbRet
    {
        /// <summary>
        /// Along with FullName, ListID is a way to identify a list object.
        /// </summary>
        string ListID { get; }
    }
}