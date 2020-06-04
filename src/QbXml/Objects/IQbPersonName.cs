namespace QbSync.QbXml.Objects
{
    public interface IQbPersonName
    {
        string Salutation { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string Suffix { get; set; }
    }
}
