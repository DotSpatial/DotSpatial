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
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in October, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This contains the shared symbolizer properties between Points, Lines and Polygons.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter)),
    Serializable]
    public class FeatureSymbolizerOld : LegendItem, IFeatureSymbolizerOld
    {
        #region Variables

        private Brush _fillBrush;
        private Color _fillColor;
        private bool _isTextured;
        private bool _isVisible;
        private string _name;
        private float _opacity;
        private ScaleMode _scaleMode;
        private SmoothingMode _smoothing;
        private string _textureFile;
        private Bitmap _textureImage;

        #endregion

        #region constructors

        /// <summary>
        /// This constructor takes on some default values, and assumes that it
        /// has no other underlying symblizer to reference.
        /// </summary>
        public FeatureSymbolizerOld()
        {
            // Use the property to also set FillColor
            _fillColor = SymbologyGlobal.RandomColor();
            _fillBrush = new SolidBrush(_fillColor);
            _opacity = 1f;
            _isVisible = true; // This is boolean and should be true by default
            _smoothing = SmoothingMode.AntiAlias;
            LegendType = LegendType.Symbol;
            base.LegendSymbolMode = SymbolMode.Symbol;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override Size GetLegendSymbolSize()
        {
            return new Size(16, 16);
        }

        /// <summary>
        /// Draws a basic symbol to the specified rectangle
        /// </summary>
        /// <param name="g">The graphics surface to draw on</param>
        /// <param name="target">The target to draw the symbol to</param>
        public virtual void Draw(Graphics g, Rectangle target)
        {
            g.FillRectangle(FillBrush, target);
            g.DrawRectangle(Pens.Black, target);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean indicating whether or not this specific feature should be drawn.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets a boolean indicating whether or not this should be drawn.")]
        public virtual bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
            }
        }

        /// <summary>
        /// Gets or sets the solid color to be used for filling the symbol.
        /// This will also change the FillBrush to a solid brush of the specified color.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the solid color to be used for filling the symbol.")]
        public virtual Color FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
                _fillBrush = new SolidBrush(value);
            }
        }

        /// <summary>
        /// Gets or sets the Brush to be used when filling this point.
        /// Setting this value will also change the FillColor property, but only if
        /// the brush is a SolidBrush.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Brush FillBrush
        {
            get { return _fillBrush; }
            set
            {
                _fillBrush = value;
                if (value is SolidBrush)
                {
                    _fillColor = ((SolidBrush)value).Color;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to use the specified TextureImage during drawing
        /// </summary>
        [Category("Appearance"), Description("Gets or sets whether to use the specified TextureImage during drawing")]
        public virtual bool IsTextured
        {
            get { return _isTextured; }
            set { _isTextured = value; }
        }

        /// <summary>
        /// Gets or sets a string name to help identify this Symbolizer
        /// </summary>
        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets a float value from 0 to 1 where 0 is completely transparent
        /// and 1 is completely solid.  Setting an alpha of a specific feature, like
        /// FillColor, to something less than 255 will control that feature's transparency
        /// without affecting the others.  The final transparency of the feature will
        /// be that alpha multiplied by this value and rounded to the nearest byte.
        /// </summary>
        public virtual float Opacity
        {
            get { return _opacity; }

            set
            {
                float val = value;
                if (val > 1) val = 1;
                if (val < 0) val = 0;
                byte a = Convert.ToByte(255 * val);
                FillColor = Color.FromArgb(a, FillColor.R, FillColor.G, FillColor.B);
                _opacity = value;
            }
        }

        /// <summary>
        /// Gets or Sets a ScaleModes enumeration that determines whether non-coordinate drawing
        /// properties like width or size use pixels or world coordinates.  If pixels are
        /// specified, a back transform is used to approximate pixel sizes.
        /// </summary>
        public virtual ScaleMode ScaleMode
        {
            get { return _scaleMode; }
            set { _scaleMode = value; }
        }

        /// <summary>
        /// Gets or sets the smoothing mode to use that controls advanced features like
        /// anti-aliasing.  By default this is set to antialias.
        /// </summary>
        public SmoothingMode Smoothing
        {
            get { return _smoothing; }
            set { _smoothing = value; }
        }

        /// <summary>
        /// Gets or sets the string TextureFile to define the fill texture
        /// </summary>
        [Category("Appearance"),
            //Editor(typeof(Forms.OpenFileEditor), typeof(UITypeEditor)),
         Description("Gets or sets the string TextureFile to define the fill texture")]
        public string TextureFile
        {
            get { return _textureFile; }
            set
            {
                _textureFile = value;
                if (_textureFile != null && File.Exists(_textureFile))
                {
                    TextureImage = (Bitmap)Image.FromFile(_textureFile);
                }
            }
        }

        /// <summary>
        /// Gets or sets the actual bitmap to use for the texture.
        /// </summary>
        public Bitmap TextureImage
        {
            get { return _textureImage; }
            set
            {
                _textureImage = value;
                _isTextured = _textureImage != null;
            }
        }

        #endregion

        #region IFeatureSymbolizerOld Members

        /// <summary>
        /// Occurs in response to the legend symbol being painted
        /// </summary>
        /// <param name="g">The Graphics surface to draw on</param>
        /// <param name="box">The box to draw to</param>
        public override void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            SolidBrush b = new SolidBrush(FillColor);
            g.FillRectangle(b, box);
            g.DrawRectangle(Pens.Black, box);
            b.Dispose();
        }

        #endregion
    }
}