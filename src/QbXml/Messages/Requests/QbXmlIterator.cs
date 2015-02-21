namespace QbSync.QbXml.Messages.Requests
{
    public class QbXmlIterator<T> : QbXmlObject<T>, IQbIteratorRequest
        where T : new()
    {
        public string IteratorID
        {
            get;
            set;
        }

        public int? MaxReturned
        {
            get;
            set;
        }

        public IteratorType? Iterator
        {
            get;
            set;
        }

        protected override void ProcessObj(T obj)
        {
            base.ProcessObj(obj);

            if (string.IsNullOrEmpty(IteratorID))
            {
                Iterator = IteratorType.Start;
            }
            else
            {
                Iterator = IteratorType.Continue;
            }
        }
    }
}
