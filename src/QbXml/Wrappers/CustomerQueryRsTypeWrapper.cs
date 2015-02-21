using QbSync.QbXml.Messages.Responses;
using QbSync.QbXml.Objects;
using System.Linq;
using System.Xml;

namespace QbSync.QbXml.Wrappers
{
    public class CustomerQueryRsTypeWrapper : CustomerQueryRsType, QbIteratorResponse
    {
        public CustomerQueryRsTypeWrapper()
        {

        }

        public string IteratorID
        {
            get
            {
                return this.iteratorID;
            }
        }

        public int? IteratorRemainingCount
        {
            get
            {
                if (this.iteratorRemainingCount == null)
                {
                    return null;
                }

                return int.Parse(this.iteratorRemainingCount);
            }
        }
    }
}