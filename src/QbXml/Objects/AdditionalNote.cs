using QBSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBSync.QbXml.Objects
{
    public class AdditionalNote
    {
        public IdType NoteID { get; set; }
        public DateTimeType Date { get; set; }
        public StrType Note { get; set; }
    }
}