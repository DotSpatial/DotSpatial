// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using DotSpatial.Data;
using NUnit.Framework;

namespace DotSpatial.Controls.Tests
{
    /// <summary>
    /// Tests for the dateline crossing map.
    /// </summary>
    [TestFixture]
    internal class DatelineCrossingMapTest
    {
        /// <summary>
        /// Checks whether panning left across the date line shows two views.
        /// </summary>
        [Test]
        public void PanLeftAcrossDatelineShowsTwoViews()
        {
            DatelineCrossingMap map = new DatelineCrossingMap
            {
                Size = new Size(100, 100),
                ViewExtents = new Extent(-175, -10, -155, 10)
            };
            Assert.AreEqual(new Extent(-175, -10, -155, 10), map.ViewExtents, "Set view has not been changed for aspect ratio");
            map.MapFrame.Pan(new Point(-50, 0));
            Assert.AreEqual(new Extent(175, -10, 195, 10), map.MapFrame.ViewExtents, "main frame panned");
            Assert.AreEqual(new Extent(-185, -10, -165, 10), map.SecondaryMapFrame.ViewExtents, "secondary frame takes up the slack");
            Assert.AreEqual(new Rectangle(0, 0, 25, 100), ((MapFrame)map.MapFrame).ClipRectangle, "main frame shows on the left");
            Assert.AreEqual(new Rectangle(25, 0, 75, 100), ((MapFrame)map.SecondaryMapFrame).ClipRectangle, "secondary frame shows on the left");
        }

        /// <summary>
        /// Checks whether panning right across the date line shows two views.
        /// </summary>
        [Test]
        public void PanRightAcrossDatelineShowsTwoViews()
        {
            DatelineCrossingMap map = new DatelineCrossingMap
            {
                Size = new Size(100, 100),
                ViewExtents = new Extent(155, -10, 175, 10)
            };
            Assert.AreEqual(new Extent(155, -10, 175, 10), map.ViewExtents, "Set view has not been changed for aspect ratio");
            map.MapFrame.Pan(new Point(50, 0));
            Assert.AreEqual(new Extent(165, -10, 185, 10), map.MapFrame.ViewExtents, "main frame panned");
            Assert.AreEqual(new Extent(-195, -10, -175, 10), map.SecondaryMapFrame.ViewExtents, "secondary frame takes up the slack");
            Assert.AreEqual(new Rectangle(0, 0, 75, 100), ((MapFrame)map.MapFrame).ClipRectangle, "main frame shows on the left");
            Assert.AreEqual(new Rectangle(75, 0, 25, 100), ((MapFrame)map.SecondaryMapFrame).ClipRectangle, "secondary frame shows on the right");
        }

        /// <summary>
        /// Checks whether panning through the dateline normalizes the view.
        /// </summary>
        [Test]
        public void PanThroughDatelineNormalizesView()
        {
            DatelineCrossingMap map = new DatelineCrossingMap
            {
                Size = new Size(100, 100),
                ViewExtents = new Extent(155, -10, 175, 10)
            };
            map.MapFrame.Pan(new Point(150, 0));
            Assert.AreEqual(new Extent(-175, -10, -155, 10), map.MapFrame.ViewExtents, "main frame panned into west");
            Assert.AreEqual(new Rectangle(0, 0, 100, 100), ((MapFrame)map.MapFrame).ClipRectangle, "main frame shows the whole view");
        }

        /// <summary>
        /// Checks that aspect ratio is preserved on normalizing.
        /// </summary>
        [Test]
        public void NormalizePreservesAspectRatioAbove360()
        {
            var ext = new Extent(-200, -90, 200, 90);
            var extNormalized = ext.Normalised();
            Assert.AreEqual(new Extent(-180, -81, 180, 81), extNormalized);
        }

        /// <summary>
        /// Checks that the x values of a west area that does not cross the date line stay below 180.
        /// </summary>
        [Test]
        public void NormalisePutsNonCrossingWestAreaInto180()
        {
            var ext = new Extent(-220, -90, -190, 90);
            var extNormalized = ext.Normalised();
            Assert.AreEqual(new Extent(140, -90, 170, 90), extNormalized);
        }

        /// <summary>
        /// Checks that the x values of a east area that does not cross the date line stay below 180.
        /// </summary>
        [Test]
        public void NormalisePutsNonCrossingEastAreaInto180()
        {
            var ext = new Extent(190, -90, 220, 90);
            var extNormalized = ext.Normalised();
            Assert.AreEqual(new Extent(-170, -90, -140, 90), extNormalized);
        }

        /// <summary>
        /// Checks that normalize can handle strange areas.
        /// </summary>
        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void NormalizeDoesNotChokeOnStupidArea()
        {
            var ext = new Extent(-1e38, -90, -1.0001e38, 90);
            ext.Normalised();
        }

        /// <summary>
        /// Checks that normalize puts east crossing area into 0 to 360.
        /// </summary>
        [Test]
        public void NormalizePutEastCrossingAreaInto0To360()
        {
            var ext = new Extent(170, -90, 220, 90);
            var extNormalized = ext.Normalised();
            Assert.AreEqual(new Extent(170, -90, 220, 90), extNormalized);
        }

        /// <summary>
        /// Checks that normalize puts west crossing area into 0 to 360.
        /// </summary>
        [Test]
        public void NormalizePutWestCrossingAreaInto0To360()
        {
            var ext = new Extent(-220, -90, -170, 90);
            var extNormalized = ext.Normalised();
            Assert.AreEqual(new Extent(140, -90, 190, 90), extNormalized);
        }
    }
}
