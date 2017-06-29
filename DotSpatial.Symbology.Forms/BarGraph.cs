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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/4/2009 10:42:27 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// The bar graph, when given a rectangular frame to work in, calculates appropriate bins
    /// from values, and draws the various labels and bars necessary.
    /// </summary>
    public class BarGraph : IDisposable
    {
        #region Private Variables

        private int _axisTextHeight;
        private int[] _bins;
        private List<ColorRange> _colorRanges;
        private int _countTextWidth;
        private TextFont _font;
        private int _height;
        private bool _logY;
        private int _maxBinCount;
        private double _maximum;
        private double _mean;
        private int _minHeight;
        private double _minimum;
        private int _numColumns;
        private ColorRange _selectedRange;
        private bool _showMean;
        private bool _showStandardDeviation;
        private double _std;
        private string _title;
        private TextFont _titleFont;
        private int _titleHeight;
        private int _width;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of BarGraph
        /// </summary>
        public BarGraph(int width, int height)
        {
            _numColumns = 40;
            _font = new TextFont();
            _titleFont = new TextFont(20f);
            _title = "Statistical Breaks:";
            _countTextWidth = 15;
            _axisTextHeight = 30;
            _minHeight = 20;
            _width = width;
            _height = height;
            _minimum = 0;
            _maximum = 100;
            _colorRanges = new List<ColorRange>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the bounding rectangle for the actual graph itself
        /// </summary>
        public Rectangle GetGraphBounds()
        {
            return new Rectangle(_countTextWidth, _titleHeight + 5, _width - _countTextWidth - 10, _height - _axisTextHeight - 10 - _titleHeight);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the array of integers that represent the positive integer value stored in
        /// each bar.
        /// </summary>
        public int[] Bins
        {
            get { return _bins; }
            set { _bins = value; }
        }

        /// <summary>
        /// Gets or sets the list of color ranges that control how the colors are drawn to the graph.
        /// </summary>
        public List<ColorRange> ColorRanges
        {
            get { return _colorRanges; }
            set { _colorRanges = value; }
        }

        /// <summary>
        /// Gets or sets the Font for text like the axis labels
        /// </summary>
        public TextFont Font
        {
            get { return _font; }
            set { _font = value; }
        }

        /// <summary>
        /// Gets or sets the integer height
        /// </summary>
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether or not count values should be drawn with
        /// heights that are proportional to the logarithm of the count, instead of the count itself.
        /// </summary>
        public bool LogY
        {
            get { return _logY; }
            set
            {
                _logY = value;
            }
        }

        /// <summary>
        /// Gets or sets the integer maximum from all of the current bins.
        /// </summary>
        public int MaxBinCount
        {
            get { return _maxBinCount; }
            set { _maxBinCount = value; }
        }

        /// <summary>
        /// This doesn't affect the statistical minimum or maximum, but rather the current view extents.
        /// </summary>
        public double Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        /// <summary>
        /// The mean line can be drawn if it is in the view range.  This is the statistical mean
        /// for all the values, not just the values currently in view.
        /// </summary>
        public double Mean
        {
            get { return _mean; }
            set { _mean = value; }
        }

        /// <summary>
        /// Gets or sets the double standard deviation.  If ShowStandardDeviation is true, then
        /// they will be represented by red lines on either side of the mean.
        /// </summary>
        public double StandardDeviation
        {
            get { return _std; }
            set { _std = value; }
        }

        /// <summary>
        /// Very small counts frequently dissappear next to big counts.  One strategey is to use a
        /// minimum height, so that the difference between 0 and 1 is magnified on the columns.
        /// </summary>
        public int MinHeight
        {
            get { return _minHeight; }
            set { _minHeight = value; }
        }

        /// <summary>
        /// Gets or sets the maximum extent for this graph.  This doesn't affect the numeric statistics,
        /// but only the current view of that statistics.
        /// </summary>
        public double Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        /// <summary>
        /// Gets or sets the number of columns.  Setting this will recalculate the bins.
        /// </summary>
        public int NumColumns
        {
            get { return _numColumns; }
            set
            {
                if (value < 0) value = 0;
                _numColumns = value;
            }
        }

        /// <summary>
        /// Gets or sets the color range.
        /// </summary>
        public ColorRange SelectedRange
        {
            get { return _selectedRange; }
            set { _selectedRange = value; }
        }

        /// <summary>
        /// Boolean, if this is true, the mean will be shown as a blue dotted line.
        /// </summary>
        public bool ShowMean
        {
            get { return _showMean; }
            set
            {
                _showMean = value;
            }
        }

        /// <summary>
        /// Boolean, if this is true, the integral standard deviations from the mean will be drawn
        /// as red dotted lines.
        /// </summary>
        public bool ShowStandardDeviation
        {
            get { return _showStandardDeviation; }
            set
            {
                _showStandardDeviation = value;
            }
        }

        /// <summary>
        /// Gets or sets the title of the graph.
        /// </summary>
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        /// <summary>
        /// Gets or sets the font to use for the graph title
        /// </summary>
        public TextFont TitleFont
        {
            get { return _titleFont; }
            set
            {
                _titleFont = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of this graph in pixels.
        /// </summary>
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes the font and titlefont
        /// </summary>
        public void Dispose()
        {
            _font.Dispose();
            _titleFont.Dispose();
        }

        #endregion

        /// <summary>
        /// Draws the graph, the colored bins, selected region, and any text, but not any sliders.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clip"></param>
        public void Draw(Graphics g, Rectangle clip)
        {
            DrawText(g);
            Rectangle gb = GetGraphBounds();
            DrawSelectionHighlight(g, clip, gb);
            DrawColumns(g, clip);
            g.DrawRectangle(Pens.Black, gb);
            if (_showMean)
            {
                Pen p = new Pen(Color.Blue);
                p.DashStyle = DashStyle.Dash;
                float x = GetPosition(_mean);
                if (x > -32000 && x < 32000) g.DrawLine(p, x, gb.Top, x, gb.Bottom);
                p.Dispose();
            }
            if (_showStandardDeviation)
            {
                Pen p = new Pen(Color.Red);
                p.DashStyle = DashStyle.Dash;
                for (int i = 1; i < 6; i++)
                {
                    double h = _mean + _std * i;
                    double l = _mean - _std * i;
                    if (h < _maximum && h > _minimum)
                    {
                        float x = GetPosition(h);
                        if (x > -32000 && x < 32000) g.DrawLine(p, x, gb.Top, x, gb.Bottom);
                    }
                    if (l < _maximum && l > _minimum)
                    {
                        float x = GetPosition(l);
                        if (x > -32000 && x < 32000) g.DrawLine(p, x, gb.Top, x, gb.Bottom);
                    }
                }
                p.Dispose();
            }
        }

        private void DrawColumns(Graphics g, Rectangle clip)
        {
            if (_numColumns == 0 || _maxBinCount == 0) return;
            Rectangle gb = GetGraphBounds();
            float dX = (gb.Width) / (float)_numColumns;
            float dY = (gb.Height) / (float)_maxBinCount;
            if (_maxBinCount == 0) return;
            if (_logY) dY = (float)((gb.Height) / Math.Log(_maxBinCount));
            for (int i = 0; i < _numColumns; i++)
            {
                if (_bins[i] == 0) continue;
                float h = dY * _bins[i];
                if (_logY) h = (float)((dY) * Math.Log(_bins[i]));
                if (h < _minHeight) h = _minHeight;
                RectangleF rect = new RectangleF(dX * i + gb.X, gb.Height - h + gb.Y, dX, h);
                if (!clip.IntersectsWith(rect)) continue;
                Color light = Color.LightGray;
                Color dark = Color.DarkGray;
                double centerValue = CenterValue(i);

                if (_colorRanges != null)
                {
                    foreach (ColorRange item in _colorRanges)
                    {
                        if (!item.Contains(centerValue)) continue;
                        Color c = item.Color;
                        light = c.Lighter(.2f);
                        dark = c.Darker(.2f);
                        break;
                    }
                }

                LinearGradientBrush lgb = new LinearGradientBrush(rect, light, dark, LinearGradientMode.Horizontal);
                g.FillRectangle(lgb, rect.X, rect.Y, rect.Width, rect.Height);
                lgb.Dispose();
            }
        }

        /// <summary>
        /// Given a double value, this returns the floating point position on this graph,
        /// based on the current minimum, maximum values.
        /// </summary>
        /// <param name="value">The double value to locate</param>
        /// <returns>A floating point X position</returns>
        public float GetPosition(double value)
        {
            Rectangle gb = GetGraphBounds();
            return (float)(gb.Width * (value - _minimum) / (_maximum - _minimum) + gb.X);
        }

        /// <summary>
        /// Given a floating point X coordinate (relative to the control, not just the graph)
        /// this will return the double value represented by that location.
        /// </summary>
        /// <param name="position">The floating point position</param>
        /// <returns>The double value at the specified X coordinate</returns>
        public double GetValue(float position)
        {
            Rectangle gb = GetGraphBounds();
            return ((position - gb.X) / gb.Width) * (_maximum - _minimum) + _minimum;
        }

        private void DrawSelectionHighlight(Graphics g, Rectangle clip, Rectangle gb)
        {
            if (_selectedRange == null) return;

            int index = _colorRanges.IndexOf(_selectedRange);
            if (index < 0) return;
            float left = gb.Left;
            if (_selectedRange.Range.Maximum < _minimum) return;
            if (_selectedRange.Range.Minimum > _maximum) return;
            if (_selectedRange.Range.Minimum != null)
            {
                float rangeLeft = GetPosition(_selectedRange.Range.Minimum.Value);
                if (rangeLeft > left) left = rangeLeft;
            }
            float right = gb.Right;
            if (_selectedRange.Range.Maximum != null)
            {
                float rangeRight = GetPosition(_selectedRange.Range.Maximum.Value);
                if (rangeRight < right) right = rangeRight;
            }
            Rectangle selectionRect = new Rectangle((int)left, gb.Top, (int)(right - left), gb.Height);
            if (!clip.IntersectsWith(selectionRect)) return;
            GraphicsPath gp = new GraphicsPath();
            gp.AddRoundedRectangle(selectionRect, 2);
            if (selectionRect.Width != 0 && selectionRect.Height != 0)
            {
                LinearGradientBrush lgb = new LinearGradientBrush(selectionRect, Color.FromArgb(241, 248, 253), Color.FromArgb(213, 239, 252), LinearGradientMode.ForwardDiagonal);
                g.FillPath(lgb, gp);
                lgb.Dispose();
            }

            gp.Dispose();
        }

        /// <summary>
        /// Gets the real data value at the center of one of the bins.
        /// </summary>
        /// <param name="binIndex"></param>
        /// <returns></returns>
        public double CenterValue(int binIndex)
        {
            return _minimum + (_maximum - _minimum) * (binIndex + .5) / NumColumns;
        }

        private static string Format(double value)
        {
            if (Math.Abs(value) < 1) return value.ToString("E4");
            if (value < 1E10) return value.ToString("#, ###");
            return value.ToString("E4");
        }

        /// <summary>
        /// Draws only the text for this bar graph.  This will also calculate some critical
        /// font measurements to help size the internal part of the graph.
        /// </summary>
        /// <param name="g"></param>
        public void DrawText(Graphics g)
        {
            _titleHeight = (int)Math.Ceiling(g.MeasureString("My", _titleFont.GetFont()).Height);
            Font fnt = _font.GetFont();
            string min = Format(_minimum);
            SizeF minSize = g.MeasureString(min, fnt);
            string max = Format(_maximum);
            SizeF maxSize = g.MeasureString(max, fnt);
            string mid = Format((_maximum + _minimum) / 2);
            SizeF midSize = g.MeasureString(mid, fnt);
            _axisTextHeight = (int)(Math.Ceiling(Math.Max(Math.Max(minSize.Height, maxSize.Height), midSize.Height)));
            _axisTextHeight += 10;

            const string one = "1";
            SizeF oneSize = g.MeasureString(one, fnt);
            string count = Format(_maxBinCount);
            SizeF countSize = g.MeasureString(count, fnt);
            SizeF halfSize = new SizeF(0, 0);
            string halfCount = string.Empty;
            if (_maxBinCount > 1)
            {
                halfCount = Format(_maxBinCount / 2f);
                if (_logY) halfCount = Format(Math.Exp(Math.Log(_maxBinCount) / 2));
                halfSize = g.MeasureString(halfCount, fnt);
            }

            _countTextWidth = (int)(Math.Ceiling(Math.Max(Math.Max(oneSize.Width, countSize.Width), halfSize.Width)));
            _countTextWidth += 20;

            Rectangle gb = GetGraphBounds();

            _font.Draw(g, min, gb.X, gb.Bottom + 15);
            g.DrawLine(Pens.Black, gb.Left, gb.Bottom, gb.Left, gb.Bottom + 10);
            _font.Draw(g, max, gb.Right - maxSize.Width, gb.Bottom + 15);
            g.DrawLine(Pens.Black, gb.Left + gb.Width / 2, gb.Bottom, gb.Left + gb.Width / 2, gb.Bottom + 10);
            _font.Draw(g, mid, gb.X + gb.Width / 2 - midSize.Width / 2, gb.Bottom + 15);
            g.DrawLine(Pens.Black, gb.Right, gb.Bottom, gb.Right, gb.Bottom + 10);
            float dY;
            if (_maxBinCount == 0)
            {
                dY = _minHeight;
            }
            else
            {
                dY = (gb.Height) / (float)_maxBinCount;
            }
            float oneH = Math.Max(dY, _minHeight);
            _font.Draw(g, one, gb.Left - oneSize.Width - 15, gb.Bottom - oneSize.Height / 2 - oneH);
            g.DrawLine(Pens.Black, gb.Left - 10, gb.Bottom - oneH, gb.Left, gb.Bottom - oneH);
            if (_maxBinCount > 1)
            {
                _font.Draw(g, count, gb.Left - countSize.Width - 15, gb.Top);
                g.DrawLine(Pens.Black, gb.Left - 20, gb.Top, gb.Left, gb.Top);

                _font.Draw(g, halfCount, gb.Left - halfSize.Width - 15, gb.Top + gb.Height / 2 - halfSize.Height / 2);
                g.DrawLine(Pens.Black, gb.Left - 10, gb.Top + gb.Height / 2, gb.Left, gb.Top + gb.Height / 2);
            }
            SizeF titleSize = g.MeasureString(_title, _titleFont.GetFont());
            _titleFont.Draw(g, _title, gb.X + gb.Width / 2 - titleSize.Width / 2, 2.5f);
        }
    }
}