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
    internal class SymbolCollectionControl : UserControl
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

        private ScaleMode _scaleMode;
        private IList<ISymbol> _symbols;
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
        public SymbolCollectionControl()
        {
            InitializeComponent();
            _symbols = new List<ISymbol>();
            lbxItems.DrawItem += lbxItems_DrawItem;
        }

        private void lbxItems_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;
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
            ISymbol stroke = lbxItems.Items[e.Index] as ISymbol;
            if (stroke == null) return;
            Matrix old = e.Graphics.Transform;
            Matrix shift = e.Graphics.Transform;
            Size2D size = _symbols.GetBoundingSize();
            double scaleSize = 1;
            if (_scaleMode == ScaleMode.Geographic || size.Height > 14)
            {
                scaleSize = (ItemHeight - 6) / size.Height;
            }
            shift.Translate(e.Bounds.Left + e.Bounds.Width / 2, e.Bounds.Top + e.Bounds.Height / 2);
            //shift.Translate(-(float)(size.Width * scaleSize / 2), -(float)(size.Height * scaleSize / 2));
            e.Graphics.Transform = shift;
            stroke.Draw(e.Graphics, scaleSize);
            e.Graphics.Transform = old;
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolCollectionControl));
            this.lbxItems = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // lbxItems
            //
            resources.ApplyResources(this.lbxItems, "lbxItems");
            this.lbxItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbxItems.FormattingEnabled = true;
            this.lbxItems.Name = "lbxItems";
            this.lbxItems.SelectedIndexChanged += new System.EventHandler(this.lbxItems_SelectedIndexChanged);
            //
            // panel1
            //
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnAdd);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            //
            // btnDown
            //
            this.btnDown.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.down;
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Name = "btnDown";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            //
            // btnUp
            //
            this.btnUp.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.up;
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            //
            // btnRemove
            //
            this.btnRemove.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.mnuLayerClear;
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            //
            // btnAdd
            //
            this.btnAdd.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.mnuLayerAdd;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // SymbolCollectionControl
            //
            this.Controls.Add(this.lbxItems);
            this.Controls.Add(this.panel1);
            this.Name = "SymbolCollectionControl";
            resources.ApplyResources(this, "$this");
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
            foreach (ISymbol stroke in _symbols)
            {
                lbxItems.Items.Insert(0, stroke);
            }
            lbxItems.ResumeLayout();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the item height for this control.
        /// </summary>
        public int ItemHeight
        {
            get { return lbxItems.ItemHeight; }
            set { lbxItems.ItemHeight = value; }
        }

        /// <summary>
        /// Gets or sets the scale mode.  If the scale mode is set to geographic, then the
        /// specified size will be ignored when drawing the symbolic representation.
        /// </summary>
        public ScaleMode ScaleMode
        {
            get { return _scaleMode; }
            set { _scaleMode = value; }
        }

        /// <summary>
        /// Gets the selected item cast as an object.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISymbol SelectedSymbol
        {
            get
            {
                if (lbxItems == null) return null;
                if (lbxItems.SelectedItem == null) return null;
                return lbxItems.SelectedItem as ISymbol;
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
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<ISymbol> Symbols
        {
            get { return _symbols; }
            set
            {
                _symbols = value;
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
            ISymbol stroke = lbxItems.SelectedItem as ISymbol;
            if (stroke == null) return;
            _symbols.DecreaseIndex(stroke);
            RefreshList();
            lbxItems.SelectedItem = stroke;
            OnOrderChanged();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            ISymbol stroke = lbxItems.SelectedItem as ISymbol;
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

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            ISymbol stroke = lbxItems.SelectedItem as ISymbol;
            if (stroke == null) return;
            _symbols.IncreaseIndex(stroke);
            RefreshList();
            lbxItems.SelectedItem = stroke;
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