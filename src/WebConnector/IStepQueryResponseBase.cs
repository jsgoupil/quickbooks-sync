namespace QbSync.WebConnector
{
    public interface IStepQueryResponseBase
    {
        /// <summary>
        /// Returns the step name.
        /// </summary>
        /// <returns>Step name.</returns>
        string Name { get; }
    }
}
