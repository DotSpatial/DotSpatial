// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/16/2009 1:18:11 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ProgressCancelDialog
    /// </summary>
    public class ProgressCancelDialog : Form, ICancelProgressHandler
    {
        private bool _cancelled;
        private Button button1;
        private Label lblProgressText;
        private SymbologyProgressBar mwProgressBar1;

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressCancelDialog));
            this.mwProgressBar1 = new DotSpatial.Symbology.Forms.SymbologyProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.lblProgressText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // mwProgressBar1
            //
            resources.ApplyResources(this.mwProgressBar1, "mwProgressBar1");
            this.mwProgressBar1.FontColor = System.Drawing.Color.Black;
            this.mwProgressBar1.Name = "mwProgressBar1";
            this.mwProgressBar1.ShowMessage = true;
            //
            // button1
            //
            resources.ApplyResources(this.button1, "button1");
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            //
            // lblProgressText
            //
            resources.ApplyResources(this.lblProgressText, "lblProgressText");
            this.lblProgressText.Name = "lblProgressText";
            //
            // ProgressCancelDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblProgressText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mwProgressBar1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressCancelDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of ProgressCancelDialog
        /// </summary>
        public ProgressCancelDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        #region Private Functions

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

        #region ICancelProgressHandler Members

        /// <inheritdoc />
        public void Progress(string key, int percent, string message)
        {
            mwProgressBar1.Value = percent;
            SetMess ms = SetMessage;
            if (InvokeRequired)
            {
                Invoke(ms, new object[] { message });
            }
            else
            {
                SetMessage(message);
            }
        }

        /// <inheritdoc />
        public bool Cancel
        {
            get { return _cancelled; }
        }

        #endregion

        /// <summary>
        /// Fires the canceled event.
        /// </summary>
        public event EventHandler Cancelled;

        private void SetMessage(string text)
        {
            lblProgressText.Text = text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _cancelled = true;
            if (Cancelled != null) Cancelled(this, EventArgs.Empty);
            Hide();
        }

        #region Nested type: SetMess

        private delegate void SetMess(string message);

        #endregion
    }
}