// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Projections.Forms
{
    /// <summary>
    /// A User control for entering double values into text boxes.
    /// </summary>
    [DefaultEvent("TextChanged")]
    [DefaultProperty("Value")]
    [ToolboxBitmap(typeof(DoubleBox), "UserControl.ico")]
    public partial class DoubleBox : UserControl
    {
        #region Fields

        private string _invalidHelp;
        private string _regularHelp;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleBox"/> class.
        /// </summary>
        public DoubleBox()
        {
            InitializeComponent();
            IsValid = true;
            _invalidHelp = ProjectionStrings.DoubleError;
            _regularHelp = ProjectionStrings.DoubleHelp;
            ttHelp.SetToolTip(lblCaption, _regularHelp);
            ttHelp.SetToolTip(txtValue, _regularHelp);
            BackColorInvalid = Color.Salmon;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the user changes values in the text box
        /// </summary>
        public new event EventHandler TextChanged;

        /// <summary>
        /// Occurs either when changing from valid to invalid or when
        /// changing from invalid to valid.
        /// </summary>
        public event EventHandler ValidChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the invalid background color.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the background color to use when the value is invalid")]
        public Color BackColorInvalid { get; set; }

        /// <summary>
        /// Gets or sets the normal background color.
        /// </summary>
        [Category("Appearance")]
        [Description("Gets or sets the normal background color to use")]
        public Color BackColorRegular { get; set; }

        /// <summary>
        /// Gets or sets the caption.
        /// </summary>
        [Category("Appearance")]
        [Localizable(true)]
        [Description("Gets or sets the string caption label")]
        public string Caption
        {
            get
            {
                return lblCaption.Text;
            }

            set
            {
                lblCaption.Text = value;
                txtValue.Left = lblCaption.Right + 10;
                txtValue.Width = Width - txtValue.Left;
            }
        }

        /// <summary>
        /// Gets or sets teh tool tip text help when this
        /// item has an invalid entry.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the help string that appears when the value is invalid.")]
        public string InvalidHelp
        {
            get
            {
                return _invalidHelp;
            }

            set
            {
                _invalidHelp = value;
                if (IsValid == false)
                {
                    ttHelp.SetToolTip(lblCaption, _invalidHelp);
                    ttHelp.SetToolTip(txtValue, _invalidHelp);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the text in this box can be parsed
        /// into a double precision value.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the boolean indicating if the initial entry is valid.")]
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the string number format.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the string number format that controls how values appear")]
        public string NumberFormat { get; set; }

        /// <summary>
        /// Gets or sets the tool tip text for regular help.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the regular tool tip help string")]
        public string RegularHelp
        {
            get
            {
                return _regularHelp;
            }

            set
            {
                _regularHelp = value;
                if (IsValid)
                {
                    ttHelp.SetToolTip(lblCaption, _regularHelp);
                    ttHelp.SetToolTip(txtValue, _regularHelp);
                }
            }
        }

        /// <summary>
        /// Gets or sets the raw text entered in the textbox.
        /// </summary>
        public override string Text
        {
            get
            {
                return txtValue.Text;
            }

            set
            {
                txtValue.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the currently entered double value.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the double precision floating point value to use for this control.")]
        public double Value
        {
            get
            {
                double val;
                double.TryParse(txtValue.Text, out val);
                return val;
            }

            set
            {
                txtValue.Text = value.ToString(NumberFormat);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires TextChanged.
        /// </summary>
        protected virtual void OnTextChanged()
        {
            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires ValidChanged.
        /// </summary>
        protected virtual void OnValidChanged()
        {
            ValidChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MakeInvalid()
        {
            IsValid = false;
            ttHelp.SetToolTip(lblCaption, _invalidHelp);
            ttHelp.SetToolTip(txtValue, _invalidHelp);
            txtValue.BackColor = BackColorInvalid;
            OnValidChanged();
        }

        private void MakeValid()
        {
            IsValid = true;
            ttHelp.SetToolTip(lblCaption, _invalidHelp);
            ttHelp.SetToolTip(txtValue, _invalidHelp);
            txtValue.BackColor = BackColorRegular;
            OnValidChanged();
        }

        private void TxtValueTextChanged(object sender, EventArgs e)
        {
            double test;
            if (double.TryParse(txtValue.Text, out test) == false)
            {
                if (IsValid) MakeInvalid();
            }
            else
            {
                if (IsValid == false) MakeValid();
            }

            OnTextChanged();
        }

        #endregion
    }
}