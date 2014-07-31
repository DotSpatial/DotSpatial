// ********************************************************************************************************
// Product Name: DotSpatial.Layout.LayoutListBox
// Description:  Shows a list of all the items in the layout control
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll Version 6.0
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
using System.Linq;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is designed to automatically have add, subtract, up and down arrows for working with a simple collection of items.
    /// </summary>
    [ToolboxItem(true)]
    public class LayoutListBox : UserControl
    {
        #region ---------------- Class Variables

        //Internal Variables
        private Button _btnDown;
        private Panel _btnPanel;
        private Button _btnRemove;
        private Button _btnUp;
        private LayoutControl _layoutControl;
        private ListBox _lbxItems;
        private Panel _listPanel;
        private bool _suppressSelectionChange;

        #endregion

        #region ---------------- Properties

        /// <summary>
        /// Gets or sets the layoutControl
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set
            {
                if (value == null) return;
                _layoutControl = value;
                _layoutControl.SelectionChanged += _layoutControl_SelectionChanged;
                _layoutControl.ElementsChanged += _layoutControl_ElementsChanged;
                RefreshList();
            }
        }

        #endregion

        #region ---------------- Constructors

        /// <summary>
        /// Creates a new instance of the Collection Control
        /// </summary>
        public LayoutListBox()
        {
            InitializeComponent();
            _suppressSelectionChange = false;
            _lbxItems.DrawItem += lbxItems_DrawItem;
        }

        #endregion

        #region ---------------- Drawing Code

        private void lbxItems_DrawItem(object sender, DrawItemEventArgs e)
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

        #endregion

        #region ---------------- Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(LayoutListBox));
            this._lbxItems = new ListBox();
            this._btnPanel = new Panel();
            this._btnDown = new Button();
            this._btnUp = new Button();
            this._btnRemove = new Button();
            this._listPanel = new Panel();
            this._btnPanel.SuspendLayout();
            this._listPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // _lbxItems
            //
            resources.ApplyResources(this._lbxItems, "_lbxItems");
            this._lbxItems.DrawMode = DrawMode.OwnerDrawFixed;
            this._lbxItems.FormattingEnabled = true;
            this._lbxItems.Name = "_lbxItems";
            this._lbxItems.SelectionMode = SelectionMode.MultiExtended;
            this._lbxItems.SelectedIndexChanged += lbxItems_SelectedIndexChanged;
            //
            // _btnPanel
            //
            resources.ApplyResources(this._btnPanel, "_btnPanel");
            this._btnPanel.Controls.Add(this._btnDown);
            this._btnPanel.Controls.Add(this._btnUp);
            this._btnPanel.Controls.Add(this._btnRemove);
            this._btnPanel.Name = "_btnPanel";
            //
            // _btnDown
            //
            resources.ApplyResources(this._btnDown, "_btnDown");
            this._btnDown.Image = Images.down;
            this._btnDown.Name = "_btnDown";
            this._btnDown.UseVisualStyleBackColor = true;
            this._btnDown.Click += btnDown_Click;
            //
            // _btnUp
            //
            resources.ApplyResources(this._btnUp, "_btnUp");
            this._btnUp.Image = Images.up;
            this._btnUp.Name = "_btnUp";
            this._btnUp.UseVisualStyleBackColor = true;
            this._btnUp.Click += btnUp_Click;
            //
            // _btnRemove
            //
            resources.ApplyResources(this._btnRemove, "_btnRemove");
            this._btnRemove.Image = Images.mnuLayerClear;
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += btnRemove_Click;
            //
            // _listPanel
            //
            resources.ApplyResources(this._listPanel, "_listPanel");
            this._listPanel.BackColor = Color.White;
            this._listPanel.Controls.Add(this._lbxItems);
            this._listPanel.Name = "_listPanel";
            //
            // LayoutListBox
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._listPanel);
            this.Controls.Add(this._btnPanel);
            this.Name = "LayoutListBox";
            this._btnPanel.ResumeLayout(false);
            this._listPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        #region ---------------- Public Methods

        /// <summary>
        /// Refreshes the items in the list to accuratly reflect the current collection
        /// </summary>
        public void RefreshList()
        {
            _lbxItems.SuspendLayout();

            //We clear the old list
            _lbxItems.Items.Clear();

            //Updates the list of elements
            foreach (LayoutElement le in _layoutControl.LayoutElements.ToArray())
            {
                _lbxItems.Items.Add(le);
                le.ThumbnailChanged += le_ThumbnailChanged;
            }

            //Updates the selection list
            foreach (LayoutElement le in _layoutControl.SelectedLayoutElements.ToArray())
                _lbxItems.SelectedItems.Add(le);

            _lbxItems.ResumeLayout();
        }

        #endregion

        #region ---------------- Event Handlers

        private void le_ThumbnailChanged(object sender, EventArgs e)
        {
            _lbxItems.Invalidate();
        }

        private void _layoutControl_ElementsChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange) return;
            _suppressSelectionChange = true;
            RefreshList();
            _suppressSelectionChange = false;
        }

        private void _layoutControl_SelectionChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange) return;
            _suppressSelectionChange = true;
            RefreshList();
            _suppressSelectionChange = false;
        }

        private void lbxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange) return;
            _suppressSelectionChange = true;
            _layoutControl.SuspendLayout();
            _layoutControl.ClearSelection();
            _layoutControl.AddToSelection(new List<LayoutElement>(_lbxItems.SelectedItems.OfType<LayoutElement>()));
            _layoutControl.ResumeLayout();
            _suppressSelectionChange = false;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionDown();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionUp();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _layoutControl.DeleteSelected();
        }

        private void LayoutListBox_KeyUp(object sender, KeyEventArgs e)
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

        #endregion
    }
}