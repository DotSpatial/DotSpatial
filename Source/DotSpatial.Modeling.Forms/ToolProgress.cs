// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ToolProgress
// Description:  A form used to show the progress of a tool
//
// ********************************************************************************************************
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Feb, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name                  |   Date            |            Comments
// ----------------------|-------------------|-----------------------------------------------------
// Ted Dunsford          | 8/24/2009         |  Used Re-sharper to clean up a few unnecessary accessors
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A form which shows the progress of a tool
    /// </summary>
    public class ToolProgress : Form, ICancelProgressHandler
    {
        #region Variables

        private Button _btnCancel;
        private bool _executionComplete;
        private Label _lblTool;
        private ProgressBar _progressBarTool;
        private TextBox _txtBoxStatus;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the Tool Progress forms and hands over an array of tools which will then be executed
        /// </summary>
        /// <param name="numTools">The number of tools that are going to be executed</param>
        public ToolProgress(int numTools)
        {
            InitializeComponent();
            _progressBarTool.Maximum = 100;
            _progressBarTool.Minimum = 0;
            _executionComplete = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the progress method necessary to implement IProgress
        /// </summary>
        /// <param name="key">This a message with no percentage information..this is ignored</param>
        /// <param name="percent">The integer percentage from 0 to 100 that is used to control the progress bar</param>
        /// <param name="message">The actual complete message to show..this is also ignored</param>
        public void Progress(string key, int percent, string message)
        {
            if (InvokeRequired)
            {
                UpdateProg prg = UpdateProgress;
                BeginInvoke(prg, new object[] { key, percent, message });
            }
            else
            {
                UpdateProgress(key, percent, message);
            }
        }

        /// <summary>
        /// This method should be called when the process has been completed
        /// </summary>
        public void ExecutionComplete()
        {
            if (InvokeRequired)
            {
                UpdateExecComp uec = UpdateExecutionComplete;
                BeginInvoke(uec);
            }
            else
            {
                UpdateExecutionComplete();
            }
        }

        private void UpdateExecutionComplete()
        {
            _btnCancel.Text = ModelingMessageStrings.Close;
            _executionComplete = true;
            
        }

        private void UpdateProgress(string key, int percent, string message)
        {
            if (percent < 0) percent = 0;
            if (percent > 100) percent = 100;
            _progressBarTool.Value = percent;
            if (!string.IsNullOrEmpty(message))
            {
                _txtBoxStatus.AppendText("\r\n" + DateTime.Now + ": " + message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_executionComplete)
                Close();
            else
            {
                Cancel = true;
            }
        }

        #region Nested type: UpdateExecComp

        private delegate void UpdateExecComp();

        #endregion

        #region Nested type: UpdateProg

        private delegate void UpdateProg(string key, int percent, string message);

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the number of tools that have been successfully executed.
        /// </summary>
        public int ToolProgressCount { get; set; }

        /// <summary>
        /// Returns true if the cancel button was pressed.
        /// </summary>
        public bool Cancel { get; private set; }

        #endregion

        #region  Windows Form Designer generated code

        /// <summary>
        /// Required designer variable.
        /// </summary>
        protected IContainer components;

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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolProgress));
            this._progressBarTool = new System.Windows.Forms.ProgressBar();
            this._lblTool = new System.Windows.Forms.Label();
            this._txtBoxStatus = new System.Windows.Forms.TextBox();
            this._btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _progressBarTool
            // 
            this._progressBarTool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._progressBarTool.Location = new System.Drawing.Point(12, 25);
            this._progressBarTool.Name = "_progressBarTool";
            this._progressBarTool.Size = new System.Drawing.Size(494, 23);
            this._progressBarTool.TabIndex = 0;
            // 
            // _lblTool
            // 
            this._lblTool.AutoSize = true;
            this._lblTool.Location = new System.Drawing.Point(12, 9);
            this._lblTool.Name = "_lblTool";
            this._lblTool.Size = new System.Drawing.Size(72, 13);
            this._lblTool.TabIndex = 3;
            this._lblTool.Text = "Tool Progress";
            // 
            // _txtBoxStatus
            // 
            this._txtBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._txtBoxStatus.Location = new System.Drawing.Point(13, 54);
            this._txtBoxStatus.Multiline = true;
            this._txtBoxStatus.Name = "_txtBoxStatus";
            this._txtBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtBoxStatus.Size = new System.Drawing.Size(494, 323);
            this._txtBoxStatus.TabIndex = 4;
            // 
            // _btnCancel
            // 
            this._btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCancel.Location = new System.Drawing.Point(431, 383);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(75, 23);
            this._btnCancel.TabIndex = 5;
            this._btnCancel.Text = "Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ToolProgress
            // 
            this.ClientSize = new System.Drawing.Size(519, 418);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._txtBoxStatus);
            this.Controls.Add(this._lblTool);
            this.Controls.Add(this._progressBarTool);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ToolProgress";
            this.Text = "Tool Progress";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}