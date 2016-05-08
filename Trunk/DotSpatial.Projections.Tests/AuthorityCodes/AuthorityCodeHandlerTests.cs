using DotSpatial.Projections.AuthorityCodes;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.AuthorityCodes
{
    [TestFixture]
    internal class AuthorityCodeHandlerTests
    {
        [Test]
        public void DefaultInstanceContainsProjections()
        {
            Assert.IsNotNull(AuthorityCodeHandler.Instance["EPSG:2000"]);
        }
    }
}
