namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Interface for common properties to all QuickBooks ret objects.
    /// </summary>
    public interface IQbRet
    {
        DATETIMETYPE TimeCreated { get; }
        
        DATETIMETYPE TimeModified { get; }
        
        string EditSequence { get; }
    }
}