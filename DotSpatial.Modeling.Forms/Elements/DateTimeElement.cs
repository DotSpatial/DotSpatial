// ********************************************************************************************************
// Product Name: DotSpatial.Tools.DateElement
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
// The Initial Developer of this Original Code is Teva Veluppillai. Created in March, 2010
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Modeling.Forms
{
    internal class DateTimeElement : DialogElement
    {
        #region Class Variables

        private DateTimePicker _dateTimePicker2;
        private bool _enableUpdate = true;

        #endregion

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this._dateTimePicker2 = new DateTimePicker();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            //
            // GroupBox1
            //
            this.GroupBox1.Controls.Add(this._dateTimePicker2);
            this.GroupBox1.Controls.SetChildIndex(this._dateTimePicker2, 0);
            //
            // _dateTimePicker2
            //
            this._dateTimePicker2.Format = DateTimePickerFormat.Short;
            this._dateTimePicker2.Location = new Point(55, 15);
            this._dateTimePicker2.Name = "_dateTimePicker2";
            this._dateTimePicker2.Size = new Size(200, 20);
            this._dateTimePicker2.TabIndex = 2;
            this._dateTimePicker2.ValueChanged += new EventHandler(this.dateTimePicker2_ValueChanged);
            //
            // DateTimeElement
            //
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.Name = "DateTimeElement";
            this.GroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an instance of the dialog
        /// </summary>
        /// <param name="param">The parameter this element represents</param>
        public DateTimeElement(DateTimeParam param)
        {
            //Needed by the designer
            InitializeComponent();
            Param = param;
            if (param.Value > _dateTimePicker2.MinDate)
            {
                _dateTimePicker2.Value = param.Value;
            }
            GroupBox.Text = param.Name;
            //We save the parameters passed in
            Refresh();
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
                if (Param.Value > _dateTimePicker2.MinDate)
                {
                    _dateTimePicker2.Value = Param.Value;
                }
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

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents
        /// </summary>
        public new DateTimeParam Param
        {
            get { return (DateTimeParam)base.Param; }
            set { base.Param = value; }
        }

        #endregion

        #region Events

        private static bool IsDateTime(string theValue)
        {
            try
            {
                Convert.ToDateTime(theValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (_enableUpdate)
            {
                if (IsDateTime(_dateTimePicker2.Text))
                {
                    Param.Value = Convert.ToDateTime(_dateTimePicker2.Text);
                }
                //else
                //{
                //    _dateTimePicker2.Value = _oldDate;
                //}
            }
        }

        #endregion
    }
}