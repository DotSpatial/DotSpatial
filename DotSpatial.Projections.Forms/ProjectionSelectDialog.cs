// ********************************************************************************************************
// Product Name: DotSpatial.Projections.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/18/2009 3:45:39 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Projections.Forms
{
    /// <summary>
    /// ProjectionSelectDialog
    /// </summary>
    public partial class ProjectionSelectDialog : Form
    {
        #region Events

        /// <summary>
        /// Occurs whenever the apply changes button is clicked, or else when the ok button is clicked.
        /// </summary>
        public event EventHandler ChangesApplied;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="ProjectionSelectDialog"/>
        /// </summary>
        public ProjectionSelectDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the currently chosen coordinate system
        /// </summary>
        public ProjectionInfo SelectedCoordinateSystem
        {
            get { return projectionSelectControl1.SelectedCoordinateSystem; }
            set { projectionSelectControl1.SelectedCoordinateSystem = value; }
        }

        #endregion
        
        #region Event Handlers

        private void btnApply_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        private void cmdOk_Click(object sender, EventArgs e)
        {
            OnApplyChanges();
        }

        #endregion

        #region Protected Methods
       
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