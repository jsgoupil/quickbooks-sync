using QbSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QbXml.Objects
{
    public class AdditionalNote
    {
        public IdType NoteID { get; set; }
        public DateTimeType Date { get; set; }
        public StrType Note { get; set; }
    }
}