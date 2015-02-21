namespace QbSync.QbXml.Messages.Requests
{
    public interface IQbIteratorRequest : QbRequestWrapper
    {
        string IteratorID
        {
            get;
            set;
        }

        int? MaxReturned
        {
            get;
            set;
        }
    }
}
