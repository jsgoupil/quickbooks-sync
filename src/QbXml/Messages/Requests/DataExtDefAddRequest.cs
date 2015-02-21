using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Linq;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDefAddRequest : QbXmlObject<DataExtDefAddRqType>
    {
        public GuidType OwnerID { get; set; }
        public string DataExtName { get; set; }
        public DataExtType DataExtType { get; set; }
        public IEnumerable<AssignToObject> AssignToObject { get; set; }
        public BoolType DataExtListRequire { get; set; }
        public BoolType DataExtTxnRequire { get; set; }
        public string DataExtFormatString { get; set; }
        public IEnumerable<string> IncludeRetElement { get; set; }

        protected override void ProcessObj(DataExtDefAddRqType obj)
        {
            base.ProcessObj(obj);

            obj.DataExtDefAdd = new DataExtDefAdd
            {
                OwnerID = OwnerID.ToString(),
                DataExtName = DataExtName,
                DataExtType = DataExtType
            };

            if (AssignToObject != null)
            {
                obj.DataExtDefAdd.AssignToObject = AssignToObject.ToArray();
            }

            if (DataExtListRequire != null)
            {
                obj.DataExtDefAdd.DataExtListRequire = DataExtListRequire.ToString();
            }

            if (DataExtTxnRequire != null)
            {
                obj.DataExtDefAdd.DataExtTxnRequire = DataExtTxnRequire.ToString();
            }

            if (DataExtFormatString != null)
            {
                obj.DataExtDefAdd.DataExtFormatString = DataExtFormatString;
            }

            if (IncludeRetElement != null)
            {
                obj.IncludeRetElement = IncludeRetElement.ToArray();
            }
        }
    }
}
