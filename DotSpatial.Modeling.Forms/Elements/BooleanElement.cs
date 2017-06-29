// ********************************************************************************************************
// Product Name: DotSpatial.Tools.BooleanElement
// Description:  Boolean Element for use in the tool dialog
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// An element for true/false values
    /// </summary>
    public class BooleanElement : DialogElement
    {
        #region Class Variables

        private bool _updateBox = true;
        private CheckBox checkBox1;

        #endregion

        #region Methods

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public BooleanElement(BooleanParam param)
        {
            //Needed by the designer
            InitializeComponent();

            Param = param;
            checkBox1.Text = param.CheckBoxText;
            GroupBox.Text = param.Name;

            DoRefresh();
        }

        private void DoRefresh()
        {
            _updateBox = false;

            //This stuff loads the default value
            if (Param.DefaultSpecified == false)
            {
                Status = ToolStatus.Empty;
                LightTipText = ModelingMessageStrings.ParameterInvalid;
                checkBox1.CheckState = CheckState.Indeterminate;
            }
            else if (Param.Value)
            {
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.ParameterValid;
                checkBox1.CheckState = CheckState.Checked;
            }
            else if (Param.Value == false)
            {
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.ParameterValid;
                checkBox1.CheckState = CheckState.Unchecked;
            }

            _updateBox = true;
        }

        /// <summary>
        /// Updates the status lights
        /// </summary>
        public override void Refresh()
        {
            DoRefresh();
        }

        #endregion

        #region Events

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the checkbox
        /// </summary>
        private void CheckBox1CheckStateChanged(object sender, EventArgs e)
        {
            if (!_updateBox) return;
            switch (checkBox1.CheckState)
            {
                case CheckState.Checked:
                    Param.Value = true;
                    break;
                case CheckState.Unchecked:
                    Param.Value = false;
                    break;
            }
        }

        /// <summary>
        /// When the check box it clicked this event fires
        /// </summary>
        private void CheckBox1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new BooleanParam Param
        {
            get { return (BooleanParam)base.Param; }
            set { base.Param = value; }
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.checkBox1 = new CheckBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBox1
            //
            this.GroupBox.Controls.Add(this.checkBox1);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.checkBox1, 0);
            //
            // lblStatus
            //
            this.StatusLabel.Location = new Point(12, 20);
            //
            // checkBox1
            //
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new Point(44, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(80, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckStateChanged += new EventHandler(this.CheckBox1CheckStateChanged);
            this.checkBox1.Click += new EventHandler(this.CheckBox1Click);
            //
            // BooleanElement
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "BooleanElement";
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}