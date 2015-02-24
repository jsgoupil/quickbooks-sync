using System;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    [Serializable]
    [XmlType(AnonymousType = true)]
    public enum IteratorType
    {
        Start,
        Continue,
        Stop
    }

    public class IteratorMapper
    {
        public static T Map<T>(IteratorType iteratorType)
        {
            return (T)Enum.Parse(typeof(T), iteratorType.ToString());
        }

        public static IteratorType Map(string iterator)
        {
            return (IteratorType)Enum.Parse(typeof(IteratorType), iterator);
        }
    }
}
