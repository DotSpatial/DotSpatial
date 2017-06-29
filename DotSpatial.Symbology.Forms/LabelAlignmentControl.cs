// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/22/2009 11:21:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// LabelAlignmentControl
    /// </summary>
    [DefaultEvent("ValueChanged"), DefaultProperty("Value"), ToolboxItem(false)]
    public class LabelAlignmentControl : UserControl
    {
        private Button btnDrop;
        private LabelAlignmentPicker labelAlignmentPicker1;
        private Label lblAlignmentText;

        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of LabelAlignmentControl
        /// </summary>
        public LabelAlignmentControl()
        {
            InitializeComponent();
            Height = 25;
            labelAlignmentPicker1.Visible = false;
            labelAlignmentPicker1.ValueChanged += labelAlignmentPicker1_ValueChanged;
        }

        private void labelAlignmentPicker1_ValueChanged(object sender, EventArgs e)
        {
            lblAlignmentText.Text = labelAlignmentPicker1.Value.ToString();
            Height = 25;
            labelAlignmentPicker1.Visible = false;
            OnValueChanged(sender, e);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Occurs when the value is changed and fires the ValueChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            if (ValueChanged != null) ValueChanged(sender, e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the actual value currently being represented in the control.
        /// </summary>
        [Category("Data"), Description("Gets or sets the actual value currently being represented in the control.")]
        public ContentAlignment Value
        {
            get
            {
                if (labelAlignmentPicker1 == null) return ContentAlignment.MiddleCenter;
                return labelAlignmentPicker1.Value;
            }
            set
            {
                labelAlignmentPicker1.Value = value;
                lblAlignmentText.Text = value.ToString();
            }
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelAlignmentControl));
            this.btnDrop = new System.Windows.Forms.Button();
            this.lblAlignmentText = new System.Windows.Forms.Label();
            this.labelAlignmentPicker1 = new DotSpatial.Symbology.Forms.LabelAlignmentPicker();
            this.SuspendLayout();
            //
            // btnDrop
            //
            resources.ApplyResources(this.btnDrop, "btnDrop");
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.UseVisualStyleBackColor = true;
            this.btnDrop.Click += new System.EventHandler(this.btnDrop_Click);
            //
            // lblAlignmentText
            //
            this.lblAlignmentText.BackColor = System.Drawing.Color.White;
            this.lblAlignmentText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblAlignmentText, "lblAlignmentText");
            this.lblAlignmentText.Name = "lblAlignmentText";
            //
            // labelAlignmentPicker1
            //
            resources.ApplyResources(this.labelAlignmentPicker1, "labelAlignmentPicker1");
            this.labelAlignmentPicker1.Name = "labelAlignmentPicker1";
            this.labelAlignmentPicker1.Value = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // LabelAlignmentControl
            //
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblAlignmentText);
            this.Controls.Add(this.labelAlignmentPicker1);
            this.Controls.Add(this.btnDrop);
            this.Name = "LabelAlignmentControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// Occurs after the drop-down has been used to select a value.
        /// </summary>
        public event EventHandler ValueChanged;

        private void btnDrop_Click(object sender, EventArgs e)
        {
            labelAlignmentPicker1.Visible = true;
            Height = 112;
        }
    }
}