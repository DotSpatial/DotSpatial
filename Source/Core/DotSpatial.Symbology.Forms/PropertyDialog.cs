// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A Dialog that can be useful for showing properties.
    /// </summary>
    public partial class PropertyDialog : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyDialog"/> class.
        /// </summary>
        public PropertyDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// This event occurs when someone presses the apply button.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets an original object place holder to allow outside handlers, but still track
        /// the object and its copy in a tightly correlated way.
        /// </summary>
        public IDescriptor OriginalObject { get; set; }

        /// <summary>
        /// Gets or sets the property grid on this dialog.
        /// </summary>
        public PropertyGrid PropertyGrid
        {
            get
            {
                return propertyGrid1;
            }

            set
            {
                propertyGrid1 = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the ChangesApplied event. If an original object IDescriptor has been set,
        /// then this directly handles the update.
        /// </summary>
        protected virtual void OnChangesApplied()
        {
            OriginalObject?.CopyProperties(PropertyGrid.SelectedObject);

            ChangesApplied?.Invoke(this, EventArgs.Empty);
        }

        private void CmdApplyClick(object sender, EventArgs e)
        {
            OnChangesApplied();
        }

        private void CmdCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CmdOkClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            OnChangesApplied();
            Close();
        }

        #endregion
    }
}