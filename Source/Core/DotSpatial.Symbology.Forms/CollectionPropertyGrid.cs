// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// CollectionPropertyGrid.
    /// </summary>
    public partial class CollectionPropertyGrid : Form
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPropertyGrid"/> class.
        /// </summary>
        public CollectionPropertyGrid()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionPropertyGrid"/> class.
        /// </summary>
        /// <param name="list">The INamedList to display.</param>
        public CollectionPropertyGrid(INamedList list)
        {
            InitializeComponent();
            NamedList = list;
            ccItems.AddClicked += CcItemsAddClicked;
            ccItems.SelectedItemChanged += CcItemsSelectedItemChanged;
            CcItemsSelectedItemChanged(ccItems, EventArgs.Empty);
            ccItems.RemoveClicked += CcItemsRemoveClicked;
            Configure();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever the add item is clicked. This is because the Collection Property Grid
        /// doesn't necessarilly know how to create a default item. (An alternative would be
        /// to send in a factory, but I think this will work just as well.)
        /// </summary>
        public event EventHandler AddItemClicked;

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the tool that connects each item with a string name.
        /// </summary>
        public INamedList NamedList
        {
            get
            {
                return ccItems?.ItemNames;
            }

            set
            {
                ccItems.ItemNames = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Occurs when the add button is clicked.
        /// </summary>
        protected virtual void OnAddClicked()
        {
            AddItemClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the ChangesApplied event.
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            ChangesApplied?.Invoke(this, EventArgs.Empty);
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            OnApplyChanges();
            Close();
        }

        private void CcItemsAddClicked(object sender, EventArgs e)
        {
            OnAddClicked();
        }

        private void CcItemsRemoveClicked(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = ccItems.SelectedObject;
        }

        private void CcItemsSelectedItemChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = ccItems.SelectedObject;
        }

        private void Configure()
        {
            dialogButtons1.OkClicked += BtnOkClick;
            dialogButtons1.CancelClicked += BtnCancelClick;
            dialogButtons1.ApplyClicked += BtnApplyClick;
        }

        #endregion
    }
}