using System;
using System.Drawing;
using DotSpatial.Data;
using NUnit.Framework;

// ********************************************************************************************************
// Product Name: DotSpatial.Controls.Tests.dll
// Description:  unit tests for DatelineCrossingMap
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is  Peter Hammond/Jia Liang Liu
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name                        |   Date             |         Comments
//-----------------------------|--------------------|-----------------------------------------------
// Peter Hammond/Jia Liang Liu |  02/20/2010        |  tests for DatelineCrossingMap 
//                             |                    |  
// ********************************************************************************************************

namespace DotSpatial.Controls.Tests
{ 
    [TestFixture]
    class DatelineCrossingMapTest
    {
         [Test]
         public void PanLeftAcrossDatelineShowsTwoViews()
         {
           DatelineCrossingMap map = new DatelineCrossingMap();
            map.Size = new Size(100, 100);
            map.ViewExtents = new Extent(-175, -10, -155, 10);
            Assert.AreEqual(new Extent(-175, -10, -155, 10), map.ViewExtents, "Set view has not been changed for aspect ratio");
            map.MapFrame.Pan(new Point(-50, 0));
            Assert.AreEqual(new Extent(175, -10, 195, 10), map.MapFrame.ViewExtents, "main frame panned");
            Assert.AreEqual(new Extent(-185, -10, -165, 10), map.SecondaryMapFrame.ViewExtents, "secondary frame takes up the slack");
            Assert.AreEqual(new Rectangle(0, 0, 25, 100), ((MapFrame) map.MapFrame).ClipRectangle, "main frame shows on the left");
            Assert.AreEqual(new Rectangle(25, 0, 75, 100), ((MapFrame) map.SecondaryMapFrame).ClipRectangle, "secondary frame shows on the left");
        }
 
        [Test]
        public void PanRightAcrossDatelineShowsTwoViews()
        {
           DatelineCrossingMap map = new DatelineCrossingMap();
            map.Size = new Size(100, 100);
            map.ViewExtents = new Extent(155, -10, 175, 10);
            Assert.AreEqual(new Extent(155, -10, 175, 10), map.ViewExtents, "Set view has not been changed for aspect ratio");
            map.MapFrame.Pan(new Point(50, 0));
            Assert.AreEqual(new Extent(165, -10, 185, 10), map.MapFrame.ViewExtents, "main frame panned");
           Assert.AreEqual(new Extent(-195, -10, -175, 10), map.SecondaryMapFrame.ViewExtents, "secondary frame takes up the slack");
           Assert.AreEqual(new Rectangle(0, 0, 75, 100), ((MapFrame) map.MapFrame).ClipRectangle, "main frame shows on the left");
           Assert.AreEqual(new Rectangle(75, 0, 25, 100), ((MapFrame) map.SecondaryMapFrame).ClipRectangle, "secondary frame shows on the right");
       }

       [Test]
       public void PanThroughDatelineNormalisesView()
       {
           DatelineCrossingMap map = new DatelineCrossingMap();
           map.Size = new Size(100, 100);
           map.ViewExtents = new Extent(155, -10, 175, 10);
           map.MapFrame.Pan(new Point(150, 0));
           Assert.AreEqual(new Extent(-175, -10, -155, 10), map.MapFrame.ViewExtents, "main frame panned into west");
           Assert.AreEqual(new Rectangle(0, 0, 100, 100), ((MapFrame) map.MapFrame).ClipRectangle, "main frame shows the whole view");
       }

       [Test]
       public void NormalisePreservesAspectRatioAbove360()
       {
           var ext = new Extent(-200, -90, 200, 90);
           var ext_normalized = ext.Normalised();
           Assert.AreEqual(new Extent(-180, -81, 180, 81), ext_normalized);
       }

       [Test]
       public void NormalisePutsNonCrossingWestAreaInto180()
       {
           var ext = new Extent(-220, -90, -190, 90);
           var ext_normalized = ext.Normalised();
           Assert.AreEqual(new Extent(140, -90, 170, 90), ext_normalized);
       }

       [Test]
       public void NormalisePutsNonCrossingEastAreaInto180()
       {
           var ext = new Extent(190, -90, 220, 90);
           var ext_normalized = ext.Normalised();
           Assert.AreEqual(new Extent(-170, -90, -140, 90), ext_normalized);
       }

       [Test]
       [ExpectedException (typeof(ArgumentException))]
       public void NormaliseDoesNotChokeOnStupidArea()
       {
           var ext = new Extent(-1e38, -90, -1.0001e38, 90);
           ext.Normalised();
       }

       [Test]
       public void NormalisePutEastCrossingAreaInto0To360()
       {
           var ext = new Extent(170, -90, 220, 90);
           var ext_normalized = ext.Normalised();
           Assert.AreEqual(new Extent(170, -90, 220, 90), ext_normalized);
       }

       [Test]
       public void NormalisePutWestCrossingAreaInto0To360()
       {
           var ext = new Extent(-220, -90, -170, 90);
           var ext_normalized = ext.Normalised();
           Assert.AreEqual(new Extent(140, -90, 190, 90), ext_normalized);
       }
   }
}
