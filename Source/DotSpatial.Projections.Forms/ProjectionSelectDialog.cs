// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;

namespace DotSpatial.Projections.Forms
{
    /// <summary>
    /// ProjectionSelectDialog
    /// </summary>
    public partial class ProjectionSelectDialog : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionSelectDialog"/> class.
        /// </summary>
        public ProjectionSelectDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the currently chosen coordinate system
        /// </summary>
        public ProjectionInfo SelectedCoordinateSystem
        {
            get
            {
                return projectionSelectControl1.SelectedCoordinateSystem;
            }

            set
            {
                projectionSelectControl1.SelectedCoordinateSystem = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the ChangesApplied event
        /// </summary>
        protected virtual void OnApplyChanges()
        {
            ChangesApplied?.Invoke(this, EventArgs.Empty);
        }

        private void BtnApplyClick(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        private void CmdOkClick(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        #endregion
    }
}