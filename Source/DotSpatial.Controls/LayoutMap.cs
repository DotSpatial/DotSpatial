// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using DotSpatial.Data;
using GeoAPI.Geometries;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A layout control that draws the content from a map control so that it can be printed
    /// </summary>
    public class LayoutMap : LayoutElement
    {
        #region Fields

        private Bitmap _buffer;
        private Envelope _envelope;
        private bool _extentChanged = true;
        private Map _mapControl;
        private RectangleF _oldRectangle;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutMap"/> class.
        /// </summary>
        /// <param name="mapControl">The map control that generates the printable content.</param>
        /// <exception cref="ArgumentNullException">Throws if mapControl is null.</exception>
        public LayoutMap(Map mapControl)
        {
            if (mapControl == null) throw new ArgumentNullException(nameof(mapControl));

            Name = "Map";
            _mapControl = mapControl;

            Envelope viewExtentEnvelope = new Envelope(_mapControl.ViewExtents.ToEnvelope());
            if (_mapControl.ExtendBuffer)
            {
                // if ExtendBuffer, Envelope must be three times smaller
                viewExtentEnvelope.ExpandBy(-viewExtentEnvelope.Width / _mapControl.MapFrame.ExtendBufferCoeff, -viewExtentEnvelope.Height / _mapControl.MapFrame.ExtendBufferCoeff);
            }

            _envelope = viewExtentEnvelope;

            ResizeStyle = ResizeStyle.NoScaling;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic envelope to be shown by the layout.
        /// </summary>
        [Browsable(false)]
        public virtual Envelope Envelope
        {
            get
            {
                return _envelope;
            }

            set
            {
                if (value.Width / value.Height < Size.Width / Size.Height)
                {
                    double yCenter = value.MinY + (value.Height / 2.0);
                    double deltaY = (value.Width / Size.Width * Size.Height) / 2.0;
                    _envelope = new Envelope(value.MinX, value.MaxX, yCenter - deltaY, yCenter + deltaY);
                }
                else
                {
                    double xCenter = value.MinX + (value.Width / 2.0);
                    double deltaX = (value.Height / Size.Height * Size.Width) / 2.0;
                    _envelope = new Envelope(xCenter - deltaX, xCenter + deltaX, value.MinY, value.MaxY);
                }

                _extentChanged = true;
                OnThumbnailChanged();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the map control that generates the printable content.
        /// </summary>
        [Browsable(false)]
        public Map MapControl
        {
            get
            {
                return _mapControl;
            }

            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
                _mapControl = value;
            }
        }

        /// <summary>
        /// Gets or sets the scale of the map.
        /// </summary>
        [Browsable(true)]
        [Category("Map")]
        public virtual long Scale
        {
            get
            {
                if (_mapControl.Layers.Count < 1)
                    return 100000;
                if (Resizing)
                    return 100000;
                return Convert.ToInt64((UnitMeterConversion() * _envelope.Width * 39.3700787 * 100D) / Size.Width);
            }

            set
            {
                if (_mapControl.Layers.Count < 1)
                    return;

                double xtl = Envelope.MinX;
                double ytl = Envelope.MaxY;
                Envelope.Init(xtl, xtl + ((value * Size.Width) / (UnitMeterConversion() * 39.3700787 * 100D)), ytl - ((value * Size.Height) / (UnitMeterConversion() * 39.3700787 * 100D)), ytl);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">Boolean, true if the drawing is printing to an actual page</param>
        public override void Draw(Graphics g, bool printing)
        {
            if (printing == false)
            {
                if (MapControl.Layers.Count <= 0 || Convert.ToInt32(Size.Width) <= 0 || Convert.ToInt32(Size.Height) <= 0)
                    return;

                if (_buffer != null && ((_buffer.Width != Convert.ToInt32(Size.Width * 96 / 100) || _buffer.Height != Convert.ToInt32(Size.Height * 96 / 100)) || _extentChanged))
                {
                    _extentChanged = false;
                    _buffer.Dispose();
                    _buffer = null;
                }

                if (_buffer == null)
                {
                    _buffer = new Bitmap(Convert.ToInt32(Size.Width * 96 / 100), Convert.ToInt32(Size.Height * 96 / 100), PixelFormat.Format32bppArgb);
                    _buffer.SetResolution(96, 96);
                    Graphics graph = Graphics.FromImage(_buffer);
                    MapControl.Print(graph, new Rectangle(0, 0, _buffer.Width, _buffer.Height), _envelope.ToExtent());
                    graph.Dispose();
                }

                g.DrawImage(_buffer, Rectangle);
            }
            else
            {
                MapControl.Print(g, new Rectangle(Location.X, Location.Y, Convert.ToInt32(Size.Width), Convert.ToInt32(Size.Height)), _envelope.ToExtent());
            }
        }

        /// <summary>
        /// Pans the map
        /// </summary>
        /// <param name="x">The amount to pan the map in the X-axis in map coord</param>
        /// <param name="y">The amount to pan the map in the Y-axis in map coord</param>
        public virtual void PanMap(double x, double y)
        {
            Envelope = new Envelope(Envelope.MinX - x, Envelope.MaxX - x, Envelope.MinY - y, Envelope.MaxY - y);
        }

        /// <summary>
        /// Zooms the map element in by 10%
        /// </summary>
        public virtual void ZoomInMap()
        {
            double tenPerWidth = (Envelope.MaxX - Envelope.MinX) / 20;
            double tenPerHeight = (Envelope.MaxY - Envelope.MinY) / 20; // todo jany_ why uses maxy tenperwidth instead of height?
            Envelope envl = new Envelope(Envelope.MinX + tenPerWidth, Envelope.MaxX - tenPerWidth, Envelope.MinY + tenPerHeight, Envelope.MaxY - tenPerWidth); // TODO jany_ can we assign this direct or do we lose MinX etc?
            Envelope = envl;
        }

        /// <summary>
        /// Zooms the map element out by 10%
        /// </summary>
        public virtual void ZoomOutMap()
        {
            double tenPerWidth = (Envelope.MaxX - Envelope.MinX) / 20;
            double tenPerHeight = (Envelope.MaxY - Envelope.MinY) / 20; // todo jany_ why uses maxy tenperwidth instead of height?
            Envelope envl = new Envelope(Envelope.MinX - tenPerWidth, Envelope.MaxX + tenPerWidth, Envelope.MinY - tenPerHeight, Envelope.MaxY + tenPerWidth); // TODO jany_ can we assign this direct or do we lose MinX etc?
            Envelope = envl;
        }

        /// <summary>
        /// Zooms the map to the fullextent of all available layers
        /// </summary>
        public virtual void ZoomToFullExtent()
        {
            Envelope = MapControl.Extent.ToEnvelope();
        }

        /// <summary>
        /// Zooms the map to the extent of the current view
        /// </summary>
        public virtual void ZoomViewExtent()
        {
            Envelope viewExtentEnvelope = new Envelope(_mapControl.ViewExtents.ToEnvelope());
            if (_mapControl.ExtendBuffer)
            {
                // if ExtendBuffer, Envelope must be three times smaller
                viewExtentEnvelope.ExpandBy(-viewExtentEnvelope.Width / _mapControl.MapFrame.ExtendBufferCoeff, -viewExtentEnvelope.Height / _mapControl.MapFrame.ExtendBufferCoeff);
            }

            Envelope = viewExtentEnvelope;
        }

        /// <summary>
        /// Updates the size of the control
        /// </summary>
        protected override void OnSizeChanged()
        {
            if (!Resizing)
            {
                // If the size has never been set before we set the maps extent to that of the map
                if (_oldRectangle.Width == 0 && _oldRectangle.Height == 0)
                {
                    ZoomViewExtent();
                }
                else
                {
                    double dx = Envelope.Width / _oldRectangle.Width;
                    double dy = Envelope.Height / _oldRectangle.Height;

                    double xtl = Envelope.MinX;
                    double ytl = Envelope.MaxY;
                    double width = Envelope.Width + ((Rectangle.Width - _oldRectangle.Width) * dx);
                    double height = Envelope.Height + ((Rectangle.Height - _oldRectangle.Height) * dy);

                    Envelope = new Envelope(xtl, xtl + width, ytl - height, ytl); // set a new envelope so the buffer gets recreated
                }

                _oldRectangle = new RectangleF(LocationF, Size);
            }

            base.OnSizeChanged();
        }

        private double UnitMeterConversion()
        {
            if (_mapControl.Layers.Count == 0) return 1;
            if (_mapControl.Layers[0].DataSet?.Projection == null) return 1;
            if (_mapControl.Layers[0].DataSet.Projection.IsLatLon)
                return _mapControl.Layers[0].DataSet.Projection.GeographicInfo.Unit.Radians * 6354101.943;
            return _mapControl.Layers[0].DataSet.Projection.Unit.Meters;
        }

        #endregion
    }
}