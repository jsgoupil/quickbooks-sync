namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// An interface indicating if the class is a request.
    /// </summary>
    public interface IQbRequest
    {
        /// <summary>
        /// The request ID.
        /// </summary>
        string requestID { get; set; }
    }
}
