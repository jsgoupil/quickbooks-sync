using System;
using System.Xml.Serialization;

namespace QbSync.QbXml.Objects
{
    /// <summary>
    /// The type of iterator.
    /// </summary>
    [Serializable]
    [XmlType(AnonymousType = true)]
    public enum IteratorType
    {
        /// <summary>
        /// Indicates we start the iterator.
        /// </summary>
        Start,

        /// <summary>
        /// Indicates we continue with the iterator.
        /// </summary>
        Continue,

        /// <summary>
        /// Indicates we stop with the iterator.
        /// </summary>
        Stop
    }
    /*
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
    */
}
