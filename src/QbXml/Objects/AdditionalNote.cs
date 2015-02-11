using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class AdditionalNoteRet
    {
        public IdType NoteID { get; set; }
        public DateTimeType Date { get; set; }
        public StrType Note { get; set; }
    }
}