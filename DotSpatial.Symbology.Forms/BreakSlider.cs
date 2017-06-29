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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/27/2009 8:23:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// BreakSlider
    /// </summary>
    public class BreakSlider : IComparable<BreakSlider>
    {
        #region Private Variables

        private ICategory _category;
        private Color _color;
        private int _count;
        private Rectangle _graphBounds;
        private double _max;
        private double _min;
        private ICategory _nextCategory;
        private ColorRange _range;
        private Color _selectColor;
        private bool _selected;
        private double _value;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of BreakSlider
        /// </summary>
        /// <param name="graphBounds">The bounds of the graph to draw in relative to the control.</param>
        /// <param name="minimum">The minimum value currently in view</param>
        /// <param name="maximum">The maximum value currently in view</param>
        /// <param name="range">The color range to connect to this slider.</param>
        public BreakSlider(Rectangle graphBounds, double minimum, double maximum, ColorRange range)
        {
            _color = Color.Blue;
            _selectColor = Color.Red;
            _graphBounds = graphBounds;
            _min = minimum;
            _max = maximum;
            _range = range;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Compares this value to the other value.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(BreakSlider other)
        {
            return _value.CompareTo(other._value);
        }

        /// <summary>
        /// This sets the values for the parental bounds, as well as the double
        /// values for the maximum and minimum values visible on the graph.
        /// </summary>
        /// <param name="graphBounds"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Setup(Rectangle graphBounds, double min, double max)
        {
            _min = min;
            _max = max;
            _graphBounds = graphBounds;
        }

        /// <summary>
        /// Causes this slider to draw itself to the specified graphics surface.
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            OnDraw(g);
        }

        /// <summary>
        /// Custom drawing
        /// </summary>
        /// <param name="g">The Graphics surface to draw to</param>
        protected virtual void OnDraw(Graphics g)
        {
            if (_value < _min || _value > _max) return;
            float pos = Position;
            RectangleF rectF = new RectangleF(pos - 1, _graphBounds.Y, 3, _graphBounds.Height);
            RectangleF topF = new RectangleF(pos - 4, _graphBounds.Y - 8, 8, 8);
            if (_selected)
            {
                LinearGradientBrush top = new LinearGradientBrush(topF, _selectColor.Lighter(.2F), _selectColor.Darker(.2F), LinearGradientMode.ForwardDiagonal);
                g.FillEllipse(top, topF);
                top.Dispose();

                LinearGradientBrush lgb = new LinearGradientBrush(rectF, _selectColor.Lighter(.2f), _selectColor.Darker(.2f), LinearGradientMode.Horizontal);
                g.FillRectangle(lgb, rectF);
                lgb.Dispose();
            }
            else
            {
                LinearGradientBrush top = new LinearGradientBrush(topF, _color.Lighter(.2F), _color.Darker(.2F), LinearGradientMode.ForwardDiagonal);
                g.FillEllipse(top, topF);
                top.Dispose();

                LinearGradientBrush lgb = new LinearGradientBrush(rectF, _color.Lighter(.2f), _color.Darker(.2f), LinearGradientMode.Horizontal);
                g.FillRectangle(lgb, rectF);
                lgb.Dispose();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color range that this break is the maximum value for.
        /// </summary>
        public ColorRange Range
        {
            get { return _range; }
            set { _range = value; }
        }

        /// <summary>
        /// Gets the bounds of the handle that extends above the graph
        /// </summary>
        public Rectangle HandleBounds
        {
            get
            {
                return new Rectangle((int)Position - 4, _graphBounds.Y - 8, 8, 8);
            }
        }

        /// <summary>
        /// Gets a bounding rectangle in coordinates relative to the parent
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position - 5, _graphBounds.Top + 5, 10, _graphBounds.Height - 5);
            }
        }

        /// <summary>
        /// Gets or sets the category that has a maximum value equal to this break.
        /// </summary>
        public ICategory Category
        {
            get { return _category; }
            set { _category = value; }
        }

        /// <summary>
        /// Gets or sets the color of this slider
        /// </summary>
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// Gets or sets the integer count representing the number of members in the class.
        /// (Technically represented left of the line.
        /// </summary>
        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        /// <summary>
        /// Gets or sets the next category, which should have a minimum corresponding to this break.
        /// </summary>
        public ICategory NextCategory
        {
            get { return _nextCategory; }
            set { _nextCategory = value; }
        }

        /// <summary>
        /// Gets or sets the position of this slider
        /// </summary>
        public float Position
        {
            get { return (float)(_graphBounds.Width * (_value - _min) / (_max - _min) + _graphBounds.X); }
            set
            {
                _value = ((value - _graphBounds.X) / _graphBounds.Width) * (_max - _min) + _min;
                _range.Range.Maximum = _value;
                if (_nextCategory != null) _nextCategory.Range.Minimum = _value;
            }
        }

        /// <summary>
        /// Gets or sets whether or not this slider is selected.
        /// </summary>
        public bool Selected
        {
            get { return _selected; }
            set { _selected = value; }
        }

        /// <summary>
        /// Gets or sets the selected color
        /// </summary>
        public Color SelectColor
        {
            get { return _selectColor; }
            set { _selectColor = value; }
        }

        /// <summary>
        /// Gets or sets the double value where this break should occur.
        /// </summary>
        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }

        #endregion
    }
}