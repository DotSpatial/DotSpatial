// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/30/2009 8:55:03 AM
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
    /// CollectionPropertyGrid
    /// </summary>
    public class CollectionPropertyGrid : Form
    {
        #region Events

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler ChangesApplied;

        /// <summary>
        /// Occurs whenever the add item is clicked.  This is because the Collection Property Grid
        /// doesn't necessarilly know how to create a default item.  (An alternative would be
        /// to send in a factory, but I think this will work just as well.)
        /// </summary>
        public event EventHandler AddItemClicked;

        #endregion

        private CollectionControl ccItems;
        private DialogButtons dialogButtons1;
        private Panel panel1;
        private PropertyGrid propertyGrid1;
        private SplitContainer splitContainer1;

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionPropertyGrid));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ccItems = new DotSpatial.Symbology.Forms.CollectionControl();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // splitContainer1
            //
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.ccItems);
            //
            // splitContainer1.Panel2
            //
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            //
            // ccItems
            //
            resources.ApplyResources(this.ccItems, "ccItems");
            this.ccItems.Name = "ccItems";
            this.ccItems.SelectedName = null;
            this.ccItems.SelectedObject = null;
            //
            // propertyGrid1
            //
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.Name = "propertyGrid1";
            //
            // panel1
            //
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.dialogButtons1);
            this.panel1.Name = "panel1";
            //
            // dialogButtons1
            //
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.Name = "dialogButtons1";
            //
            // CollectionPropertyGrid
            //
            resources.ApplyResources(this, "$this");
            this.AllowDrop = true;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "CollectionPropertyGrid";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CollectionPropertyGrid
        /// </summary>
        public CollectionPropertyGrid()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Creates a new instance of CollectionPropertyGrid
        /// </summary>
        /// <param name="list">The INamedList to display</param>
        public CollectionPropertyGrid(INamedList list)
        {
            InitializeComponent();
            NamedList = list;
            ccItems.AddClicked += ccItems_AddClicked;
            ccItems.SelectedItemChanged += ccItems_SelectedItemChanged;
            ccItems_SelectedItemChanged(ccItems, EventArgs.Empty);
            ccItems.RemoveClicked += ccItems_RemoveClicked;
            Configure();
        }

        private void Configure()
        {
            dialogButtons1.OkClicked += btnOk_Click;
            dialogButtons1.CancelClicked += btnCancel_Click;
            dialogButtons1.ApplyClicked += btnApply_Click;
        }

        private void ccItems_RemoveClicked(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = ccItems.SelectedObject;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tool that connects each item with a string name.
        /// </summary>
        public INamedList NamedList
        {
            get
            {
                if (ccItems != null) return ccItems.ItemNames;
                return null;
            }
            set
            {
                ccItems.ItemNames = value;
            }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        private void ccItems_AddClicked(object sender, EventArgs e)
        {
            OnAddClicked();
        }

        private void ccItems_SelectedItemChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = ccItems.SelectedObject;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
            Close();
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
        /// Occurs when the add button is clicked
        /// </summary>
        protected virtual void OnAddClicked()
        {
            if (AddItemClicked != null) AddItemClicked(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            if (ChangesApplied != null) ChangesApplied(this, EventArgs.Empty);
        }

        #endregion
    }
}