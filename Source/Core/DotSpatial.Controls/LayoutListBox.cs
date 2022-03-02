// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is designed to automatically have add, subtract, up and down arrows for working with a simple collection of items.
    /// </summary>
    // This control will no longer be visible
    [ToolboxItem(false)]
    public partial class LayoutListBox : UserControl
    {
        #region Fields
        private bool _suppressSelectionChange;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutListBox"/> class.
        /// </summary>
        public LayoutListBox()
        {
            InitializeComponent();
            _suppressSelectionChange = false;
            _lbxItems.DrawItem += LbxItemsDrawItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the layoutControl.
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get
            {
                return _layoutControl;
            }

            set
            {
                if (value == null) return;
                _layoutControl = value;
                _layoutControl.SelectionChanged += LayoutControlSelectionChanged;
                _layoutControl.ElementsChanged += LayoutControlElementsChanged;
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

            // We clear the old list
            _lbxItems.Items.Clear();

            // Updates the list of elements
            foreach (LayoutElement le in _layoutControl.LayoutElements.ToArray())
            {
                _lbxItems.Items.Add(le);
                le.ThumbnailChanged += LeThumbnailChanged;
            }

            // Updates the selection list
            foreach (LayoutElement le in _layoutControl.SelectedLayoutElements.ToArray())
                _lbxItems.SelectedItems.Add(le);

            _lbxItems.ResumeLayout();
        }

        private void LayoutControlElementsChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange) return;
            _suppressSelectionChange = true;
            RefreshList();
            _suppressSelectionChange = false;
        }

        private void LayoutControlSelectionChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange) return;
            _suppressSelectionChange = true;
            RefreshList();
            _suppressSelectionChange = false;
        }

        private void BtnDownClick(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionDown();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            _layoutControl.DeleteSelected();
        }

        private void BtnUpClick(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionUp();
        }

        private void LayoutListBoxKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    _layoutControl.DeleteSelected();
                    break;
                case Keys.F5:
                    _layoutControl.RefreshElements();
                    break;
            }
        }

        private void LbxItemsDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;
            Rectangle outer = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            Brush textBrush;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, outer);
                textBrush = Brushes.White;
            }
            else
            {
                textBrush = Brushes.Black;
                e.Graphics.FillRectangle(Brushes.White, outer);
            }

            Rectangle thumbRect = new Rectangle(outer.X + 3, outer.Y + 3, 32, 32);
            e.Graphics.FillRectangle(Brushes.White, thumbRect);

            LayoutElement element = _lbxItems.Items[e.Index] as LayoutElement;
            if (element != null)
            {
                e.Graphics.DrawImage(element.ThumbNail, thumbRect);
            }

            thumbRect.X--;
            thumbRect.Y--;
            thumbRect.Width++;
            thumbRect.Height++;
            e.Graphics.DrawRectangle(Pens.Black, thumbRect);
            Rectangle textRectangle = new Rectangle(outer.X + 40, outer.Y, outer.Width - 40, outer.Height);
            using (StringFormat drawFormat = new StringFormat())
            {
                drawFormat.Alignment = StringAlignment.Near;
                drawFormat.FormatFlags = StringFormatFlags.NoWrap;
                drawFormat.LineAlignment = StringAlignment.Center;
                drawFormat.Trimming = StringTrimming.EllipsisCharacter;
                if (element != null)
                {
                    e.Graphics.DrawString(element.Name, Font, textBrush, textRectangle, drawFormat);
                }
            }
        }

        private void LbxItemsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange) return;
            _suppressSelectionChange = true;
            _layoutControl.SuspendLayout();
            _layoutControl.ClearSelection();
            _layoutControl.AddToSelection(new List<LayoutElement>(_lbxItems.SelectedItems.OfType<LayoutElement>()));
            _layoutControl.ResumeLayout();
            _suppressSelectionChange = false;
        }

        private void LeThumbnailChanged(object sender, EventArgs e)
        {
            _lbxItems.Invalidate();
        }

        #endregion
    }
}