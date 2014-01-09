using System.Data;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class AttributeTableTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(15)]
        [TestCase(254)]
        public void DbfTextFieldSize(byte maxLen)
        {
            var at = new AttributeTable();
            // Add Some Columns
            at.Table.Columns.Add(new DataColumn("ID", typeof(int)));
            at.Table.Columns.Add(new DataColumn("Text", typeof(string)) { MaxLength = maxLen });

            at.Table.Rows.Add(1, string.Concat(Enumerable.Repeat("t", maxLen)));

            var fileName = Path.ChangeExtension(Path.GetTempFileName(), ".dbf");
            at.SaveAs(fileName, true);
            try
            {
                var actual = new AttributeTable(fileName);
                Assert.AreEqual(at.Table.Columns["Text"].MaxLength, actual.Table.Columns["Text"].MaxLength);
                Assert.AreEqual(at.Table.Rows[0]["Text"], actual.Table.Rows[0]["Text"]);
            }
            finally
            {
                File.Delete(fileName);
            }
        }
    }
}
