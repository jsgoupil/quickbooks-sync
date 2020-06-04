namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// Interface for common properties to all QuickBooks Txn ret objects.
    /// </summary>
    public interface IQbTxnRet : IQbRet
    {
        string TxnID { get; }
        
        string TxnNumber { get; }
        
        DATETYPE TxnDate { get; }
    }
}