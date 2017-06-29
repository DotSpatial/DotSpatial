// ********************************************************************************************************
// Product Name: DotSpatial.Tools.DialogSpacerElement
// Description:  DialogSpacerElement Element for use in the tool dialog
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
    /// An element for DialogSpacing
    /// </summary>
    public class DialogSpacerElement : DialogElement
    {
        private Label _label1;

        /// <summary>
        /// Creates an instance of a dialogspacerelement
        /// </summary>
        public DialogSpacerElement()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the text of the spacer
        /// </summary>
        public override string Text
        {
            get { return _label1.Text; }
            set { _label1.Text = value; }
        }

        /// <summary>
        /// Gets the current status the input
        /// </summary>
        public override ToolStatus Status
        {
            get { return ToolStatus.Ok; }
            set { }
        }

        private void Label1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this._label1 = new Label();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBox1
            //
            this.GroupBox.Size = new Size(492, 33);
            this.GroupBox.Visible = false;
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            //
            // lblStatus
            //
            this.StatusLabel.Visible = false;
            //
            // label1
            //
            this.Controls.Add(this._label1);
            this.Controls.SetChildIndex(this._label1, 0);
            this._label1.AutoSize = true;
            this._label1.Font = new Font("Microsoft Sans Serif", 9.75F, ((FontStyle)((FontStyle.Bold | FontStyle.Underline))), GraphicsUnit.Point, ((byte)(0)));
            this._label1.Location = new Point(8, 12);
            this._label1.Name = "_label1";
            this._label1.Size = new Size(67, 16);
            this._label1.TabIndex = 3;
            this._label1.Text = "asdfsdfs";
            this._label1.Click += new EventHandler(this.Label1Click);
            //
            // DialogSpacerElement
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "DialogSpacerElement";
            this.Size = new Size(492, 33);
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}