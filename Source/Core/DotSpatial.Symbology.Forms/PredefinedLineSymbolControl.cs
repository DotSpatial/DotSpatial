// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This control shows a list of predefined symbols with their names and preview.
    /// </summary>
    [DefaultEvent("SymbolSelected")]
    public class PredefinedLineSymbolControl : VerticalScrollControl
    {
        #region Fields

        private readonly IWindowsFormsEditorService _editorService;
        private readonly CustomLineSymbolProvider _provider;
        private Size _cellSize;
        private bool _isSelected;
        private string _mapCategory;
        private int _selectedIndex = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedLineSymbolControl"/> class.
        /// </summary>
        public PredefinedLineSymbolControl()
        {
            SymbolizerList = new List<CustomLineSymbolizer>();
            _provider = new CustomLineSymbolProvider();
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedLineSymbolControl"/> class that uses the specific symbol provider.
        /// </summary>
        /// <param name="prov">The provider class that is used to retrieve the predefined custom symbols from
        /// the XML file or another data source.</param>
        public PredefinedLineSymbolControl(CustomLineSymbolProvider prov)
        {
            _provider = prov;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedLineSymbolControl"/> class.
        /// </summary>
        /// <param name="editorService">The windows forms editor service.</param>
        /// <param name="symbols">The custom line symbolizers.</param>
        public PredefinedLineSymbolControl(IWindowsFormsEditorService editorService, List<CustomLineSymbolizer> symbols)
        {
            _editorService = editorService;
            _provider = new CustomLineSymbolProvider();
            SymbolizerList = symbols;
            Configure();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a symbol is selected
        /// </summary>
        public event EventHandler SymbolSelected;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the category of symbol displayed in the control.
        /// </summary>
        public string CategoryFilter
        {
            get
            {
                return _mapCategory;
            }

            set
            {
                if (value == null)
                {
                    _mapCategory = string.Empty;
                }
                else if (value.ToLower() == DefaultCategoryFilter.ToLower())
                {
                    _mapCategory = string.Empty;
                }
                else
                {
                    _mapCategory = value;
                }

                SymbolizerList = _provider.GetSymbolsByCategory(_mapCategory);
                _selectedIndex = 0;

                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the margin of each symbolizer preview cell in pixels.
        /// </summary>
        public int CellMargin { get; set; }

        /// <summary>
        /// Gets or sets the cell size in pixels.
        /// </summary>
        public Size CellSize
        {
            get
            {
                return _cellSize;
            }

            set
            {
                _cellSize = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the default category filter. When the 'Category Filter' is set to this value then
        /// all available custom symbols are displayed.
        /// </summary>
        public string DefaultCategoryFilter { get; set; } = "All";

        /// <summary>
        /// Gets or sets the document rectangle. This overrides underlying behavior to hide it in the properties list for this control from serialization.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Rectangle DocumentRectangle
        {
            get
            {
                return base.DocumentRectangle;
            }

            set
            {
                base.DocumentRectangle = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this form should restructure the columns
        /// as it is resized so that all the columns are visible.
        /// </summary>
        public bool DynamicColumns { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not a feature symbolizer is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }

            set
            {
                _isSelected = value;
                if (!_isSelected)
                {
                    _selectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int NumColumns { get; private set; }

        /// <summary>
        /// Gets the number of rows, which is controlled by having to show 256 cells
        /// in the given number of columns.
        /// </summary>
        public int NumRows
        {
            get
            {
                if (NumColumns == 0) return SymbolizerList.Count;
                return (int)Math.Ceiling(SymbolizerList.Count / (double)NumColumns);
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected symbolizer item.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }

            set
            {
                if (_selectedIndex >= SymbolizerList.Count)
                {
                    _selectedIndex = 0;
                }

                _selectedIndex = value;
                OnSymbolSelected();
            }
        }

        /// <summary>
        /// Gets the selected symbolizer.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomLineSymbolizer SelectedSymbolizer
        {
            get
            {
                if (SymbolizerList.Count == 0 || SelectedIndex < 0)
                {
                    return null;
                }

                if (_selectedIndex >= SymbolizerList.Count)
                {
                    _selectedIndex = SymbolizerList.Count - 1;
                }

                return SymbolizerList[_selectedIndex];
            }
        }

        /// <summary>
        /// Gets or sets the background color for the selection.
        /// </summary>
        public Color SelectionBackColor { get; set; }

        /// <summary>
        /// Gets or sets the Font Color for the selection.
        /// </summary>
        public Color SelectionForeColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the custom symbolizer names should be shown.
        /// </summary>
        public bool ShowSymbolNames { get; set; } = true;

        /// <summary>
        /// Gets or sets the list of custom symbolizers displayed in this control.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<CustomLineSymbolizer> SymbolizerList { get; set; }

        /// <summary>
        /// Gets or sets the font to use for the text that describes the predifined symbol control.
        /// </summary>
        public Font TextFont { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new symbolizer to the control. The added symbolizer will be selected by default.
        /// </summary>
        /// <param name="newSymbolizer">The added custom symbolizer.</param>
        public void AddSymbolizer(CustomLineSymbolizer newSymbolizer)
        {
            SymbolizerList.Add(newSymbolizer);
            SelectedIndex = SymbolizerList.Count - 1;
        }

        /// <summary>
        /// Checks if the control contains the specified symbolizer.
        /// </summary>
        /// <param name="symbolizer">the line symbolizer to be checked.</param>
        /// <returns>true if found, false otherwise.</returns>
        public bool ContainsSymbolizer(ILineSymbolizer symbolizer)
        {
            foreach (ILineSymbolizer sym in SymbolizerList)
            {
                if (symbolizer == sym)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Loads the list of symbolizers from the serialized file
        /// (not yet implemented).
        /// </summary>
        /// <param name="fileName">The file name from which to load.</param>
        public void Load(string fileName)
        {
        }

        /// <summary>
        /// Saves the list of symbolizers to a file using serialization
        /// (not yet implemented).
        /// </summary>
        /// <param name="fileName">The file name to save to.</param>
        public void Save(string fileName)
        {
        }

        /// <summary>
        /// Takes place when the control is initialized or invalidated.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnInitialize(PaintEventArgs e)
        {
            if (_cellSize.Width == 0)
            {
                _cellSize.Width = 50;
                _cellSize.Height = 50;
            }

            if (TextFont == null)
            {
                TextFont = new Font("Arial", 9);
            }

            DocumentRectangle = new Rectangle(0, 0, (NumColumns * _cellSize.Width) + 1, (NumRows * _cellSize.Height) + 1);

            if (DynamicColumns)
            {
                int newColumns = (Width - 20) / _cellSize.Width;
                if (newColumns != NumColumns)
                {
                    e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);
                    NumColumns = newColumns;
                    Invalidate();
                    return;
                }
            }

            if (NumColumns == 0)
            {
                NumColumns = 1;
            }

            for (int i = 0; i < SymbolizerList.Count; i++)
            {
                int row = i / NumColumns;
                int col = i % NumColumns;

                CustomSymbolizer sym = SymbolizerList[i];
                Point pointLocation = new Point(col * _cellSize.Width, row * _cellSize.Height);
                Rectangle rect = new Rectangle(pointLocation, _cellSize);
                DrawSymbolizer(e.Graphics, rect, sym);
            }

            Brush backBrush = new SolidBrush(SelectionBackColor);
            Brush foreBrush = new SolidBrush(SelectionForeColor);

            if (_isSelected)
            {
                Rectangle sRect = GetSelectedRectangle();
                e.Graphics.FillRectangle(backBrush, sRect);
                if (SelectedSymbolizer != null)
                {
                    DrawSymbolizer(e.Graphics, sRect, SelectedSymbolizer);
                    OnSymbolSelected();
                }
            }

            backBrush.Dispose();
            foreBrush.Dispose();
        }

        /// <summary>
        /// Handles the situation where a mouse up should show a magnified version of the character.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.X < 0) return;
            if (e.Y < 0) return;
            int col = (e.X + ControlRectangle.X) / _cellSize.Width;
            int row = (e.Y + ControlRectangle.Y) / _cellSize.Height;

            if (((NumColumns * row) + col) < 256)
            {
                _isSelected = true;
                _selectedIndex = (NumColumns * row) + col;
                OnSymbolSelected();
            }

            Invalidate();
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Occurs whenever this control is resized, and forces invalidation of the entire control because
        /// we are completely changing how the paging works.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        /// <summary>
        /// Fires the SymbolSelected event args, and closes a drop down editor if it exists.
        /// </summary>
        protected virtual void OnSymbolSelected()
        {
            _editorService?.CloseDropDown();
            SymbolSelected?.Invoke(this, EventArgs.Empty);
        }

        private void Configure()
        {
            SymbolizerList = _provider.GetAllSymbols();

            _cellSize = new Size(50, 50);
            TextFont = new Font("Arial", 9);
            CellMargin = 4;
            NumColumns = GetMaxNumColumns();
            DynamicColumns = true;
            SelectionBackColor = Color.LightGray;
            SelectionForeColor = Color.White;
            DocumentRectangle = new Rectangle(0, 0, _cellSize.Width * 16, _cellSize.Height * 16);
            ResetScroll();
        }

        /// <summary>
        /// Draws a symbolizer inside the specified rectangle including margins.
        /// </summary>
        /// <param name="g">The graphics device to draw to.</param>
        /// <param name="rect">The Rectangle describing where to draw.</param>
        /// <param name="sym">The IFeatureSymbolizer to draw.</param>
        private void DrawSymbolizer(Graphics g, Rectangle rect, ICustomSymbolizer sym)
        {
            int textHeight = GetStringHeight(g, sym.Name);
            int innerCellWidth = _cellSize.Width - (2 * CellMargin);
            int innerCellHeight = _cellSize.Height - (2 * CellMargin) - textHeight;

            Rectangle newRect = new Rectangle(rect.Left + CellMargin, rect.Top + CellMargin, innerCellWidth, innerCellHeight);
            sym.Symbolizer.Draw(g, newRect);

            if (ShowSymbolNames)
            {
                StringFormat fmt = new StringFormat();
                PointF textLocation = new Point(rect.Left, rect.Bottom - textHeight);
                g.DrawString(sym.Name, TextFont, Brushes.Black, textLocation, fmt);
            }
        }

        // gets the maximum available number of columns based on cell margin and width
        private int GetMaxNumColumns()
        {
            return (int)(Width / (double)(_cellSize.Width + CellMargin));
        }

        private Rectangle GetSelectedRectangle()
        {
            int row = _selectedIndex / NumColumns;
            int col = _selectedIndex % NumColumns;

            return new Rectangle(col * _cellSize.Width, row * _cellSize.Height, _cellSize.Width, CellSize.Height);
        }

        private int GetStringHeight(Graphics g, string str)
        {
            if (ShowSymbolNames)
            {
                return Convert.ToInt32(g.MeasureString(str, TextFont).Height);
            }

            return 0;
        }

        #endregion
    }
}