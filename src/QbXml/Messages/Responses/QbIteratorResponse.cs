namespace QbSync.QbXml.Messages.Responses
{
    public interface QbIteratorResponse
    {
        string IteratorID
        {
            get;
        }

        int? IteratorRemainingCount
        {
            get;
        }
    }
}
