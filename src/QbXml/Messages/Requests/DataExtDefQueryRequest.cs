using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Linq;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDefQueryRequest : QbXmlObject<DataExtDefQueryRqType>
    {
        public IEnumerable<GuidType> OwnerID { get; set; }
        public IEnumerable<AssignToObject> AssignToObject { get; set; }
        public IEnumerable<string> IncludeRetElement { get; set; }

        protected override void ProcessObj(DataExtDefQueryRqType obj)
        {
            base.ProcessObj(obj);

            var items = new ItemWithoutName();
            if (OwnerID != null)
            {
                foreach (var item in OwnerID)
                {
                    items.Add(item);
                }
            }

            if (AssignToObject != null)
            {
                foreach (var item in AssignToObject)
                {
                    items.Add(item);
                }
            }

            obj.Items = items.GetItems();

            if (IncludeRetElement != null)
            {
                obj.IncludeRetElement = IncludeRetElement.ToArray();
            }
        }
    }
}
