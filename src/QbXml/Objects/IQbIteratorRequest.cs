#pragma warning disable IDE1006
namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// An interface indicating if the class is a request and it supports an iterator.
    /// </summary>
    public interface IQbIteratorRequest
    {
        /// <summary>
        /// The iterator type.
        /// </summary>
        IteratorType? iterator
        {
            get;
            set;
        }

        /// <summary>
        /// The iterator ID.
        /// </summary>
        string iteratorID
        {
            get;
            set;
        }

        /// <summary>
        /// The amount of results being returned.
        /// </summary>
        string MaxReturned
        {
            get;
            set;
        }
    }
}
#pragma warning restore IDE1006