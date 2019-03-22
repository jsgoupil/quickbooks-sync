namespace QbSync.WebConnector.Core
{
    /// <summary>
    /// Log Message Type.
    /// </summary>
    public enum LogMessageType
    {
        /// <summary>
        /// Error.
        /// </summary>
        Error = 0,

        /// <summary>
        /// Authenticate.
        /// </summary>
        Authenticate = 1,

        /// <summary>
        /// GetError.
        /// </summary>
        GetError = 2,

        /// <summary>
        /// Send.
        /// </summary>
        Send = 3,

        /// <summary>
        /// Receive.
        /// </summary>
        Receive = 4,

        /// <summary>
        /// Close.
        /// </summary>
        Close = 5
    }
}
