using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QbSync.QbXml.Messages.Requests
{
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
    }
}
