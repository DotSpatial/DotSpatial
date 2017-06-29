// ********************************************************************************************************
// Product Name: DotSpatial.dll Alpha
// Description:  A library module for the DotSpatial geospatial framework for .Net.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/25/2009 1:41:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Projections.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// ProjectionElement
    /// </summary>
    internal class ProjectionElement : DialogElement
    {
        #region Private Variables

        private ToolTip _tthelp;
        private Button cmdSelect;
        private Label lblProjection;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProjectionElement
        /// </summary>
        public ProjectionElement(ProjectionParam value)
        {
            _tthelp = new ToolTip();
            base.Param = value;
            InitializeComponent();
            if (value == null) return;
            GroupBox.Text = value.Name;
            if (value.Value != null)
            {
                lblProjection.Text = value.Value.ToProj4String();
            }
            value.ValueChanged += ParamValueChanged;
        }

        /// <inheritdoc/>
        protected override void ParamValueChanged(Parameter sender)
        {
            if (Param == null) return;
            if (Param.Value == null)
            {
                lblProjection.Text = string.Empty;
                return;
            }
            lblProjection.Text = Param.Value.ToProj4String();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new ProjectionParam Param
        {
            get { return (ProjectionParam)base.Param; }
            set
            {
                Status = ToolStatus.Empty;
                LightTipText = ModelingMessageStrings.ParameterInvalid;
                base.Param = value;
                if (value == null) return;
                if (value.Value == null) return;
                lblProjection.Text = value.Value.ToProj4String();
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.ParameterValid;
                Invalidate();
            }
        }

        #endregion

        private void InitializeComponent()
        {
            lblProjection = new Label();
            cmdSelect = new Button();
            SuspendLayout();
            //
            // groupBox1
            //
            GroupBox.Controls.Add(lblProjection);
            GroupBox.Controls.Add(cmdSelect);
            GroupBox.Controls.SetChildIndex(cmdSelect, 0);
            GroupBox.Controls.SetChildIndex(lblProjection, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);
            //
            // lblProjection
            //
            lblProjection.Anchor = (AnchorStyles.Top | AnchorStyles.Left)
                                   | AnchorStyles.Right;
            lblProjection.BackColor = Color.White;
            lblProjection.BorderStyle = BorderStyle.Fixed3D;
            lblProjection.Location = new Point(39, 16);
            lblProjection.Name = "lblProjection";
            lblProjection.Size = new Size(405, 20);
            lblProjection.TabIndex = 2;
            lblProjection.Text = "Press the button to select a projection";
            //
            // cmdSelect
            //
            cmdSelect.Location = new Point(450, 15);
            cmdSelect.Name = "cmdSelect";
            cmdSelect.Size = new Size(36, 23);
            cmdSelect.TabIndex = 3;
            cmdSelect.Text = "...";
            cmdSelect.UseVisualStyleBackColor = true;
            cmdSelect.Click += CmdSelectClick;
            //
            // ProjectionElement
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "ProjectionElement";
            ResumeLayout(false);
        }

        private void CmdSelectClick(object sender, EventArgs e)
        {
            ProjectionSelectDialog dlg = new ProjectionSelectDialog();
            if (dlg.ShowDialog() != DialogResult.OK) return;
            if (Param == null)
            {
                Param = new ProjectionParam("destination projection", dlg.SelectedCoordinateSystem);
            }
            else
            {
                Param.Value = dlg.SelectedCoordinateSystem;
            }
            lblProjection.Text = Param.Value.ToProj4String();
            _tthelp.SetToolTip(lblProjection, Param.Value.ToProj4String());
        }

        public override void Refresh()
        {
            Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.ParameterInvalid;
            if (Param == null) return;
            if (Param.Value == null) return;
            Status = ToolStatus.Ok;
            LightTipText = ModelingMessageStrings.ParameterValid;
        }
    }
}