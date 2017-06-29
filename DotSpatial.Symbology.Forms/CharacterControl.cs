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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/3/2009 10:46:05 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// CharacterControl
    /// </summary>
    [DefaultEvent("PopupClicked")]
    public class CharacterControl : VerticalScrollControl
    {
        #region Public Events

        /// <summary>
        /// Occurs when a magnification box is clicked
        /// </summary>
        public event EventHandler PopupClicked;

        #endregion

        #region Private Variables

        private Size _cellSize;
        private bool _dynamicColumns;
        private IWindowsFormsEditorService _editorService;
        private bool _isPopup;
        private bool _isSelected;
        private int _numColumns;
        private byte _selectedChar;
        private Color _selectionBackColor;
        private Color _selectionForeColor;
        private ICharacterSymbol _symbol;
        private byte _typeSet;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CharacterControl
        /// </summary>
        public CharacterControl()
        {
            Configure();
        }

        /// <summary>
        /// Creates a new instance of a CharacterControl designed to edit the specific symbol
        /// </summary>
        /// <param name="editorService"></param>
        /// <param name="symbol"></param>
        public CharacterControl(IWindowsFormsEditorService editorService, ICharacterSymbol symbol)
        {
            _editorService = editorService;
            _symbol = symbol;
            Configure();
        }

        private void Configure()
        {
            Font = new Font("DotSpatialSymbols", 18F, GraphicsUnit.Pixel);
            _typeSet = 0;
            _cellSize = new Size(22, 22);
            _numColumns = 16;
            _dynamicColumns = true;
            _selectionBackColor = Color.CornflowerBlue;
            _selectionForeColor = Color.White;
            DocumentRectangle = new Rectangle(0, 0, _cellSize.Width * 16, _cellSize.Height * 16);
            ResetScroll();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cell size.
        /// </summary>
        public Size CellSize
        {
            get { return _cellSize; }
            set
            {
                _cellSize = value;
                // base.DocumentRectangle = new Rectangle(0, 0, (int)_cellSize.Width * 16, (int)_cellSize.Height * 16);
                Invalidate();
            }
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
        /// Gets or sets whether or not this item is selected
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; }
        }

        /// <summary>
        /// Gets or sets the number of columns
        /// </summary>
        public int NumColumns
        {
            get { return _numColumns; }
            set
            {
                _numColumns = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets the number of rows, which is controlled by having to show 256 cells
        /// in the given number of columns.
        /// </summary>
        public int NumRows
        {
            get
            {
                if (_numColumns == 0) return 256;
                return (int)Math.Ceiling(256 / (double)_numColumns);
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
        /// Gets or sets the byte that describes the "larger" of the two bytes for a unicode set.
        /// The 256 character slots illustrate the sub-categories for those elements.
        /// </summary>
        public byte TypeSet
        {
            get { return _typeSet; }
            set { _typeSet = value; }
        }

        /// <summary>
        /// Gets or sets the selected character
        /// </summary>
        public byte SelectedChar
        {
            get { return _selectedChar; }
            set { _selectedChar = value; }
        }

        /// <summary>
        /// Gets the string form of the selected character
        /// </summary>
        public string SelectedString
        {
            get { return ((char)(_selectedChar + _typeSet * 256)).ToString(); }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Handles the situation where a mouse up should show a magnified version of the character.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_isPopup)
            {
                Rectangle pRect = GetPopupRectangle();
                Rectangle cRect = DocumentToClient(pRect);
                if (cRect.Contains(e.Location))
                {
                    _isPopup = false; // make the popup vanish, but don't change the selection.
                    cRect.Inflate(CellSize);
                    Invalidate(cRect);
                    OnPopupClicked();
                    return;
                }
            }

            if (e.X < 0) return;
            if (e.Y < 0) return;
            int col = (e.X + ControlRectangle.X) / _cellSize.Width;
            int row = (e.Y + ControlRectangle.Y) / _cellSize.Height;
            if ((_numColumns * row + col) < 256)
            {
                _isSelected = true;
                _selectedChar = (byte)(_numColumns * row + col);
                _isPopup = true;
            }
            Invalidate();
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the PopupClicked event args, and closes a drop down editor if it exists.
        /// </summary>
        protected virtual void OnPopupClicked()
        {
            if (_editorService != null)
            {
                _symbol.Code = SelectedChar;
                _editorService.CloseDropDown();
            }
            if (PopupClicked != null) PopupClicked(this, EventArgs.Empty);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitialize(PaintEventArgs e)
        {
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
            if (_numColumns == 0) _numColumns = 1;
            Font smallFont = new Font(Font.FontFamily, CellSize.Width * .8F, GraphicsUnit.Pixel);
            for (int i = 0; i < 256; i++)
            {
                int row = i / _numColumns;
                int col = i % _numColumns;
                string text = ((char)(_typeSet * 256 + i)).ToString();
                e.Graphics.DrawString(text, smallFont, Brushes.Black, new PointF(col * _cellSize.Width, row * _cellSize.Height));
            }
            for (int col = 0; col <= _numColumns; col++)
            {
                e.Graphics.DrawLine(Pens.Black, new PointF(col * _cellSize.Width, 0), new PointF(col * _cellSize.Width, NumRows * _cellSize.Height));
            }
            for (int row = 0; row <= NumRows; row++)
            {
                e.Graphics.DrawLine(Pens.Black, new PointF(0, row * _cellSize.Height), new PointF(NumColumns * _cellSize.Width, row * _cellSize.Height));
            }

            Brush backBrush = new SolidBrush(_selectionBackColor);
            Brush foreBrush = new SolidBrush(_selectionForeColor);
            if (_isSelected)
            {
                Rectangle sRect = GetSelectedRectangle();
                e.Graphics.FillRectangle(backBrush, sRect);
                e.Graphics.DrawString(SelectedString, smallFont, foreBrush, new PointF(sRect.X, sRect.Y));
            }

            if (_isPopup)
            {
                Rectangle pRect = GetPopupRectangle();
                e.Graphics.FillRectangle(Brushes.Gray, new Rectangle(pRect.X + 2, pRect.Y + 2, pRect.Width, pRect.Height));
                e.Graphics.FillRectangle(backBrush, pRect);
                e.Graphics.DrawRectangle(Pens.Black, pRect);
                Font bigFont = new Font(Font.FontFamily, CellSize.Width * 2.7F, GraphicsUnit.Pixel);
                e.Graphics.DrawString(SelectedString, bigFont, foreBrush, new PointF(pRect.X, pRect.Y));
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
            _isPopup = false;
            base.OnResize(e);
            Invalidate();
        }

        #endregion

        #region Private Methods

        private Rectangle GetPopupRectangle()
        {
            int pr = _selectedChar / _numColumns;
            int pc = _selectedChar % _numColumns;
            if (pc == 0) pc = 1;
            if (pc == _numColumns - 1) pc = _numColumns - 2;
            if (pr == 0) pr = 1;
            if (pr == NumRows - 1) pr = NumRows - 2;
            return new Rectangle((pc - 1) * CellSize.Width, (pr - 1) * CellSize.Height, CellSize.Width * 3, CellSize.Height * 3);
        }

        private Rectangle GetSelectedRectangle()
        {
            int row = _selectedChar / _numColumns;
            int col = _selectedChar % _numColumns;

            return new Rectangle(col * _cellSize.Width + 1, row * _cellSize.Height + 1, _cellSize.Width - 1, CellSize.Height - 1);
        }

        #endregion
    }
}