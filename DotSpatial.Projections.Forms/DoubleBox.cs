// ********************************************************************************************************
// Product Name: DotSpatial.Projections.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created before 2010.
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Projections.Forms
{
    /// <summary>
    /// A User control for entering double values into text boxes
    /// </summary>
    [DefaultEvent("TextChanged"), DefaultProperty("Value"),
    ToolboxBitmap(typeof(DoubleBox), "UserControl.ico")]
    public class DoubleBox : UserControl
    {
        /// <summary>
        /// Occurs when the user changes values in the text box
        /// </summary>
        public new event EventHandler TextChanged;
        /// <summary>
        /// Occurs either when changing from valid to invalid or when
        /// changing from invalid to valid.
        /// </summary>
        public event EventHandler ValidChanged;

        #region Private Variables

        private Color _backColorInvalid;
        private Color _backColorRegular;
        private string _format;
        private string _invalidHelp;
        private bool _isValid;
        private string _regularHelp;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private Label lblCaption;
        private ToolTip ttHelp;
        private TextBox txtValue;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DoubleBox
        /// </summary>
        public DoubleBox()
        {
            InitializeComponent();
            _isValid = true;
            _invalidHelp = ProjectionStrings.DoubleError;
            _regularHelp = ProjectionStrings.DoubleHelp;
            ttHelp.SetToolTip(lblCaption, _regularHelp);
            ttHelp.SetToolTip(txtValue, _regularHelp);
            _backColorInvalid = Color.Salmon;
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DoubleBox));
            this.lblCaption = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            //
            // lblCaption
            //
            resources.ApplyResources(this.lblCaption, "lblCaption");
            this.lblCaption.Name = "lblCaption";
            this.ttHelp.SetToolTip(this.lblCaption, resources.GetString("lblCaption.ToolTip"));
            //
            // txtValue
            //
            resources.ApplyResources(this.txtValue, "txtValue");
            this.txtValue.Name = "txtValue";
            this.ttHelp.SetToolTip(this.txtValue, resources.GetString("txtValue.ToolTip"));
            this.txtValue.TextChanged += new System.EventHandler(this.txtValue_TextChanged);
            //
            // DoubleBox
            //
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtValue);
            this.Controls.Add(this.lblCaption);
            this.Name = "DoubleBox";
            this.ttHelp.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the normal background color
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the normal background color to use")]
        public Color BackColorRegular
        {
            get { return _backColorRegular; }
            set { _backColorRegular = value; }
        }

        /// <summary>
        /// Gets or sets the invalid background color
        /// </summary>
        [Category("Appearance"), Description("Gets or sets the background color to use when the value is invalid")]
        public Color BackColorInvalid
        {
            get { return _backColorInvalid; }
            set { _backColorInvalid = value; }
        }

        /// <summary>
        /// Gets or sets the caption
        /// </summary>
        [Category("Appearance"), Localizable(true), Description("Gets or sets the string caption label")]
        public string Caption
        {
            get { return lblCaption.Text; }
            set
            {
                lblCaption.Text = value;
                txtValue.Left = lblCaption.Right + 10;
                txtValue.Width = Width - txtValue.Left;
            }
        }

        /// <summary>
        /// Gets or sets the string number format
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the string number format that controls how values appear")]
        public string NumberFormat
        {
            get { return _format; }
            set { _format = value; }
        }

        /// <summary>
        /// Gets or sets teh tool tip text help when this
        /// item has an invalid entry.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the help string that appears when the value is invalid.")]
        public string InvalidHelp
        {
            get { return _invalidHelp; }
            set
            {
                _invalidHelp = value;
                if (_isValid == false)
                {
                    ttHelp.SetToolTip(lblCaption, _invalidHelp);
                    ttHelp.SetToolTip(txtValue, _invalidHelp);
                }
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if the text in this box can be parsed
        /// into a double precision value.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the boolean indicating if the initial entry is valid.")]
        public bool IsValid
        {
            get { return _isValid; }
            set { _isValid = value; }
        }

        /// <summary>
        /// Gets or sets the tool tip text for regular help
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the regular tool tip help string")]
        public string RegularHelp
        {
            get { return _regularHelp; }
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
        /// Gets the raw text entered in the textbox
        /// </summary>
        public override string Text
        {
            get { return txtValue.Text; }
            set { txtValue.Text = value; }
        }

        /// <summary>
        /// Gets the currently entered double value.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the double precision floating point value to use for this control.")]
        public double Value
        {
            get
            {
                double val = 0;
                double.TryParse(txtValue.Text, out val);
                return val;
            }
            set
            {
                txtValue.Text = value.ToString(_format);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Fires TextChanged
        /// </summary>
        protected virtual void OnTextChanged()
        {
            if (TextChanged != null) TextChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires ValidChanged
        /// </summary>
        protected virtual void OnValidChanged()
        {
            if (ValidChanged != null) ValidChanged(this, EventArgs.Empty);
        }

        #endregion

        #region Private Methods

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            double test;
            if (double.TryParse(txtValue.Text, out test) == false)
            {
                if (_isValid) MakeInvalid();
            }
            else
            {
                if (_isValid == false) MakeValid();
            }
            OnTextChanged();
        }

        private void MakeInvalid()
        {
            _isValid = false;
            ttHelp.SetToolTip(lblCaption, _invalidHelp);
            ttHelp.SetToolTip(txtValue, _invalidHelp);
            txtValue.BackColor = _backColorInvalid;
            OnValidChanged();
        }

        private void MakeValid()
        {
            _isValid = true;
            ttHelp.SetToolTip(lblCaption, _invalidHelp);
            ttHelp.SetToolTip(txtValue, _invalidHelp);
            txtValue.BackColor = _backColorRegular;
            OnValidChanged();
        }

        #endregion
    }
}