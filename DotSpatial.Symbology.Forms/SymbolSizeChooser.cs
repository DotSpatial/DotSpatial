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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/14/2009 11:22:04 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SymbolSizeChooser
    /// </summary>
    [DefaultEvent("SelectedSizeChanged"),
    ToolboxItem(false)]
    public class SymbolSizeChooser : Control, ISupportInitialize
    {
        #region ISupportInitialize Members

        /// <summary>
        /// Prevent redundant updates to the boxes every time a property is changed
        /// </summary>
        public void BeginInit()
        {
            _isInitializing = true;
        }

        /// <summary>
        /// Enable live updating so that from now on, changes rebuild boxes.
        /// </summary>
        public void EndInit()
        {
            _isInitializing = false;
            RefreshBoxes();
        }

        #endregion

        /// <summary>
        /// Occurs when the selected size has changed
        /// </summary>
        public event EventHandler SelectedSizeChanged;

        #region Private Variables

        private Color _boxBackColor;
        private Color _boxSelectionColor;
        private Size _boxSize;
        private List<SizeBox> _boxes;
        private bool _isInitializing;
        private Size2D _maxSize;
        private Size2D _minSize;
        private int _numBoxes;
        private Orientation _orientation;
        private int _roundingRadius;
        private Size2D _selectedSize;
        private ISymbol _symbol;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SymbolSizeChooser
        /// </summary>
        public SymbolSizeChooser()
        {
            Configure();
        }

        /// <summary>
        /// Gets or sets a new SymbolSize chooser, specifying the symbol as part of the constructor.
        /// </summary>
        /// <param name="symbol">The symbol to draw.</param>
        public SymbolSizeChooser(ISymbol symbol)
        {
            Configure();
            _symbol = symbol;
        }

        private void Configure()
        {
            _boxes = new List<SizeBox>();
            _numBoxes = 4;
            _minSize = new Size2D(4, 4);
            _maxSize = new Size2D(30, 30);
            _selectedSize = _minSize.Copy();
            _boxSize = new Size(36, 36);
            _boxBackColor = SystemColors.Control;
            _boxSelectionColor = SystemColors.Highlight;
            _symbol = new SimpleSymbol();
            _orientation = Orientation.Horizontal;
            _roundingRadius = 6;
            RefreshBoxes();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Forces the box sub-categories to refresh given the new content.
        /// </summary>
        public virtual void RefreshBoxes()
        {
            _boxes = new List<SizeBox>();
            for (int i = 0; i < NumBoxes; i++)
            {
                CreateBox(i);
            }
            Invalidate();
        }

        private void CreateBox(int i)
        {
            SizeBox sb = new SizeBox();
            int x = 1;
            int y = 1;
            if (_orientation == Orientation.Horizontal)
            {
                x = (_boxSize.Width + 2) * i + 1;
            }
            else
            {
                y = (_boxSize.Height + 2) * i + 1;
            }
            sb.Bounds = new Rectangle(x, y, _boxSize.Width, _boxSize.Height);
            sb.BackColor = _boxBackColor;
            sb.SelectionColor = _boxSelectionColor;
            sb.RoundingRadius = _roundingRadius;

            if (i == 0)
            {
                if (_minSize != null)
                {
                    sb.Size = _minSize.Copy();
                }
                else
                {
                    sb.Size = new Size2D(4, 4);
                }
            }
            else if (i == _numBoxes - 1)
            {
                if (_maxSize != null)
                {
                    sb.Size = _maxSize.Copy();
                }
                else
                {
                    sb.Size = new Size2D(32, 32);
                }
            }
            else
            {
                if (_minSize != null && _maxSize != null)
                {
                    // because of the elses, we know that the number must be greater than 2, and that the current item is not the min or max
                    // Use squaring so that bigger sizes have larger differences between them.
                    double cw = (_maxSize.Width - _minSize.Width) / (_numBoxes);
                    double ch = (_maxSize.Height - _minSize.Height) / (_numBoxes);
                    sb.Size = new Size2D(_minSize.Width + cw * i, _minSize.Height + ch * i);
                }
                else
                {
                    sb.Size = new Size2D(16, 16);
                }
            }

            _boxes.Add(sb);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the normal background color for the boxes.
        /// </summary>
        [Description("Gets or sets the normal background color for the boxes.")]
        public Color BoxBackColor
        {
            get { return _boxBackColor; }
            set
            {
                _boxBackColor = value;
                if (!_isInitializing) RefreshBoxes();
            }
        }

        /// <summary>
        /// Gets or sets the box selection color
        /// </summary>
        [Description("Gets or sets the box selection color")]
        public Color BoxSelectionColor
        {
            get { return _boxSelectionColor; }
            set
            {
                _boxSelectionColor = value;
                if (!_isInitializing) RefreshBoxes();
            }
        }

        /// <summary>
        /// Gets or sets the rectangular extent for all the boxes.  This is not the size of the symbol.
        /// </summary>
        [Description("Gets or sets the rectangular extent for all the boxes.  This is not the size of the symbol.")]
        public Size BoxSize
        {
            get { return _boxSize; }
            set
            {
                _boxSize = value;
                if (!_isInitializing) RefreshBoxes();
            }
        }

        /// <summary>
        /// Gets or sets the maximum symbol size.
        /// </summary>
        [Description("Gets or sets the maximum symbol size."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Size2D MaximumSymbolSize
        {
            get { return _maxSize; }
            set
            {
                _maxSize = value;
                if (!_isInitializing) RefreshBoxes();
            }
        }

        /// <summary>
        /// Gets or sets the minimum symbol size
        /// </summary>
        [Description("Gets or sets the minimum symbol size"),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Size2D MinimumSymbolSize
        {
            get { return _minSize; }
            set
            {
                _minSize = value;
                if (!_isInitializing) RefreshBoxes();
            }
        }

        /// <summary>
        /// Gets or sets whether the boxes are drawn horizontally or vertically.
        /// </summary>
        [Description("Gets or sets whether the boxes are drawn horizontally or vertically.")]
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                _orientation = value;
                if (!_isInitializing) RefreshBoxes();
            }
        }

        /// <summary>
        /// Gets or sets the number of boxes
        /// </summary>
        [Description("Gets or sets the number of boxes")]
        public int NumBoxes
        {
            get { return _numBoxes; }
            set
            {
                _numBoxes = value;
                if (!_isInitializing) RefreshBoxes();
            }
        }

        /// <summary>
        /// Gets or sets the symbol to use for this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISymbol Symbol
        {
            get { return _symbol; }
            set
            {
                _symbol = value;
                foreach (SizeBox sb in _boxes)
                {
                    if (_symbol.Size == sb.Size)
                    {
                        sb.IsSelected = true;
                    }
                    else
                    {
                        sb.IsSelected = false;
                    }
                }
                if (!_isInitializing) Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the rounding radius for the boxes
        /// </summary>
        [Description("Gets or sets the rounding radius for the boxes")]
        public int RoundingRadius
        {
            get { return _roundingRadius; }
            set
            {
                _roundingRadius = value;
                if (!_isInitializing) RefreshBoxes();
            }
        }

        /// <summary>
        /// Gets or sets the currently selected size.
        /// </summary>
        [Description("Gets or sets the currently selected size."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Size2D SelectedSize
        {
            get { return _selectedSize; }
            set
            {
                _selectedSize = value;
                if (!_isInitializing) Invalidate();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the selected size changed event
        /// </summary>
        protected virtual void OnSelectedSizeChanged()
        {
            if (SelectedSizeChanged != null) SelectedSizeChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Handles the mouse up situation
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            bool changed = false;
            foreach (SizeBox sb in _boxes)
            {
                if (sb.Bounds.Contains(e.Location))
                {
                    if (sb.IsSelected == false)
                    {
                        sb.IsSelected = true;
                        _selectedSize = sb.Size.Copy();
                        _symbol.Size.Height = sb.Size.Height;
                        _symbol.Size.Width = sb.Size.Width;
                        changed = true;
                    }
                }
                else
                {
                    if (sb.IsSelected)
                    {
                        sb.IsSelected = false;
                    }
                }
            }
            Invalidate();
            if (changed) OnSelectedSizeChanged();
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Occurs durring drawing but is overridable by subclasses
        /// </summary>
        /// <param name="g"></param>
        /// <param name="clip"></param>
        protected virtual void OnDraw(Graphics g, Rectangle clip)
        {
            foreach (SizeBox sb in _boxes)
            {
                sb.Draw(g, clip, _symbol);
            }
        }

        /// <summary>
        /// Prevent flicker
        /// </summary>
        /// <param name="pevent"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Occurs as the SymbolSizeChooser control is being drawn.
        /// </summary>
        /// <param name="e"></param>
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

        #endregion
    }
}