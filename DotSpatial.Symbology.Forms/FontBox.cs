// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/20/2008 9:29:29 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// FontBox
    /// </summary>
    [DefaultProperty("Value")]
    public class FontBox : UserControl
    {
        private FontDialog _fontDialog;
        private Font _value;
        private Button cmdShowDialog;
        private Label lblFont;
        private TextBox txtFont;

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FontBox
        /// </summary>
        public FontBox()
        {
            InitializeComponent();
            _fontDialog = new FontDialog();
            txtFont.Text = Value.ToString();
        }

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FontBox));
            this.lblFont = new Label();
            this.txtFont = new TextBox();
            this.cmdShowDialog = new Button();
            this.SuspendLayout();
            //
            // lblFont
            //
            this.lblFont.AccessibleDescription = null;
            this.lblFont.AccessibleName = null;
            resources.ApplyResources(this.lblFont, "lblFont");
            this.lblFont.Font = null;
            this.lblFont.Name = "lblFont";
            //
            // txtFont
            //
            this.txtFont.AccessibleDescription = null;
            this.txtFont.AccessibleName = null;
            resources.ApplyResources(this.txtFont, "txtFont");
            this.txtFont.BackgroundImage = null;
            this.txtFont.Font = null;
            this.txtFont.Name = "txtFont";
            //
            // cmdShowDialog
            //
            this.cmdShowDialog.AccessibleDescription = null;
            this.cmdShowDialog.AccessibleName = null;
            resources.ApplyResources(this.cmdShowDialog, "cmdShowDialog");
            this.cmdShowDialog.BackgroundImage = null;
            this.cmdShowDialog.Font = null;
            this.cmdShowDialog.Name = "cmdShowDialog";
            this.cmdShowDialog.UseVisualStyleBackColor = true;
            this.cmdShowDialog.Click += new EventHandler(this.cmdShowDialog_Click);
            //
            // FontBox
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackgroundImage = null;
            this.Controls.Add(this.cmdShowDialog);
            this.Controls.Add(this.txtFont);
            this.Controls.Add(this.lblFont);
            this.Font = null;
            this.Name = "FontBox";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the font that this control should be using.
        /// </summary>
        public Font Value
        {
            get
            {
                if (_value == null) _value = new Font("Microsoft Sans Serif", 9);
                return _value;
            }
            set
            {
                _value = value;
                if (_value != null)
                {
                    txtFont.Text = _value.ToString();
                }
            }
        }

        /// <summary>
        /// Gets or sets the label text for this control.
        /// </summary>
        [Category("Text"), Description("Gets or sets the label text for this control.")]
        public string LabelText
        {
            get
            {
                return lblFont.Text;
            }
            set
            {
                lblFont.Text = value;
                Reset();
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

        #endregion

        private void cmdShowDialog_Click(object sender, EventArgs e)
        {
            if (_fontDialog.ShowDialog(ParentForm) != DialogResult.OK) return;
            Value = _fontDialog.Font;
        }

        /// <summary>
        /// Changes the starting location of the color drop down based on the current text.
        /// </summary>
        private void Reset()
        {
            txtFont.Left = lblFont.Width + 5;
            txtFont.Width = cmdShowDialog.Left - txtFont.Left - 10;
        }
    }
}