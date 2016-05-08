using System.Linq;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Nad
{
    [TestFixture]
    internal class NadTablesTests
    {
        [Test]
        public void NonEmptyAfterInitialize()
        {
            var target = new NadTables();
            Assert.IsNotEmpty(target.Tables);

            // Also check table
            var table = target.Tables.First().Value.Value;
            Assert.IsNotNull(table);
        }
    }
}
