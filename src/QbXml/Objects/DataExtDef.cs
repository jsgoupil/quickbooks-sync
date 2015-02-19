using QbSync.QbXml.Struct;
using QbSync.QbXml.Type;
using System.Collections.Generic;

namespace QbSync.QbXml.Objects
{
    public class DataExtDef
    {
        public GuidType OwnerID { get; set; }
        public IntType DataExtID { get; set; }
        public StrType DataExtName { get; set; }
        public DataExtType DataExtType { get; set; }
        public IEnumerable<AssignToObject> AssignToObject { get; set; }
        public BoolType DataExtListRequire { get; set; }
        public BoolType DataExtTxnRequire { get; set; }
        public StrType DataExtFormatString { get; set; }
    }
}
