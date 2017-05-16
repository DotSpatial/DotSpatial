// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Xml.Serialization;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// CharacterSymbol
    /// </summary>
    [Serializable]
    public class CharacterSymbol : Symbol, ICharacterSymbol
    {
        #region Fields

        private char _character;
        private Color _color;
        private string _fontFamilyName;
        private FontStyle _style;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSymbol"/> class.
        /// </summary>
        public CharacterSymbol()
        {
            _character = 'A';
            _fontFamilyName = "DotSpatialSymbols";
            _color = Color.Green;
            _style = FontStyle.Regular;
            SymbolType = SymbolType.Character;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSymbol"/> class.
        /// </summary>
        /// <param name="character">The character to use for the symbol</param>
        public CharacterSymbol(char character)
        {
            _character = character;
            _fontFamilyName = "DotSpatialSymbols";
            _color = Color.Green;
            _style = FontStyle.Regular;
            SymbolType = SymbolType.Character;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSymbol"/> class.
        /// </summary>
        /// <param name="character">The character to use for the symbol</param>
        /// <param name="fontFamily">The font family for the character</param>
        public CharacterSymbol(char character, string fontFamily)
        {
            _character = character;
            _fontFamilyName = fontFamily;
            _color = Color.Green;
            _style = FontStyle.Regular;
            SymbolType = SymbolType.Character;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSymbol"/> class.
        /// </summary>
        /// <param name="character">The character to use for the symbol</param>
        /// <param name="fontFamily">The font family for the character</param>
        /// <param name="color">The color for the character</param>
        public CharacterSymbol(char character, string fontFamily, Color color)
        {
            _character = character;
            _fontFamilyName = fontFamily;
            _color = color;
            _style = FontStyle.Regular;
            SymbolType = SymbolType.Character;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterSymbol"/> class.
        /// </summary>
        /// <param name="character">The character to use for the symbol</param>
        /// <param name="fontFamily">The font family for the character</param>
        /// <param name="color">The color for the character</param>
        /// <param name="size">The size for the symbol</param>
        public CharacterSymbol(char character, string fontFamily, Color color, double size)
        {
            _character = character;
            _fontFamilyName = fontFamily;
            _color = color;
            _style = FontStyle.Regular;
            Size = new Size2D(size, size);
            SymbolType = SymbolType.Character;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unicode category for this character.
        /// </summary>
        [XmlIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets the unicode category for this character.")]
        public UnicodeCategory Category => char.GetUnicodeCategory(_character);

        /// <summary>
        /// Gets or sets the character that this represents.
        /// </summary>
        [Description("Gets or sets the character for this symbol.")]
        [Serialize("Character")]
        public char Character
        {
            get
            {
                return _character;
            }

            set
            {
                _character = value;
            }
        }

        /// <summary>
        /// Gets or sets the character set. Unicode characters consist of 2 bytes. This represents the first byte,
        /// which can be thought of as specifying a typeset.
        /// </summary>
        [XmlIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the upper unicode byte, or character set.")]
        public byte CharacterSet
        {
            get
            {
                return (byte)(_character / 256);
            }

            set
            {
                int remainder = _character % 256;
                _character = (char)(remainder + (value * 256));
            }
        }

        /// <summary>
        /// Gets or sets the byte code for the lower 256 values. This represents the
        /// specific character in a given "typeset" range.
        /// </summary>
        /// <remarks>
        /// // Editor(typeof(CharacterCodeEditor), typeof(UITypeEditor))
        /// </remarks>
        [XmlIgnore]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("Gets or sets the lower unicode byte or character ASCII code")]
        public byte Code
        {
            get
            {
                return (byte)(_character % 256);
            }

            set
            {
                int set = _character / 256;
                _character = (char)((set * 256) + value);
            }
        }

        /// <summary>
        /// Gets or sets the color
        /// </summary>
        [XmlIgnore]
        [Description("Gets or sets the color")]
        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
            }
        }

        /// <summary>
        /// Gets or sets the string font family name to use for this character set.
        /// </summary>
        /// <remarks>
        /// // Editor(typeof(FontFamilyNameEditor), typeof(UITypeEditor)),
        /// </remarks>
        [Description("Gets or sets the font family name to use when building the font.")]
        [Serialize("FontFamilyName")]
        public string FontFamilyName
        {
            get
            {
                return _fontFamilyName;
            }

            set
            {
                _fontFamilyName = value;
            }
        }

        /// <summary>
        /// Gets or sets the opacity as a floating point value ranging from 0 to 1, where
        /// 0 is fully transparent and 1 is fully opaque. This actually adjusts the alpha of the color value.
        /// </summary>
        [Description("Gets or sets the opacity as a floating point value ranging from 0 (transparent) to 1 (opaque)")]
        [Serialize("Opacity")]
        public float Opacity
        {
            get
            {
                return _color.A / 255F;
            }

            set
            {
                int alpha = (int)(value * 255);
                if (alpha > 255) alpha = 255;
                if (alpha < 0) alpha = 0;
                if (alpha != _color.A)
                {
                    _color = Color.FromArgb(alpha, _color);
                }
            }
        }

        /// <summary>
        /// Gets or sets the font style to use for this character layer.
        /// </summary>
        [Description("Gets or sets the font style to use for this character layer.")]
        [Serialize("FontStyle")]
        public FontStyle Style
        {
            get
            {
                return _style;
            }

            set
            {
                _style = value;
            }
        }

        /// <summary>
        /// Gets or sets the xml color. This supports serialization even though Colors can't be serialized.
        /// </summary>
        [XmlElement("Color")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Serialize("XmlColor")]
        public string XmlColor
        {
            get
            {
                return ColorTranslator.ToHtml(_color);
            }

            set
            {
                _color = ColorTranslator.FromHtml(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the font color of this symbol to represent the color of this symbol
        /// </summary>
        /// <returns>The color of this symbol as a font</returns>
        public override Color GetColor()
        {
            return _color;
        }

        /// <summary>
        /// Because there is no easy range calculation supported in dot net (as compared to GDI32) that I can find, I assume that the
        /// unsupported values will come back as an open box, or at least identical in glyph form to a known unsupported like Arial 1024.
        /// (Not to be confused with Arial Unicode MS, which has basically everything.)
        /// </summary>
        /// <returns>A list of supported character subsets.</returns>
        public IList<CharacterSubset> GetSupportedSubsets()
        {
            List<CharacterSubset> result = new List<CharacterSubset>();

            PointF topLeft = new PointF(0F, 0F);
            Font arialFont = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Pixel);
            Font testFont = new Font(_fontFamilyName, 10F, FontStyle.Regular, GraphicsUnit.Pixel); // ensure it will fit on bitmap

            Bitmap empty = new Bitmap(20, 20);
            Graphics g = Graphics.FromImage(empty);
            g.DrawString(((char)1024).ToString(), arialFont, Brushes.Black, topLeft);
            g.Dispose();
            BitmapGrid emptyGrid = new BitmapGrid(empty);

            Array subsets = Enum.GetValues(typeof(CharacterSubset));

            foreach (CharacterSubset code in subsets)
            {
                char c = (char)code; // first character of each subset
                Bitmap firstChar = new Bitmap(20, 20);
                Graphics tg = Graphics.FromImage(firstChar);
                tg.DrawString(c.ToString(), testFont, Brushes.Black, topLeft);
                tg.Dispose();

                // Bitmap grids allow a byte by byte comparison between values
                BitmapGrid firstCharGrid = new BitmapGrid(firstChar);
                if (emptyGrid.Matches(firstCharGrid) == false)
                {
                    // Since this symbol is different from the default unsuported symbol
                    result.Add(code);
                }

                firstCharGrid.Dispose();
            }

            return result;
        }

        /// <summary>
        /// Modifies this symbol in a way that is appropriate for indicating a selected symbol.
        /// This could mean drawing a cyan outline, or changing the color to cyan.
        /// </summary>
        public override void Select()
        {
            _color = Color.Cyan;
        }

        /// <summary>
        /// Sets the fill color of this symbol to the specified color
        /// </summary>
        /// <param name="color">The Color</param>
        public override void SetColor(Color color)
        {
            _color = color;
        }

        /// <summary>
        /// Gets the string equivalent of the specified character code.
        /// </summary>
        /// <returns>A string version of the character</returns>
        public override string ToString()
        {
            return _character.ToString();
        }

        /// <summary>
        /// Overrides the default behavior and attempts to draw the specified symbol.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="scaleSize">If this should draw in pixels, this should be 1. Otherwise, this should be
        /// the constant that you multiply against so that drawing using geographic units will draw in pixel units.</param>
        protected override void OnDraw(Graphics g, double scaleSize)
        {
            // base.OnDraw(g, scaleSize); // handle rotation etc.
            Brush b = new SolidBrush(_color);
            string txt = new string(new[] { _character });
            float fontPointSize = (float)(Size.Height * scaleSize);
            Font fnt = new Font(_fontFamilyName, fontPointSize, _style, GraphicsUnit.Pixel);
            SizeF fSize = g.MeasureString(txt, fnt);
            float x = -fSize.Width / 2;
            float y = -fSize.Height / 2;
            if (fSize.Height > fSize.Width * 5)
            {
                // Defective fonts sometimes are created with a bad height.
                // Use the width instead
                y = -fSize.Width / 2;
            }

            g.DrawString(txt, fnt, b, new PointF(x, y));
            b.Dispose();
        }

        /// <summary>
        /// Extends the randomize code to include the character aspects, creating a random character.
        /// However, since most fonts don't support full unicode values, a character from 0 to 255 is
        /// chosen.
        /// </summary>
        /// <param name="generator">The random class generator</param>
        protected override void OnRandomize(Random generator)
        {
            _color = generator.NextColor();
            Opacity = generator.NextFloat();
            _character = (char)generator.Next(0, 255);
            _fontFamilyName = FontFamily.Families[generator.Next(0, FontFamily.Families.Length - 1)].Name;
            _style = generator.NextEnum<FontStyle>();
            base.OnRandomize(generator);
        }

        #endregion
    }
}