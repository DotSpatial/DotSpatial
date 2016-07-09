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
using DotSpatial.Modeling.Forms.Parameters;
using DotSpatial.Projections.Forms;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// ProjectionElement
    /// </summary>
    internal class ProjectionElement : DialogElement
    {
        #region Private Variables

        private Button _cmdSelect;
        private Label _lblProjection;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProjectionElement
        /// </summary>
        public ProjectionElement(ProjectionParam value)
        {
            base.Param = value;
            InitializeComponent();
            DoRefresh();
        }

        /// <inheritdoc/>
        protected override void ParamValueChanged(Parameter sender)
        {
            DoRefresh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the elements layout if the param's been changed.
        /// </summary>
        private void DoRefresh()
        {
            Status = ToolStatus.Empty;
            LightTipText = ModelingMessageStrings.ParameterInvalid;
            if (Param == null || Param.Value == null)
            {
                _lblProjection.Text = ModelingMessageStrings.ProjectionElement_PressButtonToSelectProjection;
                SetToolTipText(_lblProjection, "");
                return;
            }
            GroupBox.Text = Param.Name;
            _lblProjection.Text = Param.Value.ToProj4String();
            SetToolTipText(_lblProjection, _lblProjection.Text);
            Status = ToolStatus.Ok;
            LightTipText = ModelingMessageStrings.ParameterValid;
        }

        /// <summary>
        /// Updates the elements layout if the param's been changed.
        /// </summary>
        public override void Refresh()
        {
            DoRefresh();
        }

        /// <summary>
        /// Changes the projection of this param via the ProjectionSelectDialog.
        /// </summary>
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

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents.
        /// </summary>
        public new ProjectionParam Param
        {
            get { return (ProjectionParam)base.Param; }
            set
            {
                base.Param = value;
                DoRefresh();
                Invalidate();
            }
        }

        #endregion

        #region Generate by the designer

        private void InitializeComponent()
        {
            _lblProjection = new Label();
            _cmdSelect = new Button();
            SuspendLayout();
            //
            // groupBox1
            //
            GroupBox.Controls.Add(_lblProjection);
            GroupBox.Controls.Add(_cmdSelect);
            GroupBox.Controls.SetChildIndex(_cmdSelect, 0);
            GroupBox.Controls.SetChildIndex(_lblProjection, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);
            //
            // lblProjection
            //
            _lblProjection.Anchor = (AnchorStyles.Top | AnchorStyles.Left) | AnchorStyles.Right;
            _lblProjection.BackColor = Color.White;
            _lblProjection.BorderStyle = BorderStyle.Fixed3D;
            _lblProjection.Location = new Point(39, 16);
            _lblProjection.Name = "_lblProjection";
            _lblProjection.Size = new Size(405, 20);
            _lblProjection.TabIndex = 2;
            _lblProjection.Text = ModelingMessageStrings.ProjectionElement_PressButtonToSelectProjection;
            //
            // cmdSelect
            //
            _cmdSelect.Location = new Point(450, 15);
            _cmdSelect.Name = "_cmdSelect";
            _cmdSelect.Size = new Size(36, 23);
            _cmdSelect.TabIndex = 3;
            _cmdSelect.Text = ModelingMessageStrings.SelectButtonText;
            _cmdSelect.UseVisualStyleBackColor = true;
            _cmdSelect.Click += CmdSelectClick;
            //
            // ProjectionElement
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "ProjectionElement";
            ResumeLayout(false);
        }

        #endregion
    }
}