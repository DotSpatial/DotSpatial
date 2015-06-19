﻿// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// Peter Hammond/Jia Liang Liu |  02/20/2010        |  a map control that can be panned infinite horizentally 
//                             |                    |  and crosses dateline
// ********************************************************************************************************
using System;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Data;
using System.ComponentModel;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Based on Ted's original idea in https://dotspatial.codeplex.com/discussions/232535 discussion thread,
    /// a second frame is added to complement the main frame to handle dateline crossing. 
    /// First the main frame is normalized so that its left edge is in range [-180..180] and its width is bound to 360 but 
    /// retain its aspect ratio.
    /// After the normalization, if main frame's right edge > 180 which indicates dateline crossing, then the main frame is clipped
    /// to 180 degrees, and the secondary frame takes over, shifted to -180 degrees to the required width.
    /// </summary>
    /// <remarks>Dateline crossing map works correctly only with WGS84 datum Mercator projection.</remarks>
    //This control will no longer be visible
    [ToolboxItem(false)]
    public class DatelineCrossingMap : Map
    {
        private readonly MapFrame _geoSlaveMapFrame;
        private bool _viewExtentsBeingChanged;

        public IMapFrame SecondaryMapFrame
        {
            get { return _geoSlaveMapFrame; }
        }

        public DatelineCrossingMap()
        {
            _geoSlaveMapFrame = new MapFrame(this, new Extent(0, 0, 0, 0));

            // give the slave frame something to draw.
            _geoSlaveMapFrame.Layers = MapFrame.Layers;
            // Changing layers causes a resize event to be fired. If that event reaches the slave frame after the main frame, 
            // then it can get misaligned. So, re-assign the layers to the main frame, causing the main frame to re-register its event handlers so that the main frame resize happens second. 
            // Note that the fact that events are fired in order is an implementation detail of C#; if that changes, we might get misaligned frames after adding layers.
            MapFrame.Layers = MapFrame.Layers;
            _viewExtentsBeingChanged = false;
        }

        public void ClipViewExtents()
        {
            MapFrame.ViewExtents = MapFrame.ViewExtents.Normalised();

            if (MapFrame.ViewExtents.IsCrossDateline())
            {
                double minx = MapFrame.ViewExtents.X - 360.0;
                _geoSlaveMapFrame.ViewExtents = new Extent(minx, MapFrame.ViewExtents.MinY,
                    minx + MapFrame.ViewExtents.Width, MapFrame.ViewExtents.MaxY);
                var visible_slave_width = _geoSlaveMapFrame.ViewExtents.MaxX - (-180);
                int slave_screen_width =
                    Convert.ToInt32((double) Width*visible_slave_width/_geoSlaveMapFrame.ViewExtents.Width);
                ((MapFrame) MapFrame).ClipRectangle = new Rectangle(0, 0, Width - slave_screen_width, Height);
                _geoSlaveMapFrame.ClipRectangle = new Rectangle(Width - slave_screen_width, 0, slave_screen_width,
                    Height);
            }
        }

        protected override void OnViewExtentsChanged(object sender, ExtentArgs args)
        {
            if (!_viewExtentsBeingChanged)
            {
                _viewExtentsBeingChanged = true;
                ClipViewExtents();
                base.OnViewExtentsChanged(sender, args);
                _viewExtentsBeingChanged = false;
                Invalidate();
            }
        }

        protected override void Draw(Graphics g, PaintEventArgs e)
        {
            if (!MapFrame.ViewExtents.IsCrossDateline())
            {
                MapFrame.Draw(new PaintEventArgs(g, e.ClipRectangle));
            }
            else
            {
                MapFrame mf = (MapFrame)MapFrame;
                MapFrame.Draw(new PaintEventArgs(g, mf.ClipRectangle));
                _geoSlaveMapFrame.Draw(new PaintEventArgs(g, _geoSlaveMapFrame.ClipRectangle));
            }
        }

        protected override void OnExcludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null) return;
            mapFrame.ViewChanged -= MapFrameViewChanged;
            base.OnExcludeMapFrame(mapFrame);
        }

        protected override void OnIncludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null) return;
            mapFrame.ViewChanged += MapFrameViewChanged;
            base.OnIncludeMapFrame(mapFrame);
        }

        private void MapFrameViewChanged(object sender, ViewChangedEventArgs e)
        {
            int x_diff = e.NewView.X - e.OldView.X;
            int y_diff = e.NewView.Y - e.OldView.Y;
            int w_diff = e.NewView.Width - e.OldView.Width;
            int h_diff = e.NewView.Height - e.OldView.Height;
            Rectangle curr_view = _geoSlaveMapFrame.View;
            _geoSlaveMapFrame.View = new Rectangle(curr_view.X + x_diff, curr_view.Y + y_diff,
                curr_view.Width + w_diff, curr_view.Height + h_diff);

            // clip rectangles needs to move in opposite direction of view
            Rectangle curr_slaveClipRectangle = _geoSlaveMapFrame.ClipRectangle;
            _geoSlaveMapFrame.ClipRectangle = new Rectangle(curr_slaveClipRectangle.X - x_diff,
                curr_slaveClipRectangle.Y - y_diff,
                curr_slaveClipRectangle.Width - w_diff, curr_slaveClipRectangle.Height - h_diff);

            var mf = (MapFrame)MapFrame;
            Rectangle curr_masterClipRectangle = mf.ClipRectangle;
            mf.ClipRectangle = new Rectangle(curr_masterClipRectangle.X - x_diff, curr_masterClipRectangle.Y - y_diff,
                curr_masterClipRectangle.Width - w_diff, curr_masterClipRectangle.Height - h_diff);
        }
    }

    /// <summary>
    /// Extension of Extent class to deal with dateline crossing
    /// </summary>
    public static class DatelineCrossingExtentExtension
    {
        // Modifies the extent such that its left edge is normalised into the range [-180..180] degrees. 
        // It width remains constant unless it was originally greater than 360 degrees, 
        // in which case it is scaled to 360 degrees with and retains its aspect ratio.
        public static Extent Normalised(this Extent extent)
        {
            var new_extent = (Extent)extent.Clone();
            if (new_extent.Width > 360.0)
            {
                const double new_width = 360.0;
                double new_height = new_extent.Height * new_width / new_extent.Width;
                double x_offset = (new_extent.Width - new_width) / 2.0;
                double y_offset = (new_extent.Height - new_height) / 2.0;
                new_extent.Width = new_width;
                new_extent.Height = new_height;
                new_extent.X += x_offset;
                new_extent.Y -= y_offset; // pinned to top left.
            }

            while (new_extent.X < -180)
            {
                double x = new_extent.X + 360.0;
                if (x == new_extent.X)
                    throw new ArgumentException("Extent.X is too large for degrees to be significant");
                new_extent.X = x;
            }

            while (new_extent.X > 180)
            {
                double x = new_extent.X - 360.0;
                if (x == new_extent.X)
                    throw new ArgumentException("Extent.X is too large for degrees to be significant");
                new_extent.X = x;
            }
            return new_extent;
        }

        public static bool IsCrossDateline(this Extent extent)
        {
            return extent.MaxX > 180.0;
        }
    }
}