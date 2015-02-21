using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDefDelRequest : QbXmlObject<DataExtDefDelRqType>
    {
        public GuidType OwnerID { get; set; }
        public string DataExtName { get; set; }

        protected override void ProcessObj(DataExtDefDelRqType obj)
        {
            base.ProcessObj(obj);

            obj.OwnerID = OwnerID.ToString();
            obj.DataExtName = DataExtName;
        }
    }
}
