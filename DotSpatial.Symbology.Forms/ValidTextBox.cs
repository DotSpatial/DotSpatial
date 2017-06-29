// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  An open source drawing pad that is super simple, but extendable
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from SketchPad.exe
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/8/2008 1:01:26 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This parses the input values and changes the background color to salmon
    /// if the value won't work as a degree.
    /// </summary>
    internal class ValidTextBox : TextBox, IErrorCheck
    {
        #region Private Variables

        private readonly ToolTip _toolTip;
        private Color _errorBackgroundColor;
        private string _errorMessage;
        private bool _hasError;
        private string _name;
        private Color _normalBackgroundColor;
        private string _normalToolTipText;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AngleTextBox
        /// </summary>
        public ValidTextBox()
        {
            _errorBackgroundColor = Color.Salmon;
            _normalBackgroundColor = Color.White;
            base.BackColor = Color.White;
            _toolTip = new ToolTip();
            _normalToolTipText = "Enter a value.";
            _toolTip.SetToolTip(this, _normalToolTipText);
            _errorMessage = "No Error.";
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text that should appear as the mouse hovers over this textbox
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the text that this control should display when not showing an error.")]
        public string NormalToolTipText
        {
            get { return _normalToolTipText; }
            set
            {
                _normalToolTipText = value;
            }
        }

        /// <summary>
        /// Hide the actual BackColor property which will be controlled
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        [Category("Appearance"), Description("Gets or sets the background color of this control if the text is not valid.")]
        public Color ErrorBackgroundColor
        {
            get { return _errorBackgroundColor; }
            set { _errorBackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the normal background color for when the value is valid.
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the normal background color for when the value is valid.")]
        public Color NormalBackgroundColor
        {
            get { return _normalBackgroundColor; }
            set { _normalBackgroundColor = value; }
        }

        /// <summary>
        /// Gets a boolean indicating if this textbox has an error.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool HasError
        {
            get
            {
                return _hasError;
            }
            protected set
            {
                _hasError = value;
            }
        }

        /// <summary>
        /// Gets a string indicating the current error for this control.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                _hasError = true;
                BackColor = ErrorBackgroundColor;
                _toolTip.SetToolTip(this, _errorMessage);
            }
        }

        /// <summary>
        /// Gets or sets the formatted name to use for this control in an error message.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the formatted name to use for this control in an error message.")]
        public string MessageName
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// This changes the error text,
        /// </summary>
        public void ClearError()
        {
            _errorMessage = "No Error.";
            _hasError = false;
            BackColor = NormalBackgroundColor;
            _toolTip.SetToolTip(this, _normalToolTipText);
        }

        #endregion
    }
}