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
using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This is designed to automatically have add, subtract, up and down arrows for working with a simple collection of items.
    /// </summary>
    internal class CollectionControl : UserControl
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

        private INamedList _items;
        private Button btnAdd;
        private Button btnDown;
        private Button btnRemove;
        private Button btnUp;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private ListBox lbxItems;
        private Panel panel1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the Collection Control
        /// </summary>
        public CollectionControl()
        {
            InitializeComponent();
            _items = new NamedList<object>(); // by default this stores object, but the actual strong type can be changed by setting Items.
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionControl));
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
            // CollectionControl
            //
            this.Controls.Add(this.lbxItems);
            this.Controls.Add(this.panel1);
            this.Name = "CollectionControl";
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
            string[] names = _items.GetNames();
            foreach (string name in names)
            {
                lbxItems.Items.Insert(0, name);
            }
            lbxItems.ResumeLayout();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the currently selected name.
        /// </summary>
        public string SelectedName
        {
            get
            {
                if (lbxItems == null) return null;
                if (lbxItems.SelectedItem == null) return null;
                return lbxItems.SelectedItem.ToString();
            }
            set
            {
                if (lbxItems == null) return;
                lbxItems.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets the selected item cast as an object.
        /// </summary>
        public object SelectedObject
        {
            get
            {
                if (lbxItems == null) return null;
                if (lbxItems.SelectedItem == null) return null;
                if (_items == null) return null;
                return _items.GetItem(lbxItems.SelectedItem.ToString());
            }
            set
            {
                if (lbxItems == null) return;
                if (_items == null) return;
                string name = _items.GetNameOfObject(value);
                lbxItems.SelectedItem = name;
            }
        }

        /// <summary>
        /// This control works with the INamedList, which in turn can work with any strong typed
        /// IList.  The only button
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public INamedList ItemNames
        {
            get { return _items; }
            set
            {
                _items = value;
                RefreshList();
                if (lbxItems.Items.Count > 0)
                {
                    lbxItems.SelectedIndex = 0;
                }
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
            string name = lbxItems.SelectedItem.ToString();
            _items.Demote(name);
            RefreshList();
            lbxItems.SelectedItem = name;
            OnOrderChanged();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string name = lbxItems.SelectedItem.ToString();
            int index = lbxItems.SelectedIndex;
            _items.Remove(name);
            RefreshList();
            if (_items.Count == 0)
            {
                lbxItems.SelectedItem = null;
            }
            if (index >= _items.Count)
            {
                index -= 1;
            }
            lbxItems.SelectedIndex = index;
            OnRemoveClick();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            string name = lbxItems.SelectedItem.ToString();
            _items.Promote(name);
            RefreshList();
            lbxItems.SelectedItem = name;
            OnOrderChanged();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

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