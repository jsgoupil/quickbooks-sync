using QBSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBSync.QbXml.Objects
{
    public class DataExt
    {
        public GuidType OwnerID { get; set; }
        public StrType DataExtName { get; set; }
        public DataType DataExtType { get; set; }
        public StrType DataExtValue { get; set; }
    }
}