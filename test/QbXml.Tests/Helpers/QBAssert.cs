using QBSync.QbXml.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBSync.QuickbooksDesktopSync.Tests.Helpers
{
    public class QBAssert
    {
        public static void AreEqual(IStringConvertible expected, string actual)
        {
            NUnit.Framework.Assert.AreEqual(expected.ToString(), actual);
        }

        internal static void AreEqual(string expected, IStringConvertible actual)
        {
            NUnit.Framework.Assert.AreEqual(expected, actual.ToString());
        }
    }
}
