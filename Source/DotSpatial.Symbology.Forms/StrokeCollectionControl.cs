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
    internal partial class StrokeCollectionControl : UserControl
    {
        #region Fields

        private IList<IStroke> _strokes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StrokeCollectionControl"/> class.
        /// </summary>
        public StrokeCollectionControl()
        {
            InitializeComponent();
            _strokes = new List<IStroke>();
            _lbxItems.DrawItem += LbxItemsDrawItem;
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
        /// Occurs when the remove button has been clicked.
        /// </summary>
        public event EventHandler RemoveClicked;

        /// <summary>
        /// Occurs when someone selects one of the items in the list box.
        /// </summary>
        public event EventHandler SelectedItemChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected item cast as an object.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IStroke SelectedStroke
        {
            get
            {
                return _lbxItems?.SelectedItem as IStroke;
            }

            set
            {
                if (_lbxItems == null) return;
                _lbxItems.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the core list of strokes that will be drawn here.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<IStroke> Strokes
        {
            get
            {
                return _strokes;
            }

            set
            {
                _strokes = value;
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
            _lbxItems.SuspendLayout();
            _lbxItems.Items.Clear();
            foreach (IStroke stroke in _strokes)
            {
                _lbxItems.Items.Insert(0, stroke);
            }

            _lbxItems.ResumeLayout();
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
            IStroke stroke = _lbxItems.SelectedItem as IStroke;
            if (stroke == null) return;
            _strokes.DecreaseIndex(stroke);
            RefreshList();
            _lbxItems.SelectedItem = stroke;
            OnOrderChanged();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            IStroke stroke = _lbxItems.SelectedItem as IStroke;
            if (stroke == null) return;
            int index = _strokes.IndexOf(stroke);
            _strokes.Remove(stroke);
            RefreshList();
            if (_strokes.Count == 0) return;
            if (index >= _strokes.Count)
            {
                index -= 1;
            }

            _lbxItems.SelectedIndex = index;
            OnRemoveClick();
        }

        private void BtnUpClick(object sender, EventArgs e)
        {
            IStroke stroke = _lbxItems.SelectedItem as IStroke;
            if (stroke == null) return;
            _strokes.IncreaseIndex(stroke);
            RefreshList();
            _lbxItems.SelectedItem = stroke;
            OnOrderChanged();
        }

        private void LbxItemsSelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        private void LbxItemsDrawItem(object sender, DrawItemEventArgs e)
        {
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
            IStroke stroke = _lbxItems.Items[e.Index] as IStroke;
            if (stroke == null) return;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddLine(new Point(e.Bounds.X + 10, e.Bounds.Y + (e.Bounds.Height / 2)), new Point(e.Bounds.Width - 10, e.Bounds.Y + (e.Bounds.Height / 2)));
                stroke.DrawPath(e.Graphics, gp, 1);
            }
        }

        #endregion
    }
}