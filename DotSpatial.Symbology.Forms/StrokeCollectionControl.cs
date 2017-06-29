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
    internal class StrokeCollectionControl : UserControl
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

        private Button _btnAdd;
        private Button _btnDown;
        private Button _btnRemove;
        private Button _btnUp;
        private ListBox _lbxItems;
        private Panel _panel1;

        private IList<IStroke> _strokes;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Collection Control
        /// </summary>
        public StrokeCollectionControl()
        {
            InitializeComponent();
            _strokes = new List<IStroke>();
            _lbxItems.DrawItem += LbxItemsDrawItem;
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
            IStroke stroke = _lbxItems.Items[e.Index] as IStroke;
            if (stroke == null) return;
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(new Point(e.Bounds.X + 10, e.Bounds.Y + e.Bounds.Height / 2), new Point(e.Bounds.Width - 10, e.Bounds.Y + e.Bounds.Height / 2));
            stroke.DrawPath(e.Graphics, gp, 1);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrokeCollectionControl));
            this._lbxItems = new System.Windows.Forms.ListBox();
            this._panel1 = new System.Windows.Forms.Panel();
            this._btnDown = new System.Windows.Forms.Button();
            this._btnUp = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnAdd = new System.Windows.Forms.Button();
            this._panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // _lbxItems
            //
            resources.ApplyResources(this._lbxItems, "_lbxItems");
            this._lbxItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._lbxItems.FormattingEnabled = true;
            this._lbxItems.Name = "_lbxItems";
            this._lbxItems.SelectedIndexChanged += new System.EventHandler(this.lbxItems_SelectedIndexChanged);
            //
            // _panel1
            //
            this._panel1.Controls.Add(this._btnDown);
            this._panel1.Controls.Add(this._btnUp);
            this._panel1.Controls.Add(this._btnRemove);
            this._panel1.Controls.Add(this._btnAdd);
            resources.ApplyResources(this._panel1, "_panel1");
            this._panel1.Name = "_panel1";
            //
            // _btnDown
            //
            this._btnDown.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.down;
            resources.ApplyResources(this._btnDown, "_btnDown");
            this._btnDown.Name = "_btnDown";
            this._btnDown.UseVisualStyleBackColor = true;
            this._btnDown.Click += new System.EventHandler(this.btnDown_Click);
            //
            // _btnUp
            //
            this._btnUp.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.up;
            resources.ApplyResources(this._btnUp, "_btnUp");
            this._btnUp.Name = "_btnUp";
            this._btnUp.UseVisualStyleBackColor = true;
            this._btnUp.Click += new System.EventHandler(this.btnUp_Click);
            //
            // _btnRemove
            //
            this._btnRemove.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.mnuLayerClear;
            resources.ApplyResources(this._btnRemove, "_btnRemove");
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            //
            // _btnAdd
            //
            this._btnAdd.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.mnuLayerAdd;
            resources.ApplyResources(this._btnAdd, "_btnAdd");
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            //
            // StrokeCollectionControl
            //
            this.Controls.Add(this._lbxItems);
            this.Controls.Add(this._panel1);
            this.Name = "StrokeCollectionControl";
            resources.ApplyResources(this, "$this");
            this._panel1.ResumeLayout(false);
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
            _lbxItems.SuspendLayout();
            _lbxItems.Items.Clear();
            foreach (IStroke stroke in _strokes)
            {
                _lbxItems.Items.Insert(0, stroke);
            }
            _lbxItems.ResumeLayout();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the selected item cast as an object.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IStroke SelectedStroke
        {
            get
            {
                if (_lbxItems == null) return null;
                if (_lbxItems.SelectedItem == null) return null;
                return _lbxItems.SelectedItem as IStroke;
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
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IList<IStroke> Strokes
        {
            get { return _strokes; }
            set
            {
                _strokes = value;
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
            if (_lbxItems.SelectedItem == null) return;
            IStroke stroke = _lbxItems.SelectedItem as IStroke;
            if (stroke == null) return;
            _strokes.DecreaseIndex(stroke);
            RefreshList();
            _lbxItems.SelectedItem = stroke;
            OnOrderChanged();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (_lbxItems.SelectedItem == null) return;
            IStroke stroke = _lbxItems.SelectedItem as IStroke;
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

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (_lbxItems.SelectedItem == null) return;
            IStroke stroke = _lbxItems.SelectedItem as IStroke;
            if (stroke == null) return;
            _strokes.IncreaseIndex(stroke);
            RefreshList();
            _lbxItems.SelectedItem = stroke;
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