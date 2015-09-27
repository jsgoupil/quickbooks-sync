namespace QbSync.QbXml.Objects
{
    public interface IQbResponse
    {
        string requestID { get; set; }
        string statusCode { get; set; }
        string statusMessage { get; set; }
        string statusSeverity { get; set; }
    }
}
