namespace QbSync.QbXml.Objects
{
    public interface IQbContactInfo
    {
        string Salutation { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
        string JobTitle { get; set; }
    }
}
