namespace QbSync.QbXml.Type
{
    public class IdType : IStringConvertible
    {
        private string value;

        public IdType(string value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value;
        }

        public static implicit operator IdType(string value)
        {
            if (value == null)
            {
                return null;
            }

            return new IdType(value);
        }
    }
}
