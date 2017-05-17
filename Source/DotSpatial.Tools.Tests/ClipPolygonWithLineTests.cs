// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    /// <summary>
    /// Tests for the clip polygon with line tool.
    /// </summary>
    [TestFixture]
    public class ClipPolygonWithLineTests
    {
        #region Methods

        /// <summary>
        /// Tests that the output filename still exists after execution of the tool.
        /// </summary>
        [Test]
        public void ExcecuteDoesntLoseOutputFilename()
        {
            var target = new ClipPolygonWithLine();

            IFeatureSet fsInput1 = FeatureSet.Open(@"Data\ClipPolygonWithLineTests\polygon.shp");
            IFeatureSet fsInput2 = FeatureSet.Open(@"Data\ClipPolygonWithLineTests\line.shp");
            IFeatureSet fsOutput = new FeatureSet
            {
                Filename = FileTools.GetTempFileName(".shp")
            };

            Assert.DoesNotThrow(() => target.Execute(fsInput1, fsInput2, fsOutput, new MockProgressHandler()));
            Assert.AreNotEqual(string.IsNullOrWhiteSpace(fsOutput.Filename), true);

            FileTools.DeleteShapeFile(fsOutput.Filename);
        }

        #endregion
    }
}