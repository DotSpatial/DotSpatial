// ********************************************************************************************************
// Product Name: DotSpatial.Drawing.PredefinedSymbols.dll Alpha
// Description:  The basic module for PredefinedSymbolControl version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from DotSpatial.Drawing.PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/15/2009 9:56:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
        #region Public Events

        /// <summary>
        /// Occurs when a symbol is selected
        /// </summary>
        public event EventHandler SymbolSelected;

        #endregion

        #region Private Variables

        private readonly IWindowsFormsEditorService _editorService;
        private readonly CustomLineSymbolProvider _provider;
        private int _cellMargin;
        private Size _cellSize;
        private string _defaultCategoryFilter = "All";
        private bool _dynamicColumns;
        private bool _isSelected;
        private string _mapCategory;
        private int _numColumns;
        private int _selectedIndex = -1;
        private Color _selectionBackColor;
        private Color _selectionForeColor;
        private bool _showSymbolNames = true;
        private List<CustomLineSymbolizer> _symbolizerList;
        private Font _textFont;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the predefined symbol control.
        /// </summary>
        public PredefinedLineSymbolControl()
        {
            _symbolizerList = new List<CustomLineSymbolizer>();
            _provider = new CustomLineSymbolProvider();
            //_symbolizerList = _provider.GetAllSymbols();

            Configure();
        }

        /// <summary>
        /// Creates a new instance of the predefined symbol control that uses the specific symbol provider
        /// </summary>
        /// <param name="prov">The provider class that is used to retrieve the predefined custom symbols from
        /// the XML file or another data source</param>
        public PredefinedLineSymbolControl(CustomLineSymbolProvider prov)
        {
            _provider = prov;
        }

        /// <summary>
        /// Creates a new instance of a Predefined symbol control designed to display a list of specific symbolizer
        /// </summary>
        /// <param name="editorService"></param>
        /// <param name="symbols"></param>
        public PredefinedLineSymbolControl(IWindowsFormsEditorService editorService, List<CustomLineSymbolizer> symbols)
        {
            _editorService = editorService;
            _provider = new CustomLineSymbolProvider();
            _symbolizerList = symbols;
            Configure();
        }

        private void Configure()
        {
            _symbolizerList = _provider.GetAllSymbols();

            _cellSize = new Size(50, 50);
            _textFont = new Font("Arial", 9);
            _cellMargin = 4;
            _numColumns = GetMaxNumColumns();
            _dynamicColumns = true;
            _selectionBackColor = Color.LightGray;
            _selectionForeColor = Color.White;
            DocumentRectangle = new Rectangle(0, 0, _cellSize.Width * 16, _cellSize.Height * 16);
            ResetScroll();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new symbolizer to the control. The added symbolizer will be selected
        /// by default
        /// </summary>
        /// <param name="newSymbolizer">The added custom symbolizer</param>
        public void AddSymbolizer(CustomLineSymbolizer newSymbolizer)
        {
            _symbolizerList.Add(newSymbolizer);
            SelectedIndex = _symbolizerList.Count - 1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cell size in pixels.
        /// </summary>
        public Size CellSize
        {
            get { return _cellSize; }
            set
            {
                _cellSize = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the margin of each symbolizer preview cell in pixels
        /// </summary>
        public int CellMargin
        {
            get { return _cellMargin; }
            set { _cellMargin = value; }
        }

        /// <summary>
        /// Overrides underlying behavior to hide it in the properties list for this control from serialization
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
        /// Gets the list of custom symbolizers displayed in this control
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<CustomLineSymbolizer> SymbolizerList
        {
            get
            {
                return _symbolizerList;
            }
            set
            {
                _symbolizerList = value;
            }
        }

        /// <summary>
        /// When the 'Category Filter' is set to this value then
        /// all available custom symbols are displayed
        /// </summary>
        public string DefaultCategoryFilter
        {
            get { return _defaultCategoryFilter; }
            set { _defaultCategoryFilter = value; }
        }

        /// <summary>
        /// Indicates whether the custom symbolizer names should be shown
        /// </summary>
        public bool ShowSymbolNames
        {
            get { return _showSymbolNames; }
            set { _showSymbolNames = value; }
        }

        /// <summary>
        /// Gets or sets the font to use for the text that describes the predifined symbol control
        /// </summary>
        public Font TextFont
        {
            get
            {
                return _textFont;
            }
            set
            {
                _textFont = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that, if true, indicates that this form should restructure the columns
        /// as it is resized so that all the columns are visible.
        /// </summary>
        public bool DynamicColumns
        {
            get { return _dynamicColumns; }
            set
            {
                _dynamicColumns = value;
            }
        }

        /// <summary>
        /// Gets or sets whether or not a feature symbolizer is selected
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
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
        /// Gets or sets the index of the selected symbolizer item
        /// </summary>
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (_selectedIndex >= _symbolizerList.Count)
                {
                    _selectedIndex = 0;
                }
                _selectedIndex = value;
                OnSymbolSelected();
            }
        }

        /// <summary>
        /// Gets or sets the number of columns
        /// </summary>
        public int NumColumns
        {
            get { return _numColumns; }
        }

        /// <summary>
        /// Gets the number of rows, which is controlled by having to show 256 cells
        /// in the given number of columns.
        /// </summary>
        public int NumRows
        {
            get
            {
                if (_numColumns == 0) return _symbolizerList.Count;
                return (int)Math.Ceiling(_symbolizerList.Count / (double)_numColumns);
            }
        }

        /// <summary>
        /// Gets or sets the background color for the selection
        /// </summary>
        public Color SelectionBackColor
        {
            get { return _selectionBackColor; }
            set { _selectionBackColor = value; }
        }

        /// <summary>
        /// The Font Color for the selection
        /// </summary>
        public Color SelectionForeColor
        {
            get { return _selectionForeColor; }
            set { _selectionForeColor = value; }
        }

        /// <summary>
        /// Gets or sets the selected symbolizer
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CustomLineSymbolizer SelectedSymbolizer
        {
            get
            {
                if (_symbolizerList.Count == 0 || SelectedIndex < 0)
                {
                    return null;
                }

                if (_selectedIndex >= _symbolizerList.Count)
                {
                    _selectedIndex = _symbolizerList.Count - 1;
                }
                return _symbolizerList[_selectedIndex];
            }
        }

        /// <summary>
        /// The category of symbol displayed in the control
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
                else if (value.ToLower() == _defaultCategoryFilter.ToLower())
                {
                    _mapCategory = String.Empty;
                }
                else
                {
                    _mapCategory = value;
                }
                _symbolizerList = _provider.GetSymbolsByCategory(_mapCategory);
                _selectedIndex = 0;

                Invalidate();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the control contains the specified symbolizer
        /// </summary>
        /// <param name="symbolizer">the line symbolizer to be checked</param>
        /// <returns>true if found, false otherwise</returns>
        public bool ContainsSymbolizer(ILineSymbolizer symbolizer)
        {
            foreach (ILineSymbolizer sym in _symbolizerList)
            {
                if (symbolizer == sym)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Saves the list of symbolizers to a file using serialization
        /// (not yet implemented)
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            //_provider.Save(fileName);
        }

        /// <summary>
        /// Loads the list of symbolizers from the serialized file
        /// (not yet implemented)
        /// </summary>
        /// <param name="fileName">The file name from which to load</param>
        public void Load(string fileName)
        {
            //_symbolizerList = (List<CustomLineSymbolizer>)_provider.Load(fileName);
            //Refresh();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Handles the situation where a mouse up should show a magnified version of the character.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.X < 0) return;
            if (e.Y < 0) return;
            int col = (e.X + ControlRectangle.X) / _cellSize.Width;
            int row = (e.Y + ControlRectangle.Y) / _cellSize.Height;

            if ((_numColumns * row + col) < 256)
            {
                _isSelected = true;
                _selectedIndex = (_numColumns * row + col);
                OnSymbolSelected();
            }
            Invalidate();
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the SymbolSelected event args, and closes a drop down editor if it exists.
        /// </summary>
        protected virtual void OnSymbolSelected()
        {
            if (_editorService != null)
            {
                _editorService.CloseDropDown();
            }
            if (SymbolSelected != null) SymbolSelected(this, EventArgs.Empty);
        }

        /// <summary>
        /// Takes place when the control is initialized or invalidated
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialize(PaintEventArgs e)
        {
            if (_cellSize.Width == 0)
            {
                _cellSize.Width = 50;
                _cellSize.Height = 50;
            }

            if (_textFont == null)
            {
                _textFont = new Font("Arial", 9);
            }

            DocumentRectangle = new Rectangle(0, 0, _numColumns * _cellSize.Width + 1, NumRows * _cellSize.Height + 1);

            if (_dynamicColumns)
            {
                int newColumns = (Width - 20) / _cellSize.Width;
                if (newColumns != _numColumns)
                {
                    e.Graphics.FillRectangle(Brushes.White, e.ClipRectangle);
                    _numColumns = newColumns;
                    Invalidate();
                    return;
                }
            }
            if (_numColumns == 0)
            {
                _numColumns = 1;
            }

            for (int i = 0; i < _symbolizerList.Count; i++)
            {
                int row = i / _numColumns;
                int col = i % _numColumns;

                CustomSymbolizer sym = _symbolizerList[i];
                Point pointLocation = new Point(col * _cellSize.Width, row * _cellSize.Height);
                Rectangle rect = new Rectangle(pointLocation, _cellSize);
                DrawSymbolizer(e.Graphics, rect, sym);
            }

            Brush backBrush = new SolidBrush(_selectionBackColor);
            Brush foreBrush = new SolidBrush(_selectionForeColor);

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
        /// Occurs whenever this control is resized, and forces invalidation of the entire control because
        /// we are completely changing how the paging works.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        #endregion

        #region Private Methods

        //gets the maximum available number of columns based on cell margin and width
        private int GetMaxNumColumns()
        {
            return (int)(Width / (double)(_cellSize.Width + _cellMargin));
        }

        /// <summary>
        /// Draws a symbolizer inside the specified rectangle including margins
        /// </summary>
        /// <param name="g">The graphics device to draw to</param>
        /// <param name="rect">The Rectangle describing where to draw</param>
        /// <param name="sym">The IFeatureSymbolizer to draw</param>
        private void DrawSymbolizer(Graphics g, Rectangle rect, ICustomSymbolizer sym)
        {
            int textHeight = GetStringHeight(g, sym.Name);
            int innerCellWidth = _cellSize.Width - 2 * _cellMargin;
            int innerCellHeight = _cellSize.Height - 2 * _cellMargin - textHeight;

            Rectangle newRect = new Rectangle(rect.Left + CellMargin, rect.Top + CellMargin, innerCellWidth, innerCellHeight);
            sym.Symbolizer.Draw(g, newRect);

            if (_showSymbolNames)
            {
                StringFormat fmt = new StringFormat();
                //fmt.Alignment = StringAlignment.Center;
                //fmt.LineAlignment = StringAlignment.Center;

                PointF textLocation = new Point(rect.Left, rect.Bottom - textHeight);
                g.DrawString(sym.Name, _textFont, Brushes.Black, textLocation, fmt);
            }
        }

        private int GetStringHeight(Graphics g, string str)
        {
            if (_showSymbolNames)
            {
                return Convert.ToInt32(g.MeasureString(str, _textFont).Height);
            }
            return 0;
        }

        private Rectangle GetSelectedRectangle()
        {
            int row = _selectedIndex / _numColumns;
            int col = _selectedIndex % _numColumns;

            return new Rectangle(col * _cellSize.Width, row * _cellSize.Height, _cellSize.Width, CellSize.Height);
        }

        #endregion
    }
}