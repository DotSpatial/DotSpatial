using System;
using NUnit.Framework;

namespace DotSpatial.Controls.Tests
{
    [TestFixture]
    class LayoutMapTests
    {
        [Test]
        public void CtorExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => new LayoutMap(null));
        }
    }
}
