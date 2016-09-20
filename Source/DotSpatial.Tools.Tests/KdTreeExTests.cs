using GeoAPI.Geometries;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    [TestFixture]
    internal class KdTreeExTests
    {
        [Test]
        public void MethodFindBestMatchNodeFound()
        {
            var kd = new KdTreeEx<object>();
            Assert.IsTrue(kd.MethodFindBestMatchNodeFound);
            Assert.DoesNotThrow(() => kd.Search(new Coordinate()));
        }
    }
}
