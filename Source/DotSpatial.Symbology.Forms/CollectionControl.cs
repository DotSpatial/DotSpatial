// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This is designed to automatically have add, subtract, up and down arrows for working with a simple collection of items.
    /// </summary>
    internal partial class CollectionControl : UserControl
    {
        #region Fields

        private INamedList _items;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionControl"/> class.
        /// </summary>
        public CollectionControl()
        {
            InitializeComponent();
            _items = new NamedList<object>(); // by default this stores object, but the actual strong type can be changed by setting Items.
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
        /// Gets or sets the INamedList. The INamedList can work with any strong typed IList.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public INamedList ItemNames
        {
            get
            {
                return _items;
            }

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

        /// <summary>
        /// Gets or sets the currently selected name.
        /// </summary>
        public string SelectedName
        {
            get
            {
                return lbxItems?.SelectedItem?.ToString();
            }

            set
            {
                if (lbxItems == null) return;
                lbxItems.SelectedItem = value;
            }
        }

        /// <summary>
        /// Gets or sets the selected item cast as an object.
        /// </summary>
        public object SelectedObject
        {
            get
            {
                if (lbxItems?.SelectedItem == null) return null;
                return _items?.GetItem(lbxItems.SelectedItem.ToString());
            }

            set
            {
                if (lbxItems == null || _items == null) return;
                string name = _items.GetNameOfObject(value);
                lbxItems.SelectedItem = name;
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
            string[] names = _items.GetNames();
            foreach (string name in names)
            {
                lbxItems.Items.Insert(0, name);
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
            RefreshList();
        }

        private void BtnDownClick(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            string name = lbxItems.SelectedItem.ToString();
            _items.Demote(name);
            RefreshList();
            lbxItems.SelectedItem = name;
            OnOrderChanged();
        }

        private void BtnRemoveClick(object sender, EventArgs e)
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

        private void BtnUpClick(object sender, EventArgs e)
        {
            if (lbxItems.SelectedItem == null) return;
            string name = lbxItems.SelectedItem.ToString();
            _items.Promote(name);
            RefreshList();
            lbxItems.SelectedItem = name;
            OnOrderChanged();
        }

        private void LbxItemsSelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedItemChanged();
        }

        #endregion
    }
}