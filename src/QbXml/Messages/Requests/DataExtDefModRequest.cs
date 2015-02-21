using QbSync.QbXml.Objects;
using QbSync.QbXml.Type;
using System.Collections.Generic;
using System.Linq;

namespace QbSync.QbXml.Messages.Requests
{
    public class DataExtDefModRequest : QbXmlObject<DataExtDefModRqType>
    {
        public GuidType OwnerID { get; set; }
        public string DataExtName { get; set; }
        public string DataExtNewName { get; set; }
        public IEnumerable<AssignToObject> AssignToObject { get; set; }
        public IEnumerable<RemoveFromObject> RemoveFromObject { get; set; }
        public BoolType DataExtListRequire { get; set; }
        public BoolType DataExtTxnRequire { get; set; }
        public string DataExtFormatString { get; set; }
        public IEnumerable<string> IncludeRetElement { get; set; }

        protected override void ProcessObj(DataExtDefModRqType obj)
        {
            base.ProcessObj(obj);

            obj.DataExtDefMod = new DataExtDefMod
            {
                OwnerID = OwnerID.ToString(),
                DataExtName = DataExtName,
                DataExtNewName = DataExtNewName,
            };

            if (AssignToObject != null)
            {
                obj.DataExtDefMod.AssignToObject = AssignToObject.ToArray();
            }

            if (RemoveFromObject != null)
            {
                obj.DataExtDefMod.RemoveFromObject = RemoveFromObject.ToArray();
            }

            if (DataExtListRequire != null)
            {
                obj.DataExtDefMod.DataExtListRequire = DataExtListRequire.ToString();
            }

            if (DataExtTxnRequire != null)
            {
                obj.DataExtDefMod.DataExtTxnRequire = DataExtTxnRequire.ToString();
            }

            if (DataExtFormatString != null)
            {
                obj.DataExtDefMod.DataExtFormatString = DataExtFormatString;
            }

            if (IncludeRetElement != null)
            {
                obj.IncludeRetElement = IncludeRetElement.ToArray();
            }
        }
    }
}
