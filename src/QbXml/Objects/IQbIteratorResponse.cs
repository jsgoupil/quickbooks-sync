namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// An interface indicating if the class is a response and it supports an iterator.
    /// </summary>
    public interface IQbIteratorResponse
    {
        /// <summary>
        /// The iterator ID.
        /// </summary>
        string iteratorID
        {
            get;
        }

        /// <summary>
        /// The amount remaining in the iterator.
        /// </summary>
        int? iteratorRemainingCount
        {
            get;
        }
    }
}
