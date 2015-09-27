namespace QbSync.QbXml.Objects
{
    public interface IQbIteratorRequest
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
