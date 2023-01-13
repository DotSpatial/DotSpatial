using System.Linq;
using DotSpatial.Projections.Nad;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Nad
{
    [TestFixture]
    internal class NadTablesTests
    {
        /// <summary>
        /// Test for NonEmptyAfterInitialize       
        /// </summary>
        [Test, Category("Projection")]
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
