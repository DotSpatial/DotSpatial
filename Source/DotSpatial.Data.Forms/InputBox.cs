// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Data.Forms
{
    /// <summary>
    /// frmInputBox.
    /// </summary>
    public class InputBox : Form
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private readonly IContainer _components = null;

        private Button _cmdCancel;
        private Button _cmdOk;
        private Label _lblMessageText;
        private TextBox _txtInput;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        public InputBox()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        /// <param name="text">Sets the text of the message to show.</param>
        public InputBox(string text)
            : this()
        {
            _lblMessageText.Text = text;
            Validation = ValidationType.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        public InputBox(string text, string caption)
            : this()
        {
            _lblMessageText.Text = text;
            Validation = ValidationType.None;
            Text = caption;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        /// <param name="validation">A DotSpatial.Data.ValidationType enumeration specifying acceptable validation to return OK.</param>
        public InputBox(string text, string caption, ValidationType validation)
            : this()
        {
            _lblMessageText.Text = text;
            Validation = validation;
            Text = caption;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        /// <param name="validation">A DotSpatial.Data.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="icon">Specifies an icon to appear on this messagebox.</param>
        public InputBox(string text, string caption, ValidationType validation, Icon icon)
            : this()
        {
            _lblMessageText.Text = text;
            Validation = validation;
            Text = caption;
            ShowIcon = true;
            Icon = icon;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        /// <param name="owner">Specifies the Form to set as the owner of this dialog.</param>
        /// <param name="text">Sets the text of the message to show.</param>
        public InputBox(Form owner, string text)
            : this()
        {
            Owner = owner;

            _lblMessageText.Text = text;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        /// <param name="owner">Specifies the Form  to set as the owner of this dialog.</param>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        public InputBox(Form owner, string text, string caption)
            : this()
        {
            Owner = owner;

            _lblMessageText.Text = text;
            Text = caption;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        /// <param name="owner">Specifies the Form to set as the owner of this dialog.</param>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        /// <param name="validation">A DotSpatial.Data.ValidationType enumeration specifying acceptable validation to return OK.</param>
        public InputBox(Form owner, string text, string caption, ValidationType validation)
            : this()
        {
            Owner = owner;

            _lblMessageText.Text = text;
            Text = caption;
            Validation = validation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputBox"/> class.
        /// </summary>
        /// <param name="owner">Specifies the Form to set as the owner of this dialog.</param>
        /// <param name="text">The string message to show.</param>
        /// <param name="caption">The string caption to allow.</param>
        /// <param name="validation">A DotSpatial.Data.ValidationType enumeration specifying acceptable validation to return OK.</param>
        /// <param name="icon">Specifies an icon to appear on this messagebox.</param>
        public InputBox(Form owner, string text, string caption, ValidationType validation, Icon icon)
            : this()
        {
            Owner = owner;
            Validation = validation;
            _lblMessageText.Text = text;
            Text = caption;
            ShowIcon = true;
            Icon = icon;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the string result that was entered for this text box.
        /// </summary>
        public string Result => _txtInput.Text;

        /// <summary>
        /// Gets or sets the type of validation to force on the value before the OK option is permitted.
        /// </summary>
        public ValidationType Validation { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _components?.Dispose();
            }

            base.Dispose(disposing);
        }

        private void CmdCancelClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void CmdOkClick(object sender, EventArgs e)
        {
            // Parse the value entered in the text box. If the value doesn't match the criteria, don't
            // allow the user to exit the dialog by pressing ok.
            switch (Validation)
            {
                case ValidationType.Byte:
                    if (Global.IsByte(_txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "byte"));
                        return;
                    }

                    break;
                case ValidationType.Double:
                    if (Global.IsDouble(_txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "double"));
                        return;
                    }

                    break;
                case ValidationType.Float:
                    if (Global.IsFloat(_txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "float"));
                        return;
                    }

                    break;
                case ValidationType.Integer:
                    if (Global.IsInteger(_txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "integer"));
                        return;
                    }

                    break;
                case ValidationType.PositiveDouble:
                    if (Global.IsDouble(_txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "double"));
                        return;
                    }

                    if (Global.GetDouble(_txtInput.Text) < 0)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "positive double"));
                        return;
                    }

                    break;
                case ValidationType.PositiveFloat:
                    if (Global.IsFloat(_txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "float"));
                        return;
                    }

                    if (Global.GetFloat(_txtInput.Text) < 0)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "positive float"));
                        return;
                    }

                    break;
                case ValidationType.PositiveInteger:
                    if (Global.IsInteger(_txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "integer"));
                        return;
                    }

                    if (Global.GetInteger(_txtInput.Text) < 0)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "positive integer"));
                        return;
                    }

                    break;
                case ValidationType.PositiveShort:
                    if (Global.IsShort(_txtInput.Text) == false)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "short"));
                        return;
                    }

                    if (Global.GetShort(_txtInput.Text) < 0)
                    {
                        LogManager.DefaultLogManager.LogMessageBox(DataFormsMessageStrings.ParseFailed_S.Replace("%S", "positive short"));
                        return;
                    }

                    break;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(InputBox));
            _lblMessageText = new Label();
            _txtInput = new TextBox();
            _cmdOk = new Button();
            _cmdCancel = new Button();
            SuspendLayout();

            // lblMessageText
            resources.ApplyResources(_lblMessageText, "_lblMessageText");
            _lblMessageText.Name = "_lblMessageText";

            // txtInput
            resources.ApplyResources(_txtInput, "_txtInput");
            _txtInput.Name = "_txtInput";

            // cmdOk
            resources.ApplyResources(_cmdOk, "_cmdOk");
            _cmdOk.BackColor = Color.Transparent;
            _cmdOk.DialogResult = DialogResult.OK;
            _cmdOk.Name = "_cmdOk";
            _cmdOk.UseVisualStyleBackColor = false;
            _cmdOk.Click += CmdOkClick;

            // cmdCancel
            resources.ApplyResources(_cmdCancel, "_cmdCancel");
            _cmdCancel.BackColor = Color.Transparent;
            _cmdCancel.DialogResult = DialogResult.Cancel;
            _cmdCancel.Name = "_cmdCancel";
            _cmdCancel.UseVisualStyleBackColor = false;
            _cmdCancel.Click += CmdCancelClick;

            // InputBox
            AcceptButton = _cmdOk;
            CancelButton = _cmdCancel;
            resources.ApplyResources(this, "$this");
            Controls.Add(_cmdCancel);
            Controls.Add(_cmdOk);
            Controls.Add(_txtInput);
            Controls.Add(_lblMessageText);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InputBox";
            ShowIcon = false;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}