// ********************************************************************************************************
// Product Name: DotSpatial.Tools.StringElement
// Description:  String Element for use in the tool dialog
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    internal class StringElement : DialogElement
    {
        #region Class Variables

        private TextBox textBox1;

        #endregion

        #region Methods

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public StringElement(StringParam param)
        {
            //Needed by the designer
            InitializeComponent();
            GroupBox.Text = param.Name;

            //We save the parameters passed in
            Param = param;
            SetupDefaultLighting();
        }

        public override void Refresh()
        {
            SetupDefaultLighting();
        }

        private void SetupDefaultLighting()
        {
            //We load the default parameters
            if (Param.DefaultSpecified)
            {
                textBox1.Text = Param.Value;
                Status = ToolStatus.Ok;
                LightTipText = ModelingMessageStrings.ParameterValid;
            }
            else
            {
                Status = ToolStatus.Empty;
                LightTipText = ModelingMessageStrings.ParameterInvalid;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box
        /// </summary>
        private void TextBox1TextChanged(object sender, EventArgs e)
        {
            Param.Value = textBox1.Text;
        }

        /// <summary>
        /// When the text box is clicked this event fires
        /// </summary>
        private void TextBox1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new StringParam Param
        {
            get { return (StringParam)base.Param; }
            set { base.Param = value; }
        }

        #endregion

        #region Generate by the designer

        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            GroupBox.SuspendLayout();
            SuspendLayout();
            //
            // groupBox1
            //
            GroupBox.Controls.Add(textBox1);
            GroupBox.Text = "Caption";
            GroupBox.Controls.SetChildIndex(textBox1, 0);
            GroupBox.Controls.SetChildIndex(StatusLabel, 0);
            //
            // lblStatus
            //
            StatusLabel.Location = new Point(12, 20);
            //
            // textBox1
            //
            textBox1.Location = new Point(44, 17);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(440, 20);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += TextBox1TextChanged;
            textBox1.Click += TextBox1Click;
            //
            // IntElement
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            Name = "IntElement";
            GroupBox.ResumeLayout(false);
            GroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}