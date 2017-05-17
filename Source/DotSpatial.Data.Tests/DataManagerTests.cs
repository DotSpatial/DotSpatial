// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// Tests for DataManager.
    /// </summary>
    [TestFixture]
    internal class DataManagerTests
    {
        /// <summary>
        /// Checks that the dialog read filter is correct.
        /// </summary>
        [Test]
        public void DialogReadFilterIsCorrect()
        {
            var ofd = new OpenFileDialog();
            Assert.DoesNotThrow(() => ofd.Filter = new DataManager().DialogReadFilter);
        }

        /// <summary>
        /// Checks that the dialog write filter is correct.
        /// </summary>
        [Test]
        public void DialogWriteFilterIsCorrect()
        {
            var ofd = new SaveFileDialog();
            Assert.DoesNotThrow(() => ofd.Filter = new DataManager().DialogWriteFilter);
        }
    }
}
