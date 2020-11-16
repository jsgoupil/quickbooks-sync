namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// An interface for common properties to all QuickBooks Txn ret objects.
    /// </summary>
    public interface IQbTxnRet : IQbRet
    {
        /// <summary>
        /// QuickBooks generates a unique TxnID for each transaction that is added to QuickBooks.
        /// </summary>
        string TxnID { get; }

        /// <summary>
        /// An identifying number for this transaction.
        /// </summary>
        string TxnNumber { get; }

        /// <summary>
        /// The date of the transaction.
        /// </summary>
        DATETYPE TxnDate { get; }
    }
}