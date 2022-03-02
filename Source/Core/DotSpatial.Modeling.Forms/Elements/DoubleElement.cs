// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Modeling.Forms.Parameters;

namespace DotSpatial.Modeling.Forms.Elements
{
    /// <summary>
    /// An element for doubles.
    /// </summary>
    public partial class DoubleElement : DialogElement
    {
        #region Fields

        private bool _enableUpdate = true;
        private string _oldText = string.Empty;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleElement"/> class.
        /// </summary>
        /// <param name="param">The parameter this element represents.</param>
        public DoubleElement(DoubleParam param)
        {
            // Needed by the designer
            InitializeializeializeComponent();
            GroupBox.Text = param.Name;

            // We save the Parameter passed in
            Param = param;

            DoRefresh();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Parameter that the element represents.
        /// </summary>
        public new DoubleParam Param
        {
            get
            {
                return (DoubleParam)base.Param;
            }

            set
            {
                base.Param = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes status lights or other content.
        /// </summary>
        public override void Refresh()
        {
            DoRefresh();
        }

        /// <summary>
        /// Checks if text contains a value double.
        /// </summary>
        /// <param name="theValue">The text to text.</param>
        /// <returns>Returns true if it is a valid double.</returns>
        private static bool IsDecimal(string theValue)
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

        private void DoRefresh()
        {
            if (_enableUpdate == false) return;

            _enableUpdate = false;

            // We load the default Parameter
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
        /// When the text box is clicked this event fires.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TextBox1Click(object sender, EventArgs e)
        {
            OnClick(e);
        }

        /// <summary>
        /// This changes the color of the light and the tooltip of the light based on the status of the text in the box.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        private void TextBox1TextChanged(object sender, EventArgs e)
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

        #endregion
    }
}