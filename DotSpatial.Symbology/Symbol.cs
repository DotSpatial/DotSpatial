// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 10:05:42 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A point symbolizer, is comprized of a set of symbols drawn one on top of the other.  This represents the base class
    /// for one of those symbols.  The specialized type, like CharacterSymbol, SimpleSymbol, and PictureSymbol.
    /// </summary>
    public class Symbol : Descriptor, ISymbol
    {
        const int TOP_LEFT = 0;
        const int TOP_RIGHT = 1;
        const int BOTTOM_LEFT = 2;
        const int BOTTOM_RIGHT = 3;

        #region Private Variables

        private double _angle; // The angle in degrees
        private ISymbol _innerSymbol;
        private Position2D _offset;
        private Size2D _size;
        private SymbolType _symbolType;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Symbol
        /// </summary>
        public Symbol()
        {
            _size = new Size2D(4, 4);
            _offset = new Position2D(0, 0);
        }

        /// <summary>
        /// This creates a wrapper class encapsulates one of the available subclasses for
        /// symbol, enumerating the different options.
        /// </summary>
        /// <param name="type">The type to use for this symbol.</param>
        public Symbol(SymbolType type)
        {
            SetInnerSymbol(type);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a color to represent this point.  If the point is using an image,
        /// then this color will be gray.
        /// </summary>
        /// <returns></returns>
        public virtual Color GetColor()
        {
            return Color.Gray;
        }

        /// <summary>
        /// Sets the primary color of this symbol to the specified color if possible
        /// </summary>
        /// <param name="color">The Color to assign</param>
        public virtual void SetColor(Color color)
        {
        }

        /// <summary>
        /// Only copies the shared placement aspects (Size, Offset, Angle) from the specified symbol.
        /// </summary>
        /// <param name="symbol">The symbol to copy values from.</param>
        public void CopyPlacement(ISymbol symbol)
        {
            if (_innerSymbol != null)
            {
                _innerSymbol.CopyPlacement(symbol);
                return;
            }
            _size = symbol.Size.Copy();
            _offset = symbol.Offset.Copy();
            _angle = symbol.Angle;
        }

        /// <summary>
        /// Draws this symbol to the graphics object given the symbolizer that specifies content
        /// across the entire set of scales.
        /// </summary>
        /// <param name="g">The graphics object should be adjusted so that (0, 0) is the center of the symbol.</param>
        /// <param name="scaleWidth">If this should draw in pixels, this should be 1.  Otherwise, this should be
        /// the constant that you multiply against so that drawing using geographic units will draw in pixel units.</param>
        public void Draw(Graphics g, double scaleWidth)
        {
            if (_innerSymbol != null)
            {
                _innerSymbol.Draw(g, scaleWidth);
                return;
            }
            Matrix old = g.Transform;
            Matrix adjust = g.Transform;
            float dx = (float)(scaleWidth * _offset.X);
            float dy = (float)(scaleWidth * _offset.Y);
            adjust.Translate(dx, -dy);
            adjust.Rotate((float)_angle);
            g.Transform = adjust;

            OnDraw(g, scaleWidth);

            g.Transform = old;
        }

        /// <summary>
        /// Calculates a size that would be necessary to contain the entire symbol, taking rotation and
        /// offset into account.
        /// </summary>
        /// <returns></returns>
        public Size2D GetBoundingSize()
        {
            if (_innerSymbol != null)
            {
                return _innerSymbol.GetBoundingSize();
            }

            // x and y represent the magnitude of the separation from the center
            double x = 0;
            double y = 0;

            double dx = _size.Width / 2;
            double dy = _size.Height / 2;
            Position2D[] corners = new Position2D[4];
            corners[TOP_LEFT] = new Position2D(-dx, dy);
            corners[TOP_RIGHT] = new Position2D(dx, dy);
            corners[BOTTOM_LEFT] = new Position2D(-dx, -dy);
            corners[BOTTOM_RIGHT] = new Position2D(dx, -dy);

            double radians = _angle * Math.PI / 180;

            for (int i = 0; i < 4; i++)
            {
                Position2D corner = corners[i];
                Position2D rotated = new Position2D();
                rotated.X = corner.X * Math.Cos(radians) - corner.Y * Math.Sin(radians);
                rotated.Y = corner.X * Math.Sin(radians) + corner.Y * Math.Cos(radians);

                Position2D shifted = new Position2D();
                shifted.X = rotated.X + _offset.X;
                shifted.Y = rotated.Y + _offset.Y;

                if (Math.Abs(shifted.X) > x) x = Math.Abs(shifted.X);
                if (Math.Abs(shifted.Y) > y) y = Math.Abs(shifted.Y);
            }

            // Since x and y represent a "distance" from (0, 0), the size has to be twice that.
            return new Size2D(2 * x, 2 * y);
        }

        /// <summary>
        /// Multiplies all of the linear measurements found in this Symbol by the specified value.
        /// This is especially useful for changing units.
        /// </summary>
        /// <param name="value">The double precision floating point value to scale by.</param>
        public void Scale(double value)
        {
            if (_innerSymbol != null)
            {
                _innerSymbol.Scale(value);
                return;
            }

            OnScale(value);
        }

        /// <summary>
        /// Modifies this symbol in a way that is appropriate for indicating a selected symbol.
        /// This could mean drawing a cyan outline, or changing the color to cyan.
        /// </summary>
        public virtual void Select()
        {
            if (_innerSymbol != null)
            {
                _innerSymbol.Select();
            }
            // We don't really have access to any symbology tools here
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the double precision floating point that controls the angle in degrees counter clockwise.
        /// </summary>
        [Serialize("Angle")]
        public double Angle
        {
            get
            {
                if (_innerSymbol != null)
                {
                    return _innerSymbol.Angle;
                }
                return _angle;
            }
            set
            {
                if (_innerSymbol != null)
                {
                    _innerSymbol.Angle = value;
                    return;
                }
                _angle = value;
            }
        }

        /// <summary>
        /// Gets or sets the 2D offset for this particular symbol
        /// </summary>
        [Serialize("Offset")]
        public Position2D Offset
        {
            get
            {
                if (_innerSymbol != null)
                {
                    return _innerSymbol.Offset;
                }
                return _offset;
            }
            set
            {
                if (_innerSymbol != null)
                {
                    _innerSymbol.Offset = value;
                }
                _offset = value;
            }
        }

        /// <summary>
        /// Gets or sets the size
        /// </summary>
        [Serialize("Size")]
        public Size2D Size
        {
            get
            {
                if (_innerSymbol != null)
                {
                    return _innerSymbol.Size;
                }
                return _size;
            }
            set
            {
                if (_innerSymbol != null)
                {
                    _innerSymbol.Size = value;
                }
                _size = value;
            }
        }

        /// <summary>
        /// Gets the symbol type for this symbol.
        /// </summary>
        [XmlIgnore]
        public virtual SymbolType SymbolType
        {
            get
            {
                if (_innerSymbol != null) return _innerSymbol.SymbolType;
                return _symbolType;
            }
            protected set
            {
                // If we are acting as a base class, _innerSymbol is null, so handle that case differently.
                if (_innerSymbol == null)
                {
                    _symbolType = value;
                    return;
                }
                SetInnerSymbol(value);
            }
        }

        #endregion

        #region Protected Methods

        ///// <summary>
        ///// Slightly modifies the copy process to duplicate an inner symbol if one is used.
        ///// </summary>
        ///// <param name="copy"></param>
        //protected override void OnCopy(Descriptor copy)
        //{
        //    Symbol s = copy as Symbol;
        //    if (s != null)
        //    {
        //        if (s._innerSymbol != null)
        //        {
        //            s._innerSymbol = s._innerSymbol.Copy();
        //        }
        //    }
        //    base.OnCopy(copy);
        //}

        /// <summary>
        /// Occurs during drawing.  The graphics object will already be rotated by the specified angle and
        /// adjusted according to the specified offset.  The mask will also be drawn before the point.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleSize"></param>
        protected virtual void OnDraw(Graphics g, double scaleSize)
        {
            float dx = (float)(scaleSize * _size.Width / 2);
            float dy = (float)(scaleSize * _size.Height / 2);
            g.FillRectangle(Brushes.LightBlue, new RectangleF(-dx, -dy, dx * 2, dy * 2));
        }

        /// <summary>
        /// An overrideable implementation of randomizing the values for this class.
        /// This can be useful for generating random symbols or else for unit testing.
        /// </summary>
        /// <param name="generator">The random number generator to use.</param>
        protected override void OnRandomize(Random generator)
        {
            // randomize the randomizeables like size and offset.
            base.OnRandomize(generator);

            // finish up by handling the angle, which is doesn't implement IRandomizable
            _angle = (float)(generator.NextDouble() * 360 - 180);
        }

        /// <summary>
        /// This occurs when the symbol is being instructed to scale.  The linear measurements are all
        /// multiplied by the specified value.  This allows for additional behavior to be programmed,
        /// or the original behavior to be overridden or replaced.
        /// </summary>
        /// <param name="value">The double precision value to scale by.</param>
        protected virtual void OnScale(double value)
        {
            _size.Width = _size.Width * value;
            _size.Height = _size.Height * value;
            _offset.X = _offset.X * value;
            _offset.Y = _offset.Y * value;
        }

        #endregion

        #region Private Functions

        private void SetInnerSymbol(SymbolType type)
        {
            ISymbol newSymbol = null;
            // If this class is acting as a wrapper class, then it should update the internal IStroke.
            switch (type)
            {
                case SymbolType.Character:
                    newSymbol = new CharacterSymbol();
                    break;
                case SymbolType.Picture:
                    newSymbol = new PictureSymbol();
                    break;
                case SymbolType.Simple:
                    newSymbol = new SimpleSymbol();
                    break;
            }
            if (newSymbol != null)
            {
                if (_innerSymbol != null) newSymbol.CopyPlacement(_innerSymbol);
            }
            _innerSymbol = newSymbol;
        }

        #endregion
    }
}