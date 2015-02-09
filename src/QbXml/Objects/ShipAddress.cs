using QbSync.QbXml.Type;

namespace QbSync.QbXml.Objects
{
    public class ShipAddress : Address
    {
        public StrType Name { get; set; }
        public BoolType DefaultShipTo { get; set; }
    }
}
