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
    [ToolboxBitmap(typeof(PatternCollectionControl), "UserControl.ico")]
    internal class PatternCollectionControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when someone clicks the add button.
        /// </summary>
        public event EventHandler AddClicked;

        /// <summary>
        /// Occurs when someone selects one of the items in the list box
        /// </summary>
        public event EventHandler SelectedItemChanged;

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
        /// Occurs when the list has been added, removed, or re-ordered in any way.
        /// </summary>
        public event EventHandler ListChanged;

        #endregion

        #region Private Variables

        private IList<IPattern> _patterns;
        private Button btnAdd;
        private Button btnDown;
        private Button btnRemove;
        private Button btnUp;
        private ListBox lbxItems;
        private Panel panel1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Collection Control
        /// </summary>
        public PatternCollectionControl()
        {
            InitializeComponent();
            _patterns = new List<IPattern>();
            lbxItems.DrawItem += lbxItems_DrawItem;
        }

        private void lbxItems_DrawItem(object sender, DrawItemEventArgs e)
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
            e.Graphics.Clip = new Region(inner);
            IPattern pattern = lbxItems.Items[e.Index] as IPattern;
            if (pattern == null) return;
            GraphicsPath gp = new GraphicsPath();
            gp.AddRectangle(new Rectangle(new Point(e.Bounds.X + 10, e.Bounds.Y + 2), new Size(e.Bounds.Width - 20, e.Bounds.Height - 4)));
            pattern.FillPath(e.Graphics, gp);
            pattern.DrawPath(e.Graphics, gp, 1);
            gp.Dispose();
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PatternCollectionControl));
            this.lbxItems = new ListBox();
            this.panel1 = new Panel();
            this.btnDown = new Button();
            this.btnUp = new Button();
            this.btnRemove = new Button();
            this.btnAdd = new Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // lbxItems
            //
            this.lbxItems.AccessibleDescription = null;
            this.lbxItems.AccessibleName = null;
            resources.ApplyResources(this.lbxItems, "lbxItems");
            this.lbxItems.BackgroundImage = null;
            this.lbxItems.DrawMode = DrawMode.OwnerDrawFixed;
            this.lbxItems.Font = null;
            this.lbxItems.FormattingEnabled = true;
            this.lbxItems.Name = "lbxItems";
            this.lbxItems.SelectedIndexChanged += new EventHandler(this.lbxItems_SelectedIndexChanged);
            //
            // panel1
            //
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            //
            // btnDown
            //
            this.btnDown.AccessibleDescription = null;
            this.btnDown.AccessibleName = null;
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.BackgroundImage = null;
            this.btnDown.Font = null;
            this.btnDown.Image = SymbologyFormsImages.down as Image;
            this.btnDown.Name = "btnDown";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new EventHandler(this.btnDown_Click);
            //
            // btnUp
            //
            this.btnUp.AccessibleDescription = null;
            this.btnUp.AccessibleName = null;
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.BackgroundImage = null;
            this.btnUp.Font = null;
            this.btnUp.Image = SymbologyFormsImages.up as Image;
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new EventHandler(this.btnUp_Click);
            //
            // btnRemove
            //
            this.btnRemove.AccessibleDescription = null;
            this.btnRemove.AccessibleName = null;
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.BackgroundImage = null;
            this.btnRemove.Font = null;
            this.btnRemove.Image = SymbologyFormsImages.mnuLayerClear as Image;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new EventHandler(this.btnRemove_Click);
            //
            // btnAdd
            //
            this.btnAdd.AccessibleDescription = null;
            this.btnAdd.AccessibleName = null;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.BackgroundImage = null;
            this.btnAdd.Font = null;
            this.btnAdd.Image = SymbologyFormsImages.mnuLayerAdd as Image;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.btnAdd_Click);
            //
            // PatternCollectionControl
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackgroundImage = null;
            this.Controls.Add(this.lbxItems);
            this.Controls.Add(this.panel1);
            this.Font = null;
            this.Name = "PatternCollectionControl";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void lbxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        /// <summary>
        /// Fires the SelectedItemChanged event
        /// </summary>
        protected virtual void OnSelectedItemChanged()
        {
            if (SelectedItemChanged != null) SelectedItemChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the items in the list to accuratly reflect the current collection
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets the selected item cast as an object.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IPattern SelectedPattern
        {
            get
            {
                if (lbxItems == null) return null;
                if (lbxItems.SelectedItem == null) return null;
                return lbxItems.SelectedItem as IPattern;
            }
            set
            {
                if (lbxItems == null) return;
                lbxItems.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the core list of patterns that will be drawn here.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<IPattern> Patterns
        {
            get { return _patterns; }
            set
            {
                _patterns = value;
                RefreshList();
            }
        }

        #endregion

        #region Event Handlers

        private void btnAdd_Click(object sender, EventArgs e)
        {
            OnAdd();
            RefreshList();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            IPattern pattern = lbxItems.SelectedItem as IPattern;
            if (pattern == null) return;
            _patterns.DecreaseIndex(pattern);
            RefreshList();
            lbxItems.SelectedItem = pattern;
            OnOrderChanged();
        }

        private void btnRemove_Click(object sender, EventArgs e)
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

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            IPattern pattern = lbxItems.SelectedItem as IPattern;
            if (pattern == null) return;
            _patterns.IncreaseIndex(pattern);
            RefreshList();
            lbxItems.SelectedItem = pattern;
            OnOrderChanged();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the AddClicked event
        /// </summary>
        protected virtual void OnAdd()
        {
            if (AddClicked != null) AddClicked(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the ListChanged event
        /// </summary>
        protected virtual void OnListChanged()
        {
            if (ListChanged != null) ListChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the RemoveCLicked event
        /// </summary>
        protected virtual void OnRemoveClick()
        {
            if (RemoveClicked != null) RemoveClicked(this, EventArgs.Empty);
            OnListChanged();
        }

        /// <summary>
        /// Fires the OnOrderChanged event
        /// </summary>
        protected virtual void OnOrderChanged()
        {
            if (OrderChanged != null) OrderChanged(this, EventArgs.Empty);
            OnListChanged();
        }

        #endregion
    }
}