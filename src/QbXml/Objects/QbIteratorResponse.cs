namespace QbSync.QbXml.Objects
{
    public interface QbIteratorResponse
    {
        string iteratorID
        {
            get;
        }

        int? iteratorRemainingCount
        {
            get;
        }
    }
}
