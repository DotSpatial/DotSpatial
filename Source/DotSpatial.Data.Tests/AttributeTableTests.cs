using DotSpatial.Tests.Common;
using NUnit.Framework;
using System;
using System.Data;
using System.IO;
using System.Linq;

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

            var fileName = FileTools.GetTempFileName(".dbf");
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

        [Test]
        public void CanReadDataRowWithZeroDates()
        {
            const string path = @"Data\Shapefiles\DateShapefile\DateShapefile.dbf";
            var at = new AttributeTable(path);
            var dt = at.SupplyPageOfData(0, 1);
            Assert.IsNotNull(dt);
            Assert.IsNotNull(dt.Rows[0]);
            Assert.AreEqual(DBNull.Value, dt.Rows[0]["datefiled"]);
        }

        [Test]
        public void ReadNullValues()
        {
            const string path = @"Data\Shapefiles\NullValues.dbf";

            var expectedRows = new object[6][];
            expectedRows[0] = new object[] { DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value, DBNull.Value };
            expectedRows[1] = new object[] { 1, 1.0, new DateTime(2016, 1, 1), true, "foo" };
            expectedRows[2] = new object[] { 2, 2.0, new DateTime(2016, 1, 1), false, "bar" };
            expectedRows[3] = new object[] { 1, DBNull.Value, new DateTime(2016, 1, 1), true, "test" };
            expectedRows[4] = new object[] { DBNull.Value, 1.0, DBNull.Value, false, DBNull.Value };
            expectedRows[5] = new object[] { 1234567890, 123456.0987, new DateTime(2016, 12, 31), DBNull.Value, "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takima" };

            var at = new AttributeTable(path);
            var dt = at.SupplyPageOfData(0, expectedRows.Length);

            Assert.IsNotNull(dt);

            for (var iRow = 0; iRow < expectedRows.Length; ++iRow)
            {
                Assert.IsNotNull(dt.Rows[iRow]);
                var content = dt.Rows[iRow].ItemArray;
                for (var iField = 0; iField < expectedRows[iRow].Length; ++iField)
                {
                    Assert.AreEqual(expectedRows[iRow][iField], content[iField]);
                }
            }
        }
    }
}
