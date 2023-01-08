using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    [TestFixture]
    internal class DatumsHandlerTests
    {
        /// <summary>
        /// Test for NonEmptyAfterInitialize       
        /// </summary>
        [Test]
        public void NonEmptyAfterInitialize()
        {
            var target = new DatumsHandler();
            target.Initialize();
            Assert.IsTrue(target.Count > 0);
        }
    }
}
