namespace QbSync.QbXml.Messages.Requests
{
    public class QbXmlObject<T> : QbRequestWrapper<T>, QbRequestWrapper
        where T : new()
    {
        public T GetQbObject()
        {
            var obj = new T();
            ProcessObj(obj);
            return obj;
        }

        protected virtual void ProcessObj(T obj)
        {
        }

        object QbRequestWrapper.GetQbObject()
        {
            return (this as QbRequestWrapper<T>).GetQbObject();
        }
    }
}
