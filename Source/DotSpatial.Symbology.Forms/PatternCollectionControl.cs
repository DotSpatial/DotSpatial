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
    [ToolboxBitmap(typeof(PatternCollectionControl), "UserControl.ico")]
    internal partial class PatternCollectionControl : UserControl
    {
        #region Fields

        private IList<IPattern> _patterns;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PatternCollectionControl"/> class.
        /// </summary>
        public PatternCollectionControl()
        {
            InitializeComponent();
            _patterns = new List<IPattern>();
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
        /// Gets or sets the core list of patterns that will be drawn here.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<IPattern> Patterns
        {
            get
            {
                return _patterns;
            }

            set
            {
                _patterns = value;
                RefreshList();
            }
        }

        /// <summary>
        /// Gets or sets the selected item cast as an object.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IPattern SelectedPattern
        {
            get
            {
                return lbxItems?.SelectedItem as IPattern;
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
        /// Refreshes the items in the list to accurately reflect the current collection
        /// </summary>
        public void RefreshList()
        {
            lbxItems.SuspendLayout();
            lbxItems.Items.Clear();
            foreach (IPattern pattern in _patterns)
            {
                lbxItems.Items.Insert(0, pattern);
            }

            lbxItems.ResumeLayout();
        }

        /// <summary>
        /// Fires the AddClicked event
        /// </summary>
        protected virtual void OnAdd()
        {
            AddClicked?.Invoke(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the ListChanged event
        /// </summary>
        protected virtual void OnListChanged()
        {
            ListChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the OnOrderChanged event
        /// </summary>
        protected virtual void OnOrderChanged()
        {
            OrderChanged?.Invoke(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the RemoveCLicked event
        /// </summary>
        protected virtual void OnRemoveClick()
        {
            RemoveClicked?.Invoke(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the SelectedItemChanged event
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
            IPattern pattern = lbxItems.SelectedItem as IPattern;
            if (pattern == null) return;
            _patterns.DecreaseIndex(pattern);
            RefreshList();
            lbxItems.SelectedItem = pattern;
            OnOrderChanged();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            if (lbxItems.Items.Count <= 1) return;
            IPattern pattern = lbxItems.SelectedItem as IPattern;
            int index = _patterns.IndexOf(pattern);
            _patterns.Remove(pattern);
            RefreshList();
            if (_patterns.Count == 0) return;
            if (index >= _patterns.Count)
            {
                index -= 1;
            }

            lbxItems.SelectedIndex = index;
            OnRemoveClick();
        }

        private void BtnUpClick(object sender, EventArgs e)
        {
            IPattern pattern = lbxItems.SelectedItem as IPattern;
            if (pattern == null) return;
            _patterns.IncreaseIndex(pattern);
            RefreshList();
            lbxItems.SelectedItem = pattern;
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
                using (Brush b = new SolidBrush(BackColor))
                {
                    e.Graphics.FillRectangle(b, outer);
                }
            }

            Rectangle inner = new Rectangle(e.Bounds.X + 5, e.Bounds.Y + 1, e.Bounds.Width - 10, e.Bounds.Height - 3);
            e.Graphics.FillRectangle(Brushes.White, inner);
            e.Graphics.DrawRectangle(Pens.Black, inner);
            e.Graphics.Clip = new Region(inner);
            IPattern pattern = lbxItems.Items[e.Index] as IPattern;
            if (pattern == null) return;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRectangle(new Rectangle(new Point(e.Bounds.X + 10, e.Bounds.Y + 2), new Size(e.Bounds.Width - 20, e.Bounds.Height - 4)));
                pattern.FillPath(e.Graphics, gp);
                pattern.DrawPath(e.Graphics, gp, 1);
            }
        }

        private void LbxItemsSelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        #endregion
    }
}