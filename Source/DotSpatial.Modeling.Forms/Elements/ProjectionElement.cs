// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;
using DotSpatial.Modeling.Forms.Parameters;
using DotSpatial.Projections.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// ProjectionElement
    /// </summary>
    internal partial class ProjectionElement : DialogElement
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectionElement"/> class.
        /// </summary>
        /// <param name="value">The ProjectionParam.</param>
        public ProjectionElement(ProjectionParam value)
        {
            base.Param = value;
            InitializeComponent();
            DoRefresh();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents.
        /// </summary>
        public new ProjectionParam Param
        {
            get
            {
                return (ProjectionParam)base.Param;
            }

            set
            {
                base.Param = value;
                DoRefresh();
                Invalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the elements layout if the param's been changed.
        /// </summary>
        public override void Refresh()
        {
            DoRefresh();
        }

        /// <inheritdoc/>
        protected override void ParamValueChanged(Parameter sender)
        {
            DoRefresh();
        }

        /// <summary>
        /// Changes the projection of this param via the ProjectionSelectDialog.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void CmdSelectClick(object sender, EventArgs e)
        {
            using (ProjectionSelectDialog dlg = new ProjectionSelectDialog())
            {
                if (dlg.ShowDialog() != DialogResult.OK) return;
                if (Param == null)
                {
                    Param = new ProjectionParam("destination projection", dlg.SelectedCoordinateSystem);
                }
                else
                {
                    Param.Value = dlg.SelectedCoordinateSystem;
                }
            }
        }

        /// <summary>
        /// Updates the elements layout if the param's been changed.
        /// </summary>
        private void DoRefresh()
        {
            Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.ParameterInvalid;
            if (Param?.Value == null)
            {
                _lblProjection.Text = ModelingMessageStrings.ProjectionElement_PressButtonToSelectProjection;
                SetToolTipText(_lblProjection, string.Empty);
                return;
            }

            GroupBox.Text = Param.Name;
            _lblProjection.Text = Param.Value.ToProj4String();
            SetToolTipText(_lblProjection, _lblProjection.Text);
            Status = ToolStatus.Ok;
            LightTipText = ModelingMessageStrings.ParameterValid;
        }
        #endregion
    }
}