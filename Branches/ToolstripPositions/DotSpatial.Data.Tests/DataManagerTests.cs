using System.Windows.Forms;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class DataManagerTests
    {
        [Test]
        public void DialogReadFilter_IsCorrect()
        {
            var ofd = new OpenFileDialog();
            Assert.DoesNotThrow(() => ofd.Filter = new DataManager().DialogReadFilter);
        }

        [Test]
        public void DialogWriteFilter_IsCorrect()
        {
            var ofd = new SaveFileDialog();
            Assert.DoesNotThrow(() => ofd.Filter = new DataManager().DialogWriteFilter);
        }
    }
}
