// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This parses the input values and changes the background color to salmon
    /// if the value won't work as a degree.
    /// </summary>
    internal class ValidTextBox : TextBox
    {
        #region Fields

        private readonly ToolTip _toolTip;
        private string _errorMessage;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidTextBox"/> class.
        /// </summary>
        public ValidTextBox()
        {
            ErrorBackgroundColor = Color.Salmon;
            NormalBackgroundColor = Color.White;
            base.BackColor = Color.White;
            _toolTip = new ToolTip();
            NormalToolTipText = "Enter a value.";
            _toolTip.SetToolTip(this, NormalToolTipText);
            _errorMessage = "No Error.";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back color. Hide the actual BackColor property which will be controlled.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }

            set
            {
                base.BackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color of this control if the text is not valid.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the background color of this control if the text is not valid.")]
        public Color ErrorBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets a string indicating the current error for this control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                HasError = true;
                BackColor = ErrorBackgroundColor;
                _toolTip.SetToolTip(this, _errorMessage);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this textbox has an error.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasError { get; protected set; }

        /// <summary>
        /// Gets or sets the formatted name to use for this control in an error message.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the formatted name to use for this control in an error message.")]
        public string MessageName { get; set; }

        /// <summary>
        /// Gets or sets the normal background color for when the value is valid.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the normal background color for when the value is valid.")]
        public Color NormalBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the text that should appear as the mouse hovers over this textbox.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the text that this control should display when not showing an error.")]
        public string NormalToolTipText { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This changes the error text,.
        /// </summary>
        public void ClearError()
        {
            _errorMessage = "No Error.";
            HasError = false;
            BackColor = NormalBackgroundColor;
            _toolTip.SetToolTip(this, NormalToolTipText);
        }

        #endregion
    }
}