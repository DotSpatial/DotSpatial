// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Threading;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This class contains two separate layers.  The first is the BackImage where features that don't change
    /// over time are drawn.  This way if heavy calculations are required to draw the background, you don't
    /// have to re-draw the background over and over again every time a sprite moves.  The Front image is
    /// for small sprites that change rapidly, but
    /// </summary>
    public class BackBuffer
    {
        #region Events

        /// <summary>
        /// Occurs after something changes the geographic extents.  This refers to the outer
        /// geographic extents, not the view extents.
        /// </summary>
        public event EventHandler ExtentsChanged;

        #endregion

        #region Private Variables

        readonly int _originalThreadID;

        /// <summary>
        /// The real world extents for the entire buffer
        /// </summary>
        private IEnvelope _extents;

        /// <summary>
        /// The image being shown
        /// </summary>
        private Bitmap _image;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new BackBuffer bitmap that is the specified size.
        /// </summary>
        /// <param name="width">The width of the bitmap</param>
        /// <param name="height">The height of the bitmap</param>
        public BackBuffer(int width, int height)
        {
            _image = new Bitmap(width, height);
            _extents = new Envelope();
            _originalThreadID = Thread.CurrentThread.GetHashCode();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the back buffer.
        /// </summary>
        public virtual void Clear()
        {
            Graphics g = Graphics.FromImage(_image);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, _image.Width, _image.Height));
            g.Dispose();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the geographic extents for the entire back buffer.
        /// </summary>
        public IEnvelope Extents
        {
            get { return _extents; }
            set
            {
                _extents = value;
                //System.Diagnostics.Debug.WriteLine("Buffer Extents Set.");
            }
        }

        /// <summary>
        /// Gets the graphics drawing surface for the BackBuffer
        /// </summary>
        public Graphics Graphics
        {
            get { return Graphics.FromImage(_image); }
        }

        /// <summary>
        /// Gets or sets the height of this backbuffer in pixels
        /// </summary>
        public int Height
        {
            get { return _image.Height; }
        }

        /// <summary>
        /// Gets or sets the actual Bitmap being used as the back buffer.
        /// </summary>
        public Bitmap Image
        {
            get
            {
                return _image;
            }
            set
            {
                lock (this)
                {
                    _image = value;
                }
            }
        }

        /// <summary>
        /// Boolean, true if the current thread is different from the original thread, indicating
        /// that cross-threading is taking place.
        /// </summary>
        public bool InvokeRequired
        {
            get
            {
                if (Thread.CurrentThread.GetHashCode() != _originalThreadID)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Gets or sets the width of the back buffer in pixels.  This will copy
        /// the old back buffer to a new bitmap with the new width/height
        /// </summary>
        public int Width
        {
            get { return _image.Width; }
        }

        /// <summary>
        /// Obtains a graphics object already organized into world coordinates.
        /// The client rectangle and world coordinates are used in order to determine
        /// the scale and translation of the transform necessary in the graphics object.
        /// </summary>
        public virtual Graphics WorldGraphics
        {
            get
            {
                return Graphics.FromImage(_image);

                // The following code was made obsolete by the introduction of a draw window
                //if (_extents.IsNull) return result;
                //Matrix m = new Matrix();
                //float sx = Convert.ToSingle(_extents.Width / (double)_image.Width);
                //float sy = Convert.ToSingle(_extents.Height / (double)_image.Height);
                //m.Scale(1/sx, -1/sy);
                //float dx = Convert.ToSingle(_extents.Minimum.X);
                //float dy = Convert.ToSingle(_extents.Maximum.Y);
                //m.Translate(-dx, -dy);
                //result.Transform = m;
                //RectangleF clipRect = new RectangleF(Convert.ToSingle(_extents.Minimum.X), Convert.ToSingle(_extents.Maximum.Y), Convert.ToSingle(_extents.Width), Convert.ToSingle(_extents.Height));
                ////result.Clip = new Region(clipRect);
                //return result;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="testPoint"></param>
        /// <returns></returns>
        public Coordinate PixelToProj(PointF testPoint)
        {
            Coordinate coord = new Coordinate();
            coord.X = testPoint.X * _extents.Width / _image.Width + _extents.Minimum.X;
            coord.Y = _extents.Maximum.Y - testPoint.Y * _extents.Height / _image.Height;
            return coord;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="testExtents"></param>
        /// <returns></returns>
        public Extent PixelToProj(RectangleF testExtents)
        {
            Coordinate ul = PixelToProj(new PointF(testExtents.Left, testExtents.Top));
            Coordinate lr = PixelToProj(new PointF(testExtents.Right, testExtents.Bottom));
            Extent result = new Extent(ul.X, lr.Y, lr.X, ul.Y);
            return result;
        }

        /// <summary>
        /// Calculates a system.Drawing rectangle that corresponds to the specified real world
        /// envelope if it were drawn in pixels on the background image.
        /// </summary>
        /// <param name="testExtents"></param>
        /// <returns></returns>
        public RectangleF ProjToPixel(Extent testExtents)
        {
            RectangleF result = new RectangleF(0F, 0F, _image.Width, _image.Height);
            if (_extents == null || _extents.IsNull) return result;

            PointF ul = ProjToPixel(new Coordinate(testExtents.MinX, testExtents.MaxY));
            PointF lr = ProjToPixel(new Coordinate(testExtents.MaxX, testExtents.MinY));

            return new RectangleF(ul.X, ul.Y, (lr.X - ul.X), (lr.Y - ul.Y));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public PointF ProjToPixel(Coordinate coord)
        {
            PointF pt = new PointF();
            pt.X = Convert.ToSingle(((coord.X - _extents.Minimum.X) * _image.Width / _extents.Width));
            pt.Y = Convert.ToSingle(((_extents.Maximum.Y - coord.Y) * _image.Height / _extents.Height));
            return pt;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        protected virtual void OnExtentsChanged()
        {
            if (ExtentsChanged != null)
            {
                ExtentsChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        /// <summary>
        /// The envelope bounds in geographic coordinates.
        /// </summary>
        public IEnvelope Envelope
        {
            get { return _extents; }
        }
    }
}