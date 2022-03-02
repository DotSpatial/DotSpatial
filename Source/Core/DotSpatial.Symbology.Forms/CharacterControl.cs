// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// CharacterControl.
    /// </summary>
    [DefaultEvent("PopupClicked")]
    public class CharacterControl : VerticalScrollControl
    {
        #region Fields
        private readonly IWindowsFormsEditorService _editorService;
        private readonly ICharacterSymbol _symbol;
        private Size _cellSize;
        private bool _isPopup;
        private int _numColumns;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterControl"/> class.
        /// </summary>
        public CharacterControl()
        {
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterControl"/> class designed to edit the specific symbol.
        /// </summary>
        /// <param name="editorService">The editor service.</param>
        /// <param name="symbol">The character symbol.</param>
        public CharacterControl(IWindowsFormsEditorService editorService, ICharacterSymbol symbol)
        {
            _editorService = editorService;
            _symbol = symbol;
            Configure();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a magnification box is clicked.
        /// </summary>
        public event EventHandler PopupClicked;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cell size.
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
        /// Gets or sets t he document rectangle. This overrides underlying the behavior to hide it in the properties list for this control from serialization.
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
        /// Gets or sets a value indicating whether or not this item is selected.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the number of columns.
        /// </summary>
        public int NumColumns
        {
            get
            {
                return _numColumns;
            }

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
        /// Gets or sets the selected character.
        /// </summary>
        public byte SelectedChar { get; set; }

        /// <summary>
        /// Gets the string form of the selected character.
        /// </summary>
        public string SelectedString => ((char)(SelectedChar + (TypeSet * 256))).ToString();

        /// <summary>
        /// Gets or sets the background color for the selection.
        /// </summary>
        public Color SelectionBackColor { get; set; }

        /// <summary>
        /// Gets or sets the Font Color for the selection.
        /// </summary>
        public Color SelectionForeColor { get; set; }

        /// <summary>
        /// Gets or sets the byte that describes the "larger" of the two bytes for a unicode set.
        /// The 256 character slots illustrate the sub-categories for those elements.
        /// </summary>
        public byte TypeSet { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the character control.
        /// </summary>
        /// <param name="e">The paint event args.</param>
        protected override void OnInitialize(PaintEventArgs e)
        {
            DocumentRectangle = new Rectangle(0, 0, (_numColumns * _cellSize.Width) + 1, (NumRows * _cellSize.Height) + 1);
            if (DynamicColumns)
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
                string text = ((char)((TypeSet * 256) + i)).ToString();
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

            Brush backBrush = new SolidBrush(SelectionBackColor);
            Brush foreBrush = new SolidBrush(SelectionForeColor);
            if (IsSelected)
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
        /// Handles the situation where a mouse up should show a magnified version of the character.
        /// </summary>
        /// <param name="e">The event args.</param>
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
            if (((_numColumns * row) + col) < 256)
            {
                IsSelected = true;
                SelectedChar = (byte)((_numColumns * row) + col);
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

            PopupClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs whenever this control is resized, and forces invalidation of the entire control because
        /// we are completely changing how the paging works.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnResize(EventArgs e)
        {
            _isPopup = false;
            base.OnResize(e);
            Invalidate();
        }

        private void Configure()
        {
            Font = new Font("DotSpatialSymbols", 18F, GraphicsUnit.Pixel);
            TypeSet = 0;
            _cellSize = new Size(22, 22);
            _numColumns = 16;
            DynamicColumns = true;
            SelectionBackColor = Color.CornflowerBlue;
            SelectionForeColor = Color.White;
            DocumentRectangle = new Rectangle(0, 0, _cellSize.Width * 16, _cellSize.Height * 16);
            ResetScroll();
        }

        private Rectangle GetPopupRectangle()
        {
            int pr = SelectedChar / _numColumns;
            int pc = SelectedChar % _numColumns;
            if (pc == 0) pc = 1;
            if (pc == _numColumns - 1) pc = _numColumns - 2;
            if (pr == 0) pr = 1;
            if (pr == NumRows - 1) pr = NumRows - 2;
            return new Rectangle((pc - 1) * CellSize.Width, (pr - 1) * CellSize.Height, CellSize.Width * 3, CellSize.Height * 3);
        }

        private Rectangle GetSelectedRectangle()
        {
            int row = SelectedChar / _numColumns;
            int col = SelectedChar % _numColumns;

            return new Rectangle((col * _cellSize.Width) + 1, (row * _cellSize.Height) + 1, _cellSize.Width - 1, CellSize.Height - 1);
        }

        #endregion
    }
}