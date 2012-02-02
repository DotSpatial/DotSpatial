// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ToolProgress
// Description:  A form used to show the progress of a tool
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
// The Initial Developer of this Original Code is Brian Marchionni. Created in Feb, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name                  |   Date            |            Comments
// ----------------------|-------------------|-----------------------------------------------------
// Ted Dunsford          | 8/24/2009         |  Used Re-sharper to clean up a few unnecessary accessors
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A form which shows the progress of a tool
    /// </summary>
    public class ToolProgress : Form, ICancelProgressHandler
    {
        #region ------------------ Class Variables

        private Button _btnCancel;
        private bool _cancelPressed;
        private bool _executionComplete;
        private Label _lblTool;
        private Label _lblTotal;
        private ProgressBar _progressBarTool;
        private ProgressBar _progressBarTotal;
        private int _toolProgressCount;
        private TextBox _txtBoxStatus;

        #endregion

        #region ------------------ constructor

        /// <summary>
        /// Creates an instance of the Tool Progress forms and hands over an array of tools which will then be executed
        /// </summary>
        /// <param name="numTools">The number of tools that are going to be executed</param>
        public ToolProgress(int numTools)
        {
            InitializeComponent();
            _progressBarTotal.Maximum = numTools * 100;
            _progressBarTotal.Minimum = 0;
            _progressBarTool.Maximum = 100;
            _progressBarTool.Minimum = 0;
            _executionComplete = false;
        }

        #endregion

        #region ------------------- Methods

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
            _btnCancel.Text = "Close";
            _executionComplete = true;
        }

        private void UpdateProgress(string key, int percent, string message)
        {
            if (percent < 0) percent = 0;
            if (percent > 100) percent = 100;
            _progressBarTool.Value = percent;
            _progressBarTotal.Value = (_toolProgressCount) * 100 + percent;
            _txtBoxStatus.AppendText("\r\n" + DateTime.Now + ": " + message);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_executionComplete)
                Close();
            else
            {
                _cancelPressed = true;
                return;
            }
        }

        #region Nested type: UpdateExecComp

        private delegate void UpdateExecComp();

        #endregion

        #region Nested type: UpdateProg

        private delegate void UpdateProg(string key, int percent, string message);

        #endregion

        #endregion

        #region ---------------------- Properties

        /// <summary>
        /// Gets or sets the number of tools that have been succesfully executed
        /// </summary>
        public int ToolProgressCount
        {
            get { return _toolProgressCount; }
            set { _toolProgressCount = value; }
        }

        /// <summary>
        /// Returns true if the cancel button was pressed
        /// </summary>
        public bool Cancel
        {
            get { return _cancelPressed; }
        }

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
            _progressBarTool = new ProgressBar();
            _progressBarTotal = new ProgressBar();
            _lblTotal = new Label();
            _lblTool = new Label();
            _txtBoxStatus = new TextBox();
            _btnCancel = new Button();
            SuspendLayout();
            //
            // progressBarTool
            //
            _progressBarTool.Anchor = (AnchorStyles.Top | AnchorStyles.Left)
                                      | AnchorStyles.Right;
            _progressBarTool.Location = new Point(13, 77);
            _progressBarTool.Name = "_progressBarTool";
            _progressBarTool.Size = new Size(494, 23);
            _progressBarTool.TabIndex = 0;
            //
            // _progressBarTotal
            //
            _progressBarTotal.Anchor = (AnchorStyles.Top | AnchorStyles.Left)
                                       | AnchorStyles.Right;
            _progressBarTotal.Location = new Point(13, 33);
            _progressBarTotal.Name = "_progressBarTotal";
            _progressBarTotal.Size = new Size(494, 23);
            _progressBarTotal.TabIndex = 1;
            //
            // _lblTotal
            //
            _lblTotal.AutoSize = true;
            _lblTotal.Location = new Point(12, 15);
            _lblTotal.Name = "_lblTotal";
            _lblTotal.Size = new Size(75, 13);
            _lblTotal.TabIndex = 2;
            _lblTotal.Text = "Total Progress";
            //
            // _lblTool
            //
            _lblTool.AutoSize = true;
            _lblTool.Location = new Point(12, 59);
            _lblTool.Name = "_lblTool";
            _lblTool.Size = new Size(72, 13);
            _lblTool.TabIndex = 3;
            _lblTool.Text = "Tool Progress";
            //
            // _txtBoxStatus
            //
            _txtBoxStatus.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom)
                                    | AnchorStyles.Left)
                                   | AnchorStyles.Right;
            _txtBoxStatus.Location = new Point(13, 130);
            _txtBoxStatus.Multiline = true;
            _txtBoxStatus.Name = "_txtBoxStatus";
            _txtBoxStatus.ScrollBars = ScrollBars.Vertical;
            _txtBoxStatus.Size = new Size(494, 247);
            _txtBoxStatus.TabIndex = 4;
            //
            // _btnCancel
            //
            _btnCancel.Location = new Point(431, 383);
            _btnCancel.Name = "_btnCancel";
            _btnCancel.Size = new Size(75, 23);
            _btnCancel.TabIndex = 5;
            _btnCancel.Text = "Cancel";
            _btnCancel.UseVisualStyleBackColor = true;
            _btnCancel.Click += btnCancel_Click;
            //
            // ToolProgress
            //
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(519, 418);
            Controls.Add(_btnCancel);
            Controls.Add(_txtBoxStatus);
            Controls.Add(_lblTool);
            Controls.Add(_lblTotal);
            Controls.Add(_progressBarTotal);
            Controls.Add(_progressBarTool);
            Name = "ToolProgress";
            Text = "ToolProgress";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}