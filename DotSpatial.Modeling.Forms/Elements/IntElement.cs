// ********************************************************************************************************
// Product Name: DotSpatial.Tools.IntegerElement
// Description:  Integer Element for use in the tool dialog
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
// Name                 |    Date              |   Comments
// ---------------------|----------------------|----------------------------------------------------
// Ted Dunsford         |  8/28/2009           |  Cleaned up some formatting content using re-sharper.
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// an element for integers
    /// </summary>
    public class IntElement : DialogElement
    {
        #region Class Variables

        private bool _enableUpdate = true;
        private string _oldText = string.Empty;
        private TextBox txtValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public IntElement(IntParam param)
        {
            //Needed by the designer
            InitializeComponent();
            GroupBox.Text = param.Name;

            //We save the parameters passed in
            Param = param;

            HandleStatusLight();
        }

        /// <summary>
        /// Updates the status lights
        /// </summary>
        public override void Refresh()
        {
            HandleStatusLight();
        }

        private void HandleStatusLight()
        {
            _enableUpdate = false;

            //We load the default parameters
            if (Param.DefaultSpecified)
            {
                int value = Param.Value;
                if ((value >= Param.Min) && (value <= Param.Max))
                {
                    Status = ToolStatus.Ok;
                    LightTipText = ModelingMessageStrings.ParameterValid;
                    txtValue.Text = value.ToString();
                }
                else
                {
                    Status = ToolStatus.Empty;
                    LightTipText = ModelingMessageStrings.InvalidInteger.Replace("%min", Param.Min.ToString()).Replace("%max", Param.Max.ToString());
                }
            }
            else
            {
                Status = ToolStatus.Empty;
                LightTipText = ModelingMessageStrings.ParameterInvalid;
            }

            _enableUpdate = true;
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
                if (IsInteger(txtValue.Text))
                {
                    _oldText = txtValue.Text;
                    Param.Value = Convert.ToInt32(txtValue.Text);
                }
                else
                {
                    txtValue.Text = _oldText;
                }
            }
        }

        /// <summary>
        /// Checks if text contains a value integer
        /// </summary>
        /// <param name="theValue">The text to text</param>
        /// <returns>Returns true if it is a valid integer</returns>
        private static bool IsInteger(string theValue)
        {
            try
            {
                Convert.ToInt32(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        } //IsDecimal

        /// <summary>
        /// When the text box is clicked this event fires
        /// </summary>
        private void textBox1_Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new IntParam Param
        {
            get { return (IntParam)base.Param; }
            set { base.Param = value; }
        }

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtValue = new TextBox();
            this.GroupBox.SuspendLayout();
            this.SuspendLayout();
            //
            // groupBox1
            //
            this.GroupBox.Controls.Add(this.txtValue);
            this.GroupBox.Text = "Caption";
            this.GroupBox.Controls.SetChildIndex(this.txtValue, 0);
            this.GroupBox.Controls.SetChildIndex(this.StatusLabel, 0);
            //
            // lblStatus
            //
            this.StatusLabel.Location = new Point(12, 20);
            //
            // textBox1
            //
            this.txtValue.Location = new Point(44, 17);
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new Size(440, 20);
            this.txtValue.TabIndex = 3;
            this.txtValue.TextChanged += new EventHandler(this.textBox1_TextChanged);
            this.txtValue.Click += new EventHandler(this.textBox1_Click);
            //
            // IntElement
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "IntElement";
            this.GroupBox.ResumeLayout(false);
            this.GroupBox.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}