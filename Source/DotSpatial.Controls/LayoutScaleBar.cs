// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Globalization;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A scale bar control that can be linked to a map and provide a dynamic scale bar for the print layout
    /// </summary>
    public class LayoutScaleBar : LayoutElement
    {
        #region Fields

        private bool _breakBeforeZero;
        private Color _color;
        private Font _font;
        private LayoutControl _layoutControl;
        private LayoutMap _layoutMap;
        private int _numBreaks;
        private TextRenderingHint _textHint;
        private ScaleBarUnit _unit;
        private string _unitText;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutScaleBar"/> class.
        /// </summary>
        public LayoutScaleBar()
        {
            Name = "Scale Bar";
            _font = new Font("Arial", 10);
            _color = Color.Black;
            _unit = ScaleBarUnit.Kilometers;
            _unitText = "km";
            _numBreaks = 4;
            _textHint = TextRenderingHint.AntiAliasGridFit;
            ResizeStyle = ResizeStyle.HandledInternally;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether a break should be present before the 0.
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public bool BreakBeforeZero
        {
            get
            {
                return _breakBeforeZero;
            }

            set
            {
                _breakBeforeZero = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the color of the text
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public Color Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the font used to draw this text
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public Font Font
        {
            get
            {
                return _font;
            }

            set
            {
                _font = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets a layout control
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get
            {
                return _layoutControl;
            }

            set
            {
                _layoutControl = value;
                _layoutControl.ElementsChanged += LayoutControlElementsChanged;
            }
        }

        /// <summary>
        /// Gets or sets the Map control that the scale bar uses for measurement decisions
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        [Editor(typeof(LayoutMapEditor), typeof(UITypeEditor))]
        public virtual LayoutMap Map
        {
            get
            {
                return _layoutMap;
            }

            set
            {
                _layoutMap = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the number of breaks the scale bar should have
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public int NumberOfBreaks
        {
            get
            {
                return _numBreaks;
            }

            set
            {
                _numBreaks = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the hinting used to draw the text
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public TextRenderingHint TextHint
        {
            get
            {
                return _textHint;
            }

            set
            {
                _textHint = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the unit to use for the scale bar
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public ScaleBarUnit Unit
        {
            get
            {
                return _unit;
            }

            set
            {
                _unit = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        /// <summary>
        /// Gets or sets the unit text to display after the scale bar
        /// </summary>
        [Browsable(true)]
        [Category("Symbol")]
        public string UnitText
        {
            get
            {
                return _unitText;
            }

            set
            {
                _unitText = value;
                UpdateThumbnail();
                OnInvalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This gets called to instruct the element to draw itself in the appropriate spot of the graphics object
        /// </summary>
        /// <param name="g">The graphics object to draw to</param>
        /// <param name="printing">Boolean, true if printing to an actual print document</param>
        public override void Draw(Graphics g, bool printing)
        {
            GraphicsPath GP = new GraphicsPath();
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Near;
            format.Alignment = StringAlignment.Near;

            if (_layoutMap == null || _layoutMap.Scale == 0) return;

            // Sets up the pens and brushes
            Brush scaleBrush = new SolidBrush(_color);
            Pen scalePen = new Pen(scaleBrush);

            // Calculates the width of one break in greographic units
            float unitLegnth = g.MeasureString(_unitText, _font).Width * 2;
            float widthNoUnit = Size.Width - unitLegnth;
            long geoBreakWidth = Convert.ToInt64((widthNoUnit / 100 * _layoutMap.Scale) / GetConversionFactor(_unit.ToString()) / _numBreaks);

            // If the geobreakWidth is less than 1 we return and don't draw anything
            if (geoBreakWidth < 1)
                return;

            // Save the old transform
            Matrix oldTransform = g.Transform;
            g.TranslateTransform(Location.X, Location.Y);
            TextRenderingHint oldHint = g.TextRenderingHint;
            g.TextRenderingHint = _textHint;

            double n = Math.Pow(10, geoBreakWidth.ToString(CultureInfo.InvariantCulture).Length - 1);
            geoBreakWidth = Convert.ToInt64(Math.Floor(geoBreakWidth / n) * n);
            long breakWidth = Convert.ToInt64((1D * geoBreakWidth / _layoutMap.Scale) * GetConversionFactor(_unit.ToString()) * 100D);
            float fontHeight = g.MeasureString(geoBreakWidth.ToString(CultureInfo.InvariantCulture), _font).Height;
            float leftStart = g.MeasureString(Math.Abs(geoBreakWidth).ToString(CultureInfo.InvariantCulture), _font).Width / 2F;

            // Decides if a break should be drawn before the zero
            int startBreak = 0;
            if (_breakBeforeZero)
                startBreak = -1;

            g.DrawLine(scalePen, leftStart, fontHeight * 1.6f, leftStart + (breakWidth * _numBreaks), fontHeight * 1.6f);

            //g.DrawString("1 : " + String.Format("{0:0, }", Map.Scale), _font, scaleBrush, leftStart - (g.MeasureString(Math.Abs(geoBreakWidth * startBreak).ToString(), _font).Width / 2), fontHeight * 2.5F);
            //CGX
            PointF pointScale = new PointF(leftStart - (g.MeasureString(Math.Abs(geoBreakWidth * startBreak).ToString(), _font).Width / 2), fontHeight * 2.5F);
            GP.AddString("1 : " + String.Format("{0:0 }", Map.Scale), _font.FontFamily, (int)_font.Style, _font.SizeInPoints * 96F / 72F, pointScale, format);

            for (int i = startBreak; i <= _numBreaks + startBreak; i++)
            {
                PointF pUp = new PointF(leftStart, fontHeight + (fontHeight * 1.1f));
                PointF pDown = new PointF(leftStart, fontHeight * 1.1f);

                g.DrawLine(scalePen, leftStart, fontHeight * 1.1f, leftStart, fontHeight + (fontHeight * 1.1f));
                GP.AddString(Math.Abs(geoBreakWidth * i).ToString(CultureInfo.InvariantCulture), _font.FontFamily, (int)_font.Style, _font.SizeInPoints * 96F / 72F, new PointF(leftStart - (g.MeasureString(Math.Abs(geoBreakWidth * i).ToString(), _font).Width / 2), 0), format);
                leftStart = leftStart + breakWidth;
            }
            GP.AddString(_unitText, _font.FontFamily, (int)_font.Style, _font.SizeInPoints * 96F / 72F, new PointF(leftStart - breakWidth + (fontHeight / 2), fontHeight * 1.1f), format);

            g.FillPath(new SolidBrush(_color), GP);
            GP.Dispose();

            //Restore the old transform
            g.Transform = oldTransform;
            g.TextRenderingHint = oldHint;
        }

        /// <summary>
        /// Returns the conversion factor between the map units and inches
        /// </summary>
        /// <param name="mapWinUnits">A string represing the MapUnits</param>
        /// <returns>A double representing the conversion factor between MapUnits and inches. If something goes wrong we return 0</returns>
        private static double GetConversionFactor(string mapWinUnits)
        {
            switch (mapWinUnits.ToLower())
            {
                case "lat/long":
                    return 4366141.73;
                case "meters":
                    return 39.3700787;
                case "centimeters":
                    return 0.393700787;
                case "feet":
                    return 12;
                case "inches":
                    return 1;
                case "kilometers":
                    return 39370.0787;
                case "miles":
                    return 63360;
                case "millimeters":
                    return 0.0393700787;
                case "yards":
                    return 36;
                // CGX
                case "nauticalmiles":
                    return (72913.3858);
                // CGX END
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Updates the scale bar if the map is deleted
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void LayoutControlElementsChanged(object sender, EventArgs e)
        {
            if (!_layoutControl.LayoutElements.Contains(_layoutMap))
                Map = null;
        }

        #endregion
    }
}