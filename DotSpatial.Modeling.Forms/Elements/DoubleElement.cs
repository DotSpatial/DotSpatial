// ********************************************************************************************************
// Product Name: DotSpatial.Tools.doubleElement
// Description:  Double Element for use in the tool dialog
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
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Nov, 2008.
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
    /// An element for doubles
    /// </summary>
    public class DoubleElement : DialogElement
    {
        #region Class Variables

        private bool _enableUpdate = true;
        private string _oldText = string.Empty;
        private TextBox textBox1;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public DoubleElement(DoubleParam param)
        {
            //Needed by the designer
            InitializeializeializeComponent();
            GroupBox.Text = param.Name;

            //We save the Parameter passed in
            Param = param;

            DoRefresh();
        }

        private void DoRefresh()
        {
            if (_enableUpdate == false) return;

            _enableUpdate = false;

            //We load the default Parameter
            if (Param.DefaultSpecified)
            {
                double value = Param.Value;
                if ((value >= Param.Min) && (value <= Param.Max))
                {
                    Status = ToolStatus.Ok;
                    LightTipText = ModelingMessageStrings.ParameterValid;
                    textBox1.Text = value.ToString();
                }
                else
                {
                    Status = ToolStatus.Empty;
                    LightTipText = ModelingMessageStrings.InvalidDouble.Replace("%min", Param.Min.ToString()).Replace("%max", Param.Max.ToString());
                }
            }
            else
            {
                Status = ToolStatus.Empty;
                LightTipText = ModelingMessageStrings.ParameterInvalid;
            }

            _enableUpdate = true;
        }

        /// <summary>
        /// Refreshes status lights or other content
        /// </summary>
        public override void Refresh()
        {
            DoRefresh();
        }

        #endregion

        #region Events

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_enableUpdate)
            {
                if (IsDecimal(textBox1.Text))
                {
                    _oldText = textBox1.Text;
                    Param.Value = Convert.ToDouble(textBox1.Text);
                }
                else
                {
                    textBox1.Text = _oldText;
                }
            }
        }

        /// <summary>
        /// Checks if text contains a value double
        /// </summary>
        /// <param name="theValue">The text to text</param>
        /// <returns>Returns true if it is a valid double</returns>
        private bool IsDecimal(string theValue)
        {
            try
            {
                Convert.ToDouble(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// When the text box is clicked this event fires
        /// </summary>
        private void textBox1_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new DoubleParam Param
        {
            get { return (DoubleParam)base.Param; }
            set { base.Param = value; }
        }

        #endregion

        #endregion

        #region Windows Form Designer generated code

        private void InitializeializeializeComponent()
        {
            this.textBox1 = new TextBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBox1
            //
            this.GroupBox.Controls.Add(this.textBox1);
            this.GroupBox.Size = new Size(492, 46);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            this.GroupBox.Controls.SetChildIndex(this.textBox1, 0);
            //
            // lblStatus
            //
            this.StatusLabel.Location = new Point(12, 20);
            //
            // textBox1
            //
            this.textBox1.Location = new Point(44, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(440, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.textBox1.Click += new EventHandler(this.textBox1_Click);
            //
            // DoubleElement
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "DoubleElement";
            this.Size = new Size(492, 46);
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // DoubleElement
            //
            this.Name = "DoubleElement";
            this.ResumeLayout(false);
        }
    }
}