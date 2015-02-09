using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class ContactType
    {
        public IdType ListID { get; set; }
        public DateTimeType TimeCreated { get; set; }
        public DateTimeType TimeModified { get; set; }
        public StrType EditSequence { get; set; }
        public StrType Contact { get; set; }
        public StrType Salutation { get; set; }
        public StrType FirstName { get; set; }
        public StrType MiddleName { get; set; }
        public StrType LastName { get; set; }
        public StrType JobTitle { get; set; }
    }
}

