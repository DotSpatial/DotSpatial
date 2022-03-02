// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
    internal partial class DecorationCollectionControl : UserControl
    {
        #region Fields
        private IList<ILineDecoration> _decorations;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DecorationCollectionControl"/> class.
        /// </summary>
        public DecorationCollectionControl()
        {
            InitializeComponent();
            _decorations = new List<ILineDecoration>();
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
        /// Gets or sets the core list of lineDecorations that will be drawn here.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<ILineDecoration> Decorations
        {
            get
            {
                return _decorations;
            }

            set
            {
                _decorations = value;
                RefreshList();
            }
        }

        /// <summary>
        /// Gets or sets the selected item cast as an object.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ILineDecoration SelectedDecoration
        {
            get
            {
                return lbxItems?.SelectedItem as ILineDecoration;
            }

            set
            {
                if (lbxItems == null) return;
                lbxItems.SelectedItem = value;
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
            foreach (ILineDecoration lineDecoration in _decorations)
            {
                lbxItems.Items.Insert(0, lineDecoration);
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
        }

        private void BtnDownClick(object sender, EventArgs e)
        {
            ILineDecoration lineDecoration = lbxItems.SelectedItem as ILineDecoration;
            if (lineDecoration == null) return;
            _decorations.DecreaseIndex(lineDecoration);
            RefreshList();
            lbxItems.SelectedItem = lineDecoration;
            OnOrderChanged();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            ILineDecoration lineDecoration = lbxItems.SelectedItem as ILineDecoration;
            int index = _decorations.IndexOf(lineDecoration);
            _decorations.Remove(lineDecoration);
            RefreshList();
            if (_decorations.Count == 0) return;
            if (index >= _decorations.Count)
            {
                index -= 1;
            }

            lbxItems.SelectedIndex = index;
            OnRemoveClick();
        }

        private void BtnUpClick(object sender, EventArgs e)
        {
            ILineDecoration lineDecoration = lbxItems.SelectedItem as ILineDecoration;
            if (lineDecoration == null) return;
            _decorations.IncreaseIndex(lineDecoration);
            RefreshList();
            lbxItems.SelectedItem = lineDecoration;
            OnOrderChanged();
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
                Brush b = new SolidBrush(BackColor);
                e.Graphics.FillRectangle(b, outer);
                b.Dispose();
            }

            Rectangle inner = new Rectangle(e.Bounds.X + 5, e.Bounds.Y + 1, e.Bounds.Width - 10, e.Bounds.Height - 3);
            e.Graphics.FillRectangle(Brushes.White, inner);
            e.Graphics.DrawRectangle(Pens.Black, inner);
            int index = (e.Index < 0) ? 0 : e.Index;
            if (lbxItems.Items.Count == 0) return;
            ILineDecoration lineDecoration = lbxItems.Items[index] as ILineDecoration;
            if (lineDecoration == null) return;
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(new Point(e.Bounds.X + 10, e.Bounds.Y + (e.Bounds.Height / 2)), new Point(e.Bounds.Width - 10, e.Bounds.Y + (e.Bounds.Height / 2)));
            lineDecoration.Draw(e.Graphics, gp, 1);
            gp.Dispose();
        }

        private void LbxItemsSelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        #endregion
    }
}