// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
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

namespace DotSpatial.Controls
{
    /// <summary>
    /// Based on Ted's original idea in https://dotspatial.codeplex.com/discussions/232535 discussion thread,
    /// a second frame is added to complement the main frame to handle dateline crossing.
    /// First the main frame is normalized so that its left edge is in range [-180..180] and its width is bound to 360 but retains its aspect ratio.
    /// After the normalization, if main frame's right edge > 180 which indicates dateline crossing, then the main frame is clipped
    /// to 180 degrees, and the secondary frame takes over, shifted to -180 degrees to the required width.
    /// </summary>
    /// <remarks>Dateline crossing map works correctly only with WGS84 datum Mercator projection.</remarks>
    public class DatelineCrossingMap : Map
    {
        private readonly MapFrame _geoSlaveMapFrame;
        private bool _viewExtentsBeingChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatelineCrossingMap"/> class.
        /// </summary>
        public DatelineCrossingMap()
        {
            _geoSlaveMapFrame = new MapFrame(this, new Extent(0, 0, 0, 0)) { Layers = MapFrame.Layers };  // give the slave frame something to draw.

            // Changing layers causes a resize event to be fired. If that event reaches the slave frame after the main frame, then it can get misaligned.
            // So, re-assign the layers to the main frame, causing the main frame to re-register its event handlers so that the main frame resize happens second.
            // Note that the fact that events are fired in order is an implementation detail of C#; if that changes, we might get misaligned frames after adding layers.
            MapFrame.Layers = MapFrame.Layers;
            _viewExtentsBeingChanged = false;
        }

        /// <summary>
        /// Gets the map frame that is used to show the second part of the map.
        /// </summary>
        public IMapFrame SecondaryMapFrame
        {
            get { return _geoSlaveMapFrame; }
        }

        /// <summary>
        /// Clips the view extents so each map frame shows what it is supposed to.
        /// </summary>
        public void ClipViewExtents()
        {
            MapFrame.ViewExtents = MapFrame.ViewExtents.Normalised();

            if (MapFrame.ViewExtents.IsCrossDateline())
            {
                double minx = MapFrame.ViewExtents.X - 360.0;
                _geoSlaveMapFrame.ViewExtents = new Extent(minx, MapFrame.ViewExtents.MinY, minx + MapFrame.ViewExtents.Width, MapFrame.ViewExtents.MaxY);
                var visibleSlaveWidth = _geoSlaveMapFrame.ViewExtents.MaxX - -180;
                int slaveScreenWidth = Convert.ToInt32(Width * visibleSlaveWidth / _geoSlaveMapFrame.ViewExtents.Width);
                ((MapFrame)MapFrame).ClipRectangle = new Rectangle(0, 0, Width - slaveScreenWidth, Height);
                _geoSlaveMapFrame.ClipRectangle = new Rectangle(Width - slaveScreenWidth, 0, slaveScreenWidth, Height);
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        protected override void OnExcludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null) return;
            mapFrame.ViewChanged -= MapFrameViewChanged;
            base.OnExcludeMapFrame(mapFrame);
        }

        /// <inheritdoc />
        protected override void OnIncludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null) return;
            mapFrame.ViewChanged += MapFrameViewChanged;
            base.OnIncludeMapFrame(mapFrame);
        }

        private void MapFrameViewChanged(object sender, ViewChangedEventArgs e)
        {
            int xDiff = e.NewView.X - e.OldView.X;
            int yDiff = e.NewView.Y - e.OldView.Y;
            int wDiff = e.NewView.Width - e.OldView.Width;
            int hDiff = e.NewView.Height - e.OldView.Height;
            Rectangle currView = _geoSlaveMapFrame.View;
            _geoSlaveMapFrame.View = new Rectangle(currView.X + xDiff, currView.Y + yDiff, currView.Width + wDiff, currView.Height + hDiff);

            // clip rectangles needs to move in opposite direction of view
            Rectangle currSlaveClipRect = _geoSlaveMapFrame.ClipRectangle;
            _geoSlaveMapFrame.ClipRectangle = new Rectangle(currSlaveClipRect.X - xDiff, currSlaveClipRect.Y - yDiff, currSlaveClipRect.Width - wDiff, currSlaveClipRect.Height - hDiff);

            var mf = (MapFrame)MapFrame;
            Rectangle currMasterClipRect = mf.ClipRectangle;
            mf.ClipRectangle = new Rectangle(currMasterClipRect.X - xDiff, currMasterClipRect.Y - yDiff, currMasterClipRect.Width - wDiff, currMasterClipRect.Height - hDiff);
        }
    }
}