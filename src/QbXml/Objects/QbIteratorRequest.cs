namespace QbSync.QbXml.Objects
{
    public interface QbIteratorRequest
    {
        IteratorType? iterator
        {
            get;
            set;
        }

        string iteratorID
        {
            get;
            set;
        }

        string MaxReturned
        {
            get;
            set;
        }
    }
}
