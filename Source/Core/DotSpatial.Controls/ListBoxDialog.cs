// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// ListBoxDialog.
    /// </summary>
    public partial class ListBoxDialog : Form
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ListBoxDialog"/> class.
        /// </summary>
        public ListBoxDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the currently selected item in this list dialog box.
        /// </summary>
        public object SelectedItem => lstItems.SelectedItem;

        #endregion

        #region Methods

        /// <summary>
        /// Adds the array of objects to the dialog box.
        /// </summary>
        /// <param name="items">The items to add.</param>
        public void Add(object[] items)
        {
            lstItems.Items.AddRange(items);
        }

        /// <summary>
        /// Clears the items from the dialog box.
        /// </summary>
        public void Clear()
        {
            lstItems.Items.Clear();
            lstItems.Items.Add("(None)");
        }

        #endregion
    }
}