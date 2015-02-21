namespace QbSync.QbXml.Messages.Requests
{
    public interface QbRequestWrapper
    {
        object GetQbObject();
    }

    public interface QbRequestWrapper<T>
    {
        T GetQbObject();
    }
}
