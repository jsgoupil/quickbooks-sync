using QbSync.QbXml.Objects;
using QbSync.QbXml.Struct;

namespace QbSync.QbXml
{
    public class QbXmlMsgResponseWithErrorRecovery<T> : QbXmlMsgResponse<T>
    {
        public ErrorRecovery ErrorRecovery;
    }
}
