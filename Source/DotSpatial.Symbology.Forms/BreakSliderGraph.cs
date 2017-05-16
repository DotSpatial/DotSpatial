// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// BreakSliderGraph
    /// </summary>
    [DefaultEvent("SliderMoved")]
    public class BreakSliderGraph : Control
    {
        #region Fields
        private readonly ContextMenu _contextMenu;
        private readonly BarGraph _graph;
        private readonly Statistics _statistics;
        private Color _breakColor;
        private bool _dragCursor;
        private string _fieldName;
        private bool _isDragging;
        private bool _isRaster;
        private string _normalizationField;
        private IRaster _raster;
        private IRasterLayer _rasterLayer;
        private IRasterSymbolizer _rasterSymbolizer;
        private IFeatureScheme _scheme;
        private Color _selectedBreakColor;
        private BreakSlider _selectedSlider;
        private IAttributeSource _source;
        private DataTable _table;
        private List<double> _values;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BreakSliderGraph"/> class.
        /// </summary>
        public BreakSliderGraph()
        {
            _graph = new BarGraph(Width, Height);
            MaximumSampleSize = 10000;
            Breaks = new List<BreakSlider>();
            _selectedBreakColor = Color.Red;
            _breakColor = Color.Blue;
            TitleFont = new Font("Arial", 20, FontStyle.Bold);
            BorderStyle = BorderStyle.Fixed3D;
            _contextMenu = new ContextMenu();
            _contextMenu.MenuItems.Add("Reset Zoom", ResetZoomClicked);
            _contextMenu.MenuItems.Add("Zoom To Categories", CategoryZoomClicked);
            _statistics = new Statistics();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the slider has been completely repositioned.
        /// </summary>
        public event EventHandler<BreakSliderEventArgs> SliderMoved;

        /// <summary>
        /// Occurs when manual break sliding begins so that the mode can be
        /// switched to manual, rather than showing equal breaks or something.
        /// </summary>
        public event EventHandler<BreakSliderEventArgs> SliderMoving;

        /// <summary>
        /// Occurs when a click in a range changes the slider that is selected.
        /// </summary>
        public event EventHandler<BreakSliderEventArgs> SliderSelected;

        /// <summary>
        /// Occurs after the statistics have been re-calculated.
        /// </summary>
        public event EventHandler<StatisticalEventArgs> StatisticsUpdated;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the attribute source.
        /// </summary>
        public IAttributeSource AttributeSource
        {
            get
            {
                return _source;
            }

            set
            {
                _source = value;
                UpdateBins();
            }
        }

        /// <summary>
        /// Gets or sets the border style for this control.
        /// </summary>
        public BorderStyle BorderStyle { get; set; }

        /// <summary>
        /// Gets or sets the color to use for the moveable breaks.
        /// </summary>
        public Color BreakColor
        {
            get
            {
                return _breakColor;
            }

            set
            {
                _breakColor = value;
                if (Breaks == null) return;
                foreach (BreakSlider slider in Breaks)
                {
                    slider.Color = value;
                }
            }
        }

        /// <summary>
        /// Gets the list of breaks that are currently part of this graph.
        /// </summary>
        public List<BreakSlider> Breaks { get; }

        /// <summary>
        /// Gets or sets the color to use when a break is selected
        /// </summary>
        public Color BreakSelectedColor
        {
            get
            {
                return _selectedBreakColor;
            }

            set
            {
                _selectedBreakColor = value;
                if (Breaks == null) return;
                foreach (BreakSlider slider in Breaks)
                {
                    slider.SelectColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the string field name.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Fieldname
        {
            get
            {
                return _fieldName;
            }

            set
            {
                _fieldName = value;
                UpdateBins();
            }
        }

        /// <summary>
        /// Gets or sets the font color.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the color for the axis labels")]
        public Color FontColor
        {
            get
            {
                if (_graph != null) return _graph.Font.Color;
                return Color.Black;
            }

            set
            {
                if (_graph != null) _graph.Font.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the method to use when breaks are calculated or reset.
        /// </summary>
        public IntervalMethod IntervalMethod
        {
            get
            {
                if (_scheme?.EditorSettings == null) return IntervalMethod.EqualInterval;
                return _scheme.EditorSettings.IntervalMethod;
            }

            set
            {
                if (_scheme?.EditorSettings == null) return;
                _scheme.EditorSettings.IntervalMethod = value;
                UpdateBreaks();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not count values should be drawn with
        /// heights that are proportional to the logarithm of the count, instead of the count itself.
        /// </summary>
        public bool LogY
        {
            get
            {
                return _graph != null && _graph.LogY;
            }

            set
            {
                if (_graph != null)
                {
                    _graph.LogY = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the maximum sample size to use when calculating statistics.
        /// The default is 10000.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the maximum sample size to use when calculating statistics.")]
        public int MaximumSampleSize { get; set; }

        /// <summary>
        /// Gets or sets the minimum height. Very small counts frequently dissappear next to big counts. One strategey is to use a
        /// minimum height, so that the difference between 0 and 1 is magnified on the columns.
        /// </summary>
        public int MinHeight
        {
            get
            {
                return _graph?.MinHeight ?? 20;
            }

            set
            {
                if (_graph != null) _graph.MinHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets the string normalization field
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string NormalizationField
        {
            get
            {
                return _normalizationField;
            }

            set
            {
                _normalizationField = value;
                UpdateBins();
            }
        }

        /// <summary>
        /// Gets or sets the number of columns. Setting this will recalculate the bins.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the number of columns representing the data histogram")]
        public int NumColumns
        {
            get
            {
                if (_graph == null) return 0;
                return _graph.NumColumns;
            }

            set
            {
                if (_graph == null) return;
                _graph.NumColumns = value;
                FillBins();
                UpdateBreaks();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the raster layer. This will also force this control to use
        /// the raster for calculations, rather than the dataset.
        /// </summary>
        public IRasterLayer RasterLayer
        {
            get
            {
                return _rasterLayer;
            }

            set
            {
                _rasterLayer = value;
                if (value == null) return;
                _isRaster = true;
                _raster = _rasterLayer.DataSet;
                _rasterSymbolizer = _rasterLayer.Symbolizer;
                ResetExtents();
                UpdateBins();
            }
        }

        /// <summary>
        /// Gets or sets the scheme that is currently being used to symbolize the values.
        /// Setting this automatically updates the graph extents to the statistical bounds
        /// of the scheme.
        /// </summary>
        public IFeatureScheme Scheme
        {
            get
            {
                return _scheme;
            }

            set
            {
                _scheme = value;
                _isRaster = false;
                ResetExtents();
                UpdateBreaks();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mean will be shown as a blue dotted line.
        /// </summary>
        [Category("Appearance")]
        [Description("Boolean, if this is true, the mean will be shown as a blue dotted line.")]
        public bool ShowMean
        {
            get
            {
                return _graph != null && _graph.ShowMean;
            }

            set
            {
                if (_graph != null)
                {
                    _graph.ShowMean = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the integral standard deviations from the mean will be drawn
        /// as red dotted lines.
        /// </summary>
        [Category("Appearance")]
        [Description("Boolean, if this is true, the integral standard deviations from the mean will be drawn as red dotted lines.")]
        public bool ShowStandardDeviation
        {
            get
            {
                return _graph != null && _graph.ShowStandardDeviation;
            }

            set
            {
                if (_graph != null)
                {
                    _graph.ShowStandardDeviation = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets the statistics that have been currently calculated for this graph.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Statistics Statistics
        {
            get
            {
                if (_isRaster)
                {
                    return _statistics;
                }

                return _scheme?.Statistics;
            }
        }

        /// <summary>
        /// Gets or sets the data Table for which the statistics should be applied
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DataTable Table
        {
            get
            {
                return _table;
            }

            set
            {
                _table = value;
                UpdateBins();
            }
        }

        /// <summary>
        /// Gets or sets the title of the graph.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the title of the graph.")]
        public string Title
        {
            get
            {
                return _graph?.Title;
            }

            set
            {
                if (_graph != null) _graph.Title = value;
            }
        }

        /// <summary>
        /// Gets or sets the color to use for the graph title.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the color to use for the graph title.")]
        public Color TitleColor
        {
            get
            {
                return _graph?.TitleFont.Color ?? Color.Black;
            }

            set
            {
                if (_graph != null) _graph.TitleFont.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the font to use for the graph title
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the font to use for the graph title.")]
        public Font TitleFont
        {
            get
            {
                return _graph?.TitleFont.GetFont();
            }

            set
            {
                _graph?.TitleFont.SetFont(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// When the mouse wheel event occurs, this forwards the event to this control.
        /// </summary>
        /// <param name="delta">The delta.</param>
        /// <param name="x">The x.</param>
        public void DoMouseWheel(int delta, float x)
        {
            double val = _graph.GetValue(x);

            if (delta > 0)
            {
                _graph.Minimum = _graph.Minimum + ((val - _graph.Minimum) / 2);
                _graph.Maximum = _graph.Maximum - ((_graph.Maximum - val) / 2);
            }
            else
            {
                _graph.Minimum = _graph.Minimum - (val - _graph.Minimum);
                _graph.Maximum = _graph.Maximum + (_graph.Maximum - val);
            }

            if (_isRaster)
            {
                if (_graph.Minimum < _rasterSymbolizer.Scheme.Statistics.Minimum) _graph.Minimum = _rasterSymbolizer.Scheme.Statistics.Minimum;
                if (_graph.Maximum > _rasterSymbolizer.Scheme.Statistics.Maximum) _graph.Maximum = _rasterSymbolizer.Scheme.Statistics.Maximum;
            }
            else
            {
                if (_graph.Minimum < _scheme.Statistics.Minimum) _graph.Minimum = _scheme.Statistics.Minimum;
                if (_graph.Maximum > _scheme.Statistics.Maximum) _graph.Maximum = _scheme.Statistics.Maximum;
            }

            FillBins();
            UpdateBreaks();
            Invalidate();
        }

        /// <summary>
        /// Returns the BreakSlider that corresponds to the specified mouse position, where
        /// the actual handle and divider represent the maximum of that range.
        /// </summary>
        /// <param name="location">The location, which can be anywhere to the left of the slider but to the
        /// right of any other sliders.</param>
        /// <returns>The BreakSlider that covers the range that contains the location, or null.</returns>
        public BreakSlider GetBreakAt(Point location)
        {
            if (location.X < 0) return null;
            foreach (BreakSlider slider in Breaks)
            {
                if (slider.Position < location.X) continue;
                return slider;
            }

            return null;
        }

        /// <summary>
        /// Resets the breaks using the current settings, and generates a new set of
        /// categories using the given interval method.
        /// </summary>
        /// <param name="handler">The progress handler.</param>
        public void ResetBreaks(ICancelProgressHandler handler)
        {
            if (_fieldName == null) return;
            if (_scheme?.EditorSettings == null) return;
            if (_source != null)
            {
                _scheme.CreateCategories(_source, handler);
            }
            else
            {
                if (_table == null) return;
                _scheme.CreateCategories(_table);
            }

            ResetZoom();
        }

        /// <summary>
        /// Given a scheme, this resets the graph extents to the statistical bounds.
        /// </summary>
        public void ResetExtents()
        {
            if (_graph == null) return;
            Statistics stats;
            if (_isRaster)
            {
                if (_raster == null) return;
                stats = _rasterSymbolizer.Scheme.Statistics;
            }
            else
            {
                if (_scheme == null) return;
                stats = _scheme.Statistics;
            }

            _graph.Minimum = stats.Minimum;
            _graph.Maximum = stats.Maximum;
            _graph.Mean = stats.Mean;
            _graph.StandardDeviation = stats.StandardDeviation;
        }

        /// <summary>
        /// Selects one of the specific breaks.
        /// </summary>
        /// <param name="slider">The break slider.</param>
        public void SelectBreak(BreakSlider slider)
        {
            if (_selectedSlider != null) _selectedSlider.Selected = false;
            _selectedSlider = slider;

            if (_selectedSlider != null)
            {
                _selectedSlider.Selected = true;
                _graph.SelectedRange = _selectedSlider.Range;
            }
        }

        /// <summary>
        /// Given a scheme, this will build the break list to match approximately. This does not
        /// force the interval method to build a new scheme.
        /// </summary>
        public void UpdateBreaks()
        {
            if (_isRaster)
            {
                UpdateRasterBreaks();
                return;
            }

            if (_scheme == null) return;
            IFeatureCategory selectedCat = null;
            if (_selectedSlider != null) selectedCat = _selectedSlider.Category as IFeatureCategory;
            Breaks.Clear();
            Statistics stats = _scheme.Statistics;
            Rectangle gb = _graph.GetGraphBounds();
            _graph.ColorRanges.Clear();
            foreach (IFeatureCategory category in _scheme.GetCategories())
            {
                ColorRange cr = new ColorRange(category.GetColor(), category.Range);
                _graph.ColorRanges.Add(cr);
                BreakSlider bs = new BreakSlider(gb, _graph.Minimum, _graph.Maximum, cr)
                {
                    Color = _breakColor,
                    SelectColor = _selectedBreakColor
                };
                if (selectedCat != null && category == selectedCat)
                {
                    bs.Selected = true;
                    _selectedSlider = bs;
                    _graph.SelectedRange = cr;
                }

                bs.Value = category.Maximum != null ? double.Parse(category.Maximum.ToString()) : stats.Maximum;

                bs.Category = category;
                Breaks.Add(bs);
            }

            Breaks.Sort();

            // Moving a break generally affects both a maximum and a minimum.
            // Point to the next category to actuate that.
            for (int i = 0; i < Breaks.Count - 1; i++)
            {
                Breaks[i].NextCategory = Breaks[i + 1].Category;

                // We use the maximums to set up breaks. Minimums should simply
                // be set to work with the maximums of the previous category.
                Breaks[i + 1].Category.Minimum = Breaks[i].Value;
            }

            if (Breaks.Count == 0) return;
            int breakIndex = 0;
            BreakSlider nextSlider = Breaks[breakIndex];
            int count = 0;
            if (_graph?.Bins == null) return;
            foreach (double value in _values)
            {
                if (value < nextSlider.Value)
                {
                    count++;
                    continue;
                }

                nextSlider.Count = count;
                while (value > nextSlider.Value)
                {
                    breakIndex++;
                    if (breakIndex >= Breaks.Count)
                    {
                        break;
                    }

                    nextSlider = Breaks[breakIndex];
                }

                count = 0;
            }
        }

        /// <summary>
        /// Given a break slider's new position, this will update the category related to that break.
        /// </summary>
        /// <param name="slider">The break slider.</param>
        public void UpdateCategory(BreakSlider slider)
        {
            slider.Category.Maximum = slider.Value;
            slider.Category.ApplyMinMax(_scheme.EditorSettings);
            int index = Breaks.IndexOf(slider);
            if (index < 0) return;
            if (index < Breaks.Count - 1)
            {
                Breaks[index + 1].Category.Minimum = slider.Value;
                Breaks[index + 1].Category.ApplyMinMax(_scheme.EditorSettings);
            }
        }

        /// <summary>
        /// Updates the raster breaks.
        /// </summary>
        public void UpdateRasterBreaks()
        {
            if (_rasterLayer == null) return;
            IColorCategory selectedBrk = null;
            if (_selectedSlider != null) selectedBrk = _selectedSlider.Category as IColorCategory;
            Breaks.Clear();
            Statistics stats = _rasterSymbolizer.Scheme.Statistics;
            Rectangle gb = _graph.GetGraphBounds();
            _graph.ColorRanges.Clear();
            foreach (IColorCategory category in _rasterSymbolizer.Scheme.Categories)
            {
                ColorRange cr = new ColorRange(category.LowColor, category.Range);
                _graph.ColorRanges.Add(cr);
                BreakSlider bs = new BreakSlider(gb, _graph.Minimum, _graph.Maximum, cr)
                {
                    Color = _breakColor,
                    SelectColor = _selectedBreakColor
                };
                if (selectedBrk != null && category == selectedBrk)
                {
                    bs.Selected = true;
                    _selectedSlider = bs;
                    _graph.SelectedRange = cr;
                }

                bs.Value = category.Maximum != null ? double.Parse(category.Maximum.ToString()) : stats.Maximum;

                bs.Category = category;
                Breaks.Add(bs);
            }

            Breaks.Sort();

            // Moving a break generally affects both a maximum and a minimum.
            // Point to the next category to actuate that.
            for (int i = 0; i < Breaks.Count - 1; i++)
            {
                Breaks[i].NextCategory = Breaks[i + 1].Category;

                // We use the maximums to set up breaks. Minimums should simply
                // be set to work with the maximums of the previous category.
                // _breaks[i + 1].Category.Minimum = _breaks[i].Value; REMOVED BY jany_ (2015-07-07) Don't set minimum, because that changes the minimum of the rasters category which causes the colors to change when saving in RasterColorControl without making changes or for example only applying opacity without wanting to use statistics.
            }

            if (Breaks.Count == 0) return;
            int breakIndex = 0;
            BreakSlider nextSlider = Breaks[breakIndex];
            int count = 0;
            if (_graph?.Bins == null) return;
            foreach (double value in _values)
            {
                if (value < nextSlider.Value)
                {
                    count++;
                    continue;
                }

                nextSlider.Count = count;
                while (value > nextSlider.Value)
                {
                    breakIndex++;
                    if (breakIndex >= Breaks.Count)
                    {
                        break;
                    }

                    nextSlider = Breaks[breakIndex];
                }

                count = 0;
            }
        }

        /// <summary>
        /// Zooms so that the minimum is the minimum of the lowest category or else the
        /// minimum of the extents, and the maximum is the maximum of the largest category
        /// or else the maximum of the statistics.
        /// </summary>
        public void ZoomToCategoryRange()
        {
            if (_graph == null) return;
            Statistics stats;
            double? min;
            double? max;
            if (_isRaster)
            {
                if (_raster == null) return;
                stats = _rasterSymbolizer.Scheme.Statistics;
                min = _rasterSymbolizer.Scheme.Categories[0].Minimum;
                max = _rasterSymbolizer.Scheme.Categories[_rasterSymbolizer.Scheme.Categories.Count - 1].Maximum;
            }
            else
            {
                if (_scheme == null) return;
                stats = _scheme.Statistics;
                var cats = _scheme.GetCategories().ToList();
                min = cats.First().Minimum;
                max = cats.Last().Maximum;
            }

            _graph.Minimum = (min == null || min.Value < stats.Minimum) ? stats.Minimum : min.Value;
            _graph.Maximum = (max == null || max.Value > stats.Maximum) ? stats.Maximum : max.Value;
            FillBins();
            UpdateBreaks();
            Invalidate();
        }

        /// <summary>
        /// Handles disposing to release unmanaged memory.
        /// </summary>
        /// <param name="disposing">Indicates whether managed resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            _graph?.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Occurs during drawing.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clip">The clip rectangle.</param>
        protected virtual void OnDraw(Graphics g, Rectangle clip)
        {
            if (Height <= 1 || Width <= 1) return;

            // Draw text first because the text is used to auto-fit the remaining graph.
            _graph.Draw(g, clip);

            if (BorderStyle == BorderStyle.Fixed3D)
            {
                g.DrawLine(Pens.White, 0, Height - 1, Width - 1, Height - 1);
                g.DrawLine(Pens.White, Width - 1, 0, Width - 1, Height - 1);
                g.DrawLine(Pens.Gray, 0, 0, 0, Height - 1);
                g.DrawLine(Pens.Gray, 0, 0, Width - 1, 0);
            }

            if (BorderStyle == BorderStyle.FixedSingle)
            {
                g.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
            }

            if (Breaks == null) return;
            Rectangle gb = _graph.GetGraphBounds();
            foreach (BreakSlider slider in Breaks)
            {
                slider.Setup(gb, _graph.Minimum, _graph.Maximum);

                if (slider.Bounds.IntersectsWith(clip)) slider.Draw(g);
            }
        }

        /// <summary>
        /// Occurs when the mose down occurs.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            if (e.Button == MouseButtons.Right)
            {
                _contextMenu.Show(this, e.Location);
                return;
            }

            foreach (BreakSlider slider in Breaks)
            {
                if (!slider.Bounds.Contains(e.Location) && !slider.HandleBounds.Contains(e.Location)) continue;

                // not sure if this works right. Hopefully, just the little rectangles form a double region.
                Region rg = new Region();
                if (_selectedSlider != null)
                {
                    rg.Union(_selectedSlider.Bounds);
                    _selectedSlider.Selected = false;
                }

                _selectedSlider = slider;
                slider.Selected = true;
                rg.Union(_selectedSlider.Bounds);
                Invalidate(rg);
                _isDragging = true;
                OnSliderMoving();
                return;
            }

            if (_selectedSlider != null) _selectedSlider.Selected = false;
            _selectedSlider = null;
            base.OnMouseDown(e);
        }

        /// <summary>
        /// Handles the mouse move event.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (_isDragging)
            {
                Region rg = new Region();
                Rectangle gb = _graph.GetGraphBounds();
                if (_selectedSlider != null)
                {
                    rg.Union(_selectedSlider.Bounds);
                    int x = e.X;
                    int index = Breaks.IndexOf(_selectedSlider);
                    if (x > gb.Right) x = gb.Right;
                    if (x < gb.Left) x = gb.Left;
                    if (index > 0)
                    {
                        if (x < Breaks[index - 1].Position + 2) x = (int)Breaks[index - 1].Position + 2;
                    }

                    if (index < Breaks.Count - 1)
                    {
                        if (x > Breaks[index + 1].Position - 2) x = (int)Breaks[index + 1].Position - 2;
                    }

                    _selectedSlider.Position = x;
                    rg.Union(_selectedSlider.Bounds);
                    Invalidate(rg);
                }

                return;
            }

            bool overSlider = false;
            foreach (BreakSlider slider in Breaks)
            {
                if (slider.Bounds.Contains(e.Location) || slider.HandleBounds.Contains(e.Location))
                {
                    overSlider = true;
                }
            }

            if (_dragCursor && !overSlider)
            {
                Cursor = Cursors.Arrow;
                _dragCursor = false;
            }

            if (!_dragCursor && overSlider)
            {
                _dragCursor = true;
                Cursor = Cursors.SizeWE;
            }

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Handles the mouse up event.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;

                _selectedSlider.Category.Maximum = _selectedSlider.Value;
                if (_isRaster)
                {
                    _rasterSymbolizer.Scheme.ApplySnapping(_selectedSlider.Category);
                    _selectedSlider.Category.ApplyMinMax(_rasterSymbolizer.Scheme.EditorSettings);
                }
                else
                {
                    _scheme.ApplySnapping(_selectedSlider.Category);
                    _selectedSlider.Category.ApplyMinMax(_scheme.EditorSettings);
                }

                if (_selectedSlider.NextCategory != null)
                {
                    _selectedSlider.NextCategory.Minimum = _selectedSlider.Value;
                    if (_isRaster)
                    {
                        _rasterSymbolizer.Scheme.ApplySnapping(_selectedSlider.NextCategory);
                        _selectedSlider.Category.ApplyMinMax(_rasterSymbolizer.Scheme.EditorSettings);
                    }
                    else
                    {
                        _scheme.ApplySnapping(_selectedSlider.NextCategory);
                        _selectedSlider.NextCategory.ApplyMinMax(_scheme.EditorSettings);
                    }
                }

                OnSliderMoved();
                Invalidate();
                return;
            }

            BreakSlider s = GetBreakAt(e.Location);
            SelectBreak(s);
            OnSliderSelected(s);
            Invalidate();

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Custom drawing.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle clip = e.ClipRectangle;
            if (clip.IsEmpty) clip = ClientRectangle;
            Bitmap bmp = new Bitmap(clip.Width, clip.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.TranslateTransform(-clip.X, -clip.Y);
            g.Clip = new Region(clip);
            g.Clear(BackColor);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            OnDraw(g, clip);
            g.Dispose();
            e.Graphics.DrawImage(bmp, clip, new Rectangle(0, 0, clip.Width, clip.Height), GraphicsUnit.Pixel);
        }

        /// <summary>
        /// Prevents flicker by preventing the white background being drawn here.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Occurs during a resize.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnResize(EventArgs e)
        {
            _graph.Width = Width;
            _graph.Height = Height;
            base.OnResize(e);
            Invalidate();
        }

        /// <summary>
        /// Fires the slider moved event.
        /// </summary>
        protected virtual void OnSliderMoved()
        {
            SliderMoved?.Invoke(this, new BreakSliderEventArgs(_selectedSlider));
        }

        /// <summary>
        /// Occurs when the slider moves.
        /// </summary>
        protected virtual void OnSliderMoving()
        {
            SliderMoving?.Invoke(this, new BreakSliderEventArgs(_selectedSlider));
        }

        /// <summary>
        /// Fires the SliderSelected event.
        /// </summary>
        /// <param name="slider">The break slider.</param>
        protected virtual void OnSliderSelected(BreakSlider slider)
        {
            SliderSelected?.Invoke(this, new BreakSliderEventArgs(slider));
        }

        /// <summary>
        /// Fires the statistics updated event.
        /// </summary>
        protected virtual void OnStatisticsUpdated()
        {
            StatisticsUpdated?.Invoke(this, new StatisticalEventArgs(_scheme.Statistics));
        }

        private void CategoryZoomClicked(object sender, EventArgs e)
        {
            ZoomToCategoryRange();
        }

        private void FillBins()
        {
            if (_values == null) return;
            double min = _graph.Minimum;
            double max = _graph.Maximum;
            if (min == max)
            {
                min = min - 10;
                max = max + 10;
            }

            double binSize = (max - min) / _graph.NumColumns;
            int numBins = _graph.NumColumns;
            int[] bins = new int[numBins];
            int maxBinCount = 0;
            foreach (double val in _values)
            {
                if (val < min || val > max) continue;
                int index = (int)Math.Ceiling((val - min) / binSize);
                if (index >= numBins) index = numBins - 1;
                bins[index]++;
                if (bins[index] > maxBinCount)
                {
                    maxBinCount = bins[index];
                }
            }

            _graph.MaxBinCount = maxBinCount;
            _graph.Bins = bins;
        }

        private bool IsValidField(string fieldName)
        {
            if (fieldName == null) return false;
            if (_source != null) return _source.GetColumn(fieldName) != null;
            return _table != null && _table.Columns.Contains(fieldName);
        }

        private void ReadValues()
        {
            if (_isRaster)
            {
                _values = _raster.GetRandomValues(_rasterSymbolizer.EditorSettings.MaxSampleCount);
                _statistics.Calculate(_values);
                if (_values == null) return;
                return;
            }

            _values = _scheme.Values;
            if (_values == null) return;
            _scheme.Statistics.Calculate(_values);
        }

        private void ResetZoom()
        {
            ResetExtents();
            FillBins();
            UpdateBreaks();
            Invalidate();
        }

        private void ResetZoomClicked(object sender, EventArgs e)
        {
            ResetZoom();
        }

        private void UpdateBins()
        {
            if (_isRaster)
            {
                if (_raster == null) return;
            }
            else
            {
                if (_source == null && _table == null) return;
                if (!IsValidField(_fieldName)) return;
            }

            ReadValues();
            FillBins();
            UpdateBreaks();
        }

        #endregion
    }
}