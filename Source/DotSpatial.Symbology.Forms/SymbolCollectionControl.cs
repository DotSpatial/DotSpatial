// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 4:23:05 PM
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
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This is designed to automatically have add, subtract, up and down arrows for working with a simple collection of items.
    /// </summary>
    internal partial class SymbolCollectionControl : UserControl
    {
        #region Fields

        private IList<ISymbol> _symbols;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolCollectionControl"/> class.
        /// </summary>
        public SymbolCollectionControl()
        {
            InitializeComponent();
            _symbols = new List<ISymbol>();
            lbxItems.DrawItem += LbxItemsDrawItem;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when someone clicks the add button.
        /// </summary>
        public event EventHandler AddClicked;

        /// <summary>
        /// Occurs when the list has been added, removed, or re-ordered in any way.
        /// </summary>
        public event EventHandler ListChanged;

        /// <summary>
        /// Occurs when either the Promote or Demote function has been used,
        /// changing the order.
        /// </summary>
        public event EventHandler OrderChanged;

        /// <summary>
        /// Occurs when the remove button has been clicked
        /// </summary>
        public event EventHandler RemoveClicked;

        /// <summary>
        /// Occurs when someone selects one of the items in the list box
        /// </summary>
        public event EventHandler SelectedItemChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the item height for this control.
        /// </summary>
        public int ItemHeight
        {
            get
            {
                return lbxItems.ItemHeight;
            }

            set
            {
                lbxItems.ItemHeight = value;
            }
        }

        /// <summary>
        /// Gets or sets the scale mode. If the scale mode is set to geographic, then the
        /// specified size will be ignored when drawing the symbolic representation.
        /// </summary>
        public ScaleMode ScaleMode { get; set; }

        /// <summary>
        /// Gets or sets the selected item cast as an object.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISymbol SelectedSymbol
        {
            get
            {
                return lbxItems?.SelectedItem as ISymbol;
            }

            set
            {
                if (lbxItems == null) return;
                lbxItems.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the core list of strokes that will be drawn here.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<ISymbol> Symbols
        {
            get
            {
                return _symbols;
            }

            set
            {
                _symbols = value;
                RefreshList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the items in the list to accurately reflect the current collection.
        /// </summary>
        public void RefreshList()
        {
            lbxItems.SuspendLayout();
            lbxItems.Items.Clear();
            foreach (ISymbol stroke in _symbols)
            {
                lbxItems.Items.Insert(0, stroke);
            }

            lbxItems.ResumeLayout();
        }

        /// <summary>
        /// Fires the AddClicked event.
        /// </summary>
        protected virtual void OnAdd()
        {
            AddClicked?.Invoke(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the ListChanged event.
        /// </summary>
        protected virtual void OnListChanged()
        {
            ListChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the OnOrderChanged event.
        /// </summary>
        protected virtual void OnOrderChanged()
        {
            OrderChanged?.Invoke(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the RemoveCLicked event.
        /// </summary>
        protected virtual void OnRemoveClick()
        {
            RemoveClicked?.Invoke(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the SelectedItemChanged event.
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
            SelectedItemChanged?.Invoke(this, EventArgs.Empty);
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            OnAdd();
            RefreshList();
        }

        private void BtnDownClick(object sender, EventArgs e)
        {
            ISymbol stroke = lbxItems.SelectedItem as ISymbol;
            if (stroke == null) return;
            _symbols.DecreaseIndex(stroke);
            RefreshList();
            lbxItems.SelectedItem = stroke;
            OnOrderChanged();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            ISymbol stroke = lbxItems.SelectedItem as ISymbol;
            if (stroke == null) return;
            int index = _symbols.IndexOf(stroke);
            _symbols.Remove(stroke);
            RefreshList();
            if (_symbols.Count == 0) return;
            if (index >= _symbols.Count)
            {
                index -= 1;
            }

            lbxItems.SelectedIndex = index;
            OnRemoveClick();
        }

        private void BtnUpClick(object sender, EventArgs e)
        {
            ISymbol stroke = lbxItems.SelectedItem as ISymbol;
            if (stroke == null) return;
            _symbols.IncreaseIndex(stroke);
            RefreshList();
            lbxItems.SelectedItem = stroke;
            OnOrderChanged();
        }

        private void LbxItemsDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;
            Rectangle outer = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, outer);
            }
            else
            {
                using (Brush b = new SolidBrush(BackColor))
                {
                    e.Graphics.FillRectangle(b, outer);
                }
            }

            Rectangle inner = new Rectangle(e.Bounds.X + 5, e.Bounds.Y + 1, e.Bounds.Width - 10, e.Bounds.Height - 3);
            e.Graphics.FillRectangle(Brushes.White, inner);
            e.Graphics.DrawRectangle(Pens.Black, inner);
            ISymbol stroke = lbxItems.Items[e.Index] as ISymbol;
            if (stroke == null) return;
            Matrix old = e.Graphics.Transform;
            Matrix shift = e.Graphics.Transform;
            Size2D size = _symbols.GetBoundingSize();
            double scaleSize = 1;
            if (ScaleMode == ScaleMode.Geographic || size.Height > 14)
            {
                scaleSize = (ItemHeight - 6) / size.Height;
            }

            shift.Translate(e.Bounds.Left + (e.Bounds.Width / 2), e.Bounds.Top + (e.Bounds.Height / 2));
            e.Graphics.Transform = shift;
            stroke.Draw(e.Graphics, scaleSize);
            e.Graphics.Transform = old;
        }

        private void LbxItemsSelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        #endregion
    }
}