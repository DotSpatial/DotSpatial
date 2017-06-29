// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/26/2009 2:13:31 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// RoundedSlider
    /// </summary>
    public class RoundedHandle
    {
        #region Private Variables

        private Color _color;
        private bool _isDragging;
        private GradientSlider _parent;
        private float _position;
        private int _roundingRadius;
        private bool _visible;
        private int _width;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of RoundedSlider
        /// </summary>
        public RoundedHandle()
        {
            _width = 10;
            _roundingRadius = 4;
            _color = Color.SteelBlue;
            _visible = true;
        }

        /// <summary>
        /// Creates a new instance of a rounded handle, specifying the parent gradient slider
        /// </summary>
        /// <param name="parent"></param>
        public RoundedHandle(GradientSlider parent)
        {
            _parent = parent;
            _width = 10;
            _roundingRadius = 4;
            _color = Color.SteelBlue;
            _visible = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws this slider on the specified graphics object
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            if (_visible == false) return;
            if (_width == 0) return;
            Rectangle bounds = GetBounds();
            GraphicsPath gp = new GraphicsPath();
            gp.AddRoundedRectangle(bounds, _roundingRadius);
            LinearGradientBrush lgb = new LinearGradientBrush(bounds, Color.Lighter(.3F), Color.Darker(.3F), LinearGradientMode.ForwardDiagonal);
            g.FillPath(lgb, gp);
            lgb.Dispose();
            gp.Dispose();
        }

        /// <summary>
        /// Gets the bounds of this handle in the coordinates of the parent slider.
        /// </summary>
        public Rectangle GetBounds()
        {
            float sx = (_position - _parent.Minimum) / (_parent.Maximum - _parent.Minimum);
            int x = Convert.ToInt32(sx * (_parent.Width - _width));
            Rectangle bounds = new Rectangle(x, 0, _width, _parent.Height);
            return bounds;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating whether this is visible or not
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDragging
        {
            get { return _isDragging; }
            set { _isDragging = value; }
        }

        /// <summary>
        /// Gets or sets the parent
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GradientSlider Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        /// <summary>
        /// Gets or sets the Position
        /// </summary>
        [Description("Gets or sets the Position")]
        public float Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_parent != null)
                {
                    if (_position > _parent.Maximum) _position = _parent.Maximum;
                    if (_position < _parent.Minimum) _position = _parent.Minimum;
                }
            }
        }

        /// <summary>
        /// Gets or sets the integer width of this slider in pixels.
        /// </summary>
        [Description("Gets or sets the integer width of this slider in pixels.")]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// Gets or sets the basic color of the slider
        /// </summary>
        [Description("Gets or sets the basic color of the slider")]
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the integer describing the radius of the curves in the corners of the slider
        /// </summary>
        [Description("Gets or sets the integer describing the radius of the curves in the corners of the slider")]
        public int RoundingRadius
        {
            get { return _roundingRadius; }
            set { _roundingRadius = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that controls whether or not this handle is drawn and visible.
        /// </summary>
        public bool Visible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        #endregion
    }
}