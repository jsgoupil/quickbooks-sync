namespace QbSync.QbXml.Objects
{
    public interface IQbIteratorResponse
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
