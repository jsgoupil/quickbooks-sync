using QbSync.QbXml.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace QbSync.QbXml
{
    public class QbXmlMsgResponse<T>
    {
        public int RequestId { get; set; }
        public int StatusCode { get; set; }
        public StatusSeverity StatusSeverity { get; set; }
        public string StatusMessage { get; set; }
        public int? IteratorRemainingCount { get; set; }
        public string IteratorID { get; set; }

        public T Object { get; set; }
    }
}
