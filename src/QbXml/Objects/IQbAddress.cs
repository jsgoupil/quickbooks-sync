namespace QbSync.QbXml.Objects
{
    public interface IQbAddress : IQbAddressBlock
    {
        string City { get; set; }
        string State { get; set; }
        string PostalCode { get; set; }
        string Country { get; set; }
        string Addr1 { get; set; }
    }
}
