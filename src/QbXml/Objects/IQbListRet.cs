namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Interface for common properties to all QuickBooks List ret objects.
    /// </summary>
    public interface IQbListRet : IQbRet
    {
        string ListID { get; }
    }
}