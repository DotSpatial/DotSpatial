// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
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
        #region Fields

        private int _axisTextHeight;
        private int _countTextWidth;
        private int _numColumns;
        private int _titleHeight;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BarGraph"/> class.
        /// </summary>
        /// <param name="width">The width of the bar graph.</param>
        /// <param name="height">The height of the bar graph.</param>
        public BarGraph(int width, int height)
        {
            _numColumns = 40;
            Font = new TextFont();
            TitleFont = new TextFont(20f);
            Title = "Statistical Breaks:";
            _countTextWidth = 15;
            _axisTextHeight = 30;
            MinHeight = 20;
            Width = width;
            Height = height;
            Minimum = 0;
            Maximum = 100;
            ColorRanges = new List<ColorRange>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the array of integers that represent the positive integer value stored in each bar.
        /// </summary>
        public int[] Bins { get; set; }

        /// <summary>
        /// Gets or sets the list of color ranges that control how the colors are drawn to the graph.
        /// </summary>
        public List<ColorRange> ColorRanges { get; set; }

        /// <summary>
        /// Gets or sets the Font for text like the axis labels.
        /// </summary>
        public TextFont Font { get; set; }

        /// <summary>
        /// Gets or sets the integer height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not count values should be drawn with
        /// heights that are proportional to the logarithm of the count, instead of the count itself.
        /// </summary>
        public bool LogY { get; set; }

        /// <summary>
        /// Gets or sets the integer maximum from all of the current bins.
        /// </summary>
        public int MaxBinCount { get; set; }

        /// <summary>
        /// Gets or sets the maximum. This doesn't affect the statistical minimum or maximum, but rather the current view extents.
        /// </summary>
        public double Maximum { get; set; }

        /// <summary>
        /// Gets or sets the mean. The mean line can be drawn if it is in the view range. This is the statistical mean
        /// for all the values, not just the values currently in view.
        /// </summary>
        public double Mean { get; set; }

        /// <summary>
        /// Gets or sets the minimum height. Very small counts frequently disappear next to big counts. One strategy is to use a
        /// minimum height, so that the difference between 0 and 1 is magnified on the columns.
        /// </summary>
        public int MinHeight { get; set; }

        /// <summary>
        /// Gets or sets the minimum extent for this graph. This doesn't affect the numeric statistics,
        /// but only the current view of that statistics.
        /// </summary>
        public double Minimum { get; set; }

        /// <summary>
        /// Gets or sets the number of columns. Setting this will recalculate the bins.
        /// </summary>
        public int NumColumns
        {
            get
            {
                return _numColumns;
            }

            set
            {
                if (value < 0) value = 0;
                _numColumns = value;
            }
        }

        /// <summary>
        /// Gets or sets the color range.
        /// </summary>
        public ColorRange SelectedRange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the mean will be shown as a blue dotted line.
        /// </summary>
        public bool ShowMean { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the integral standard deviations from the mean will be drawn
        /// as red dotted lines.
        /// </summary>
        public bool ShowStandardDeviation { get; set; }

        /// <summary>
        /// Gets or sets the double standard deviation.  If ShowStandardDeviation is true, then
        /// they will be represented by red lines on either side of the mean.
        /// </summary>
        public double StandardDeviation { get; set; }

        /// <summary>
        /// Gets or sets the title of the graph.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the font to use for the graph title.
        /// </summary>
        public TextFont TitleFont { get; set; }

        /// <summary>
        /// Gets or sets the width of this graph in pixels.
        /// </summary>
        public int Width { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the real data value at the center of one of the bins.
        /// </summary>
        /// <param name="binIndex">Index of the bin.</param>
        /// <returns>The center value.</returns>
        public double CenterValue(int binIndex)
        {
            return Minimum + ((Maximum - Minimum) * (binIndex + .5) / NumColumns);
        }

        /// <summary>
        /// Disposes the font and titlefont
        /// </summary>
        public void Dispose()
        {
            Font.Dispose();
            TitleFont.Dispose();
        }

        /// <summary>
        /// Draws the graph, the colored bins, selected region, and any text, but not any sliders.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clip">The clip rectangle.</param>
        public void Draw(Graphics g, Rectangle clip)
        {
            DrawText(g);
            Rectangle gb = GetGraphBounds();
            DrawSelectionHighlight(g, clip, gb);
            DrawColumns(g, clip);
            g.DrawRectangle(Pens.Black, gb);
            if (ShowMean)
            {
                using (Pen p = new Pen(Color.Blue) { DashStyle = DashStyle.Dash })
                {
                    float x = GetPosition(Mean);
                    if (x > -32000 && x < 32000) g.DrawLine(p, x, gb.Top, x, gb.Bottom);
                }
            }

            if (ShowStandardDeviation)
            {
                using (Pen p = new Pen(Color.Red) { DashStyle = DashStyle.Dash })
                {
                    for (int i = 1; i < 6; i++)
                    {
                        double h = Mean + (StandardDeviation * i);
                        double l = Mean - (StandardDeviation * i);
                        if (h < Maximum && h > Minimum)
                        {
                            float x = GetPosition(h);
                            if (x > -32000 && x < 32000) g.DrawLine(p, x, gb.Top, x, gb.Bottom);
                        }

                        if (l < Maximum && l > Minimum)
                        {
                            float x = GetPosition(l);
                            if (x > -32000 && x < 32000) g.DrawLine(p, x, gb.Top, x, gb.Bottom);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draws only the text for this bar graph.  This will also calculate some critical
        /// font measurements to help size the internal part of the graph.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        public void DrawText(Graphics g)
        {
            _titleHeight = (int)Math.Ceiling(g.MeasureString("My", TitleFont.GetFont()).Height);
            Font fnt = Font.GetFont();
            string min = Format(Minimum);
            SizeF minSize = g.MeasureString(min, fnt);
            string max = Format(Maximum);
            SizeF maxSize = g.MeasureString(max, fnt);
            string mid = Format((Maximum + Minimum) / 2);
            SizeF midSize = g.MeasureString(mid, fnt);
            _axisTextHeight = (int)Math.Ceiling(Math.Max(Math.Max(minSize.Height, maxSize.Height), midSize.Height));
            _axisTextHeight += 10;

            const string One = "1";
            SizeF oneSize = g.MeasureString(One, fnt);
            string count = Format(MaxBinCount);
            SizeF countSize = g.MeasureString(count, fnt);
            SizeF halfSize = new SizeF(0, 0);
            string halfCount = string.Empty;
            if (MaxBinCount > 1)
            {
                halfCount = Format(MaxBinCount / 2f);
                if (LogY) halfCount = Format(Math.Exp(Math.Log(MaxBinCount) / 2));
                halfSize = g.MeasureString(halfCount, fnt);
            }

            _countTextWidth = (int)Math.Ceiling(Math.Max(Math.Max(oneSize.Width, countSize.Width), halfSize.Width));
            _countTextWidth += 20;

            Rectangle gb = GetGraphBounds();

            Font.Draw(g, min, gb.X, gb.Bottom + 15);
            g.DrawLine(Pens.Black, gb.Left, gb.Bottom, gb.Left, gb.Bottom + 10);
            Font.Draw(g, max, gb.Right - maxSize.Width, gb.Bottom + 15);
            g.DrawLine(Pens.Black, gb.Left + (gb.Width / 2), gb.Bottom, gb.Left + (gb.Width / 2), gb.Bottom + 10);
            Font.Draw(g, mid, gb.X + (gb.Width / 2) - (midSize.Width / 2), gb.Bottom + 15);
            g.DrawLine(Pens.Black, gb.Right, gb.Bottom, gb.Right, gb.Bottom + 10);
            float dY;
            if (MaxBinCount == 0)
            {
                dY = MinHeight;
            }
            else
            {
                dY = gb.Height / (float)MaxBinCount;
            }

            float oneH = Math.Max(dY, MinHeight);
            Font.Draw(g, One, gb.Left - oneSize.Width - 15, gb.Bottom - (oneSize.Height / 2) - oneH);
            g.DrawLine(Pens.Black, gb.Left - 10, gb.Bottom - oneH, gb.Left, gb.Bottom - oneH);
            if (MaxBinCount > 1)
            {
                Font.Draw(g, count, gb.Left - countSize.Width - 15, gb.Top);
                g.DrawLine(Pens.Black, gb.Left - 20, gb.Top, gb.Left, gb.Top);

                Font.Draw(g, halfCount, gb.Left - halfSize.Width - 15, gb.Top + (gb.Height / 2) - (halfSize.Height / 2));
                g.DrawLine(Pens.Black, gb.Left - 10, gb.Top + (gb.Height / 2), gb.Left, gb.Top + (gb.Height / 2));
            }

            SizeF titleSize = g.MeasureString(Title, TitleFont.GetFont());
            TitleFont.Draw(g, Title, gb.X + (gb.Width / 2) - (titleSize.Width / 2), 2.5f);
        }

        /// <summary>
        /// Gets the bounding rectangle for the actual graph itself.
        /// </summary>
        /// <returns>The bounding rectangle for the actual graph.</returns>
        public Rectangle GetGraphBounds()
        {
            return new Rectangle(_countTextWidth, _titleHeight + 5, Width - _countTextWidth - 10, Height - _axisTextHeight - 10 - _titleHeight);
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
            return (float)((gb.Width * (value - Minimum) / (Maximum - Minimum)) + gb.X);
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
            return (((position - gb.X) / gb.Width) * (Maximum - Minimum)) + Minimum;
        }

        private static string Format(double value)
        {
            if (Math.Abs(value) < 1) return value.ToString("E4");
            if (value < 1E10) return value.ToString("#, ###");
            return value.ToString("E4");
        }

        private void DrawColumns(Graphics g, Rectangle clip)
        {
            if (_numColumns == 0 || MaxBinCount == 0) return;
            Rectangle gb = GetGraphBounds();
            float dX = gb.Width / (float)_numColumns;
            float dY = gb.Height / (float)MaxBinCount;
            if (MaxBinCount == 0) return;
            if (LogY) dY = (float)(gb.Height / Math.Log(MaxBinCount));
            for (int i = 0; i < _numColumns; i++)
            {
                if (Bins[i] == 0) continue;
                float h = dY * Bins[i];
                if (LogY) h = (float)(dY * Math.Log(Bins[i]));
                if (h < MinHeight) h = MinHeight;
                RectangleF rect = new RectangleF((dX * i) + gb.X, gb.Height - h + gb.Y, dX, h);
                if (!clip.IntersectsWith(rect)) continue;
                Color light = Color.LightGray;
                Color dark = Color.DarkGray;
                double centerValue = CenterValue(i);

                if (ColorRanges != null)
                {
                    foreach (ColorRange item in ColorRanges)
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

        private void DrawSelectionHighlight(Graphics g, Rectangle clip, Rectangle gb)
        {
            if (SelectedRange == null) return;

            int index = ColorRanges.IndexOf(SelectedRange);
            if (index < 0) return;
            float left = gb.Left;
            if (SelectedRange.Range.Maximum < Minimum) return;
            if (SelectedRange.Range.Minimum > Maximum) return;
            if (SelectedRange.Range.Minimum != null)
            {
                float rangeLeft = GetPosition(SelectedRange.Range.Minimum.Value);
                if (rangeLeft > left) left = rangeLeft;
            }

            float right = gb.Right;
            if (SelectedRange.Range.Maximum != null)
            {
                float rangeRight = GetPosition(SelectedRange.Range.Maximum.Value);
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

        #endregion
    }
}