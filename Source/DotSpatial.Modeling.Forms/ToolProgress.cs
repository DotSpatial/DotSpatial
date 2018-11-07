// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Modeling.Forms
{
    /// <summary>
    /// A form which shows the progress of a tool
    /// </summary>
    public partial class ToolProgress : Form, ICancelProgressHandler
    {
        #region Fields

        private bool _executionComplete;
        private CultureInfo _toolProgressCulture;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolProgress"/> class.
        /// </summary>
        /// <param name="numTools">The number of tools that are going to be executed</param>
        public ToolProgress(int numTools)
        {
            InitializeComponent();
            ToolProgressCulture = new CultureInfo(string.Empty);
            _progressBarTool.Maximum = 100;
            _progressBarTool.Minimum = 0;
            _executionComplete = false;
        }

        #endregion

        private delegate void UpdateExecComp();

        private delegate void UpdateProg(string key, int percent, string message);

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the cancel button was pressed.
        /// </summary>
        public bool Cancel { get; private set; }

        /// <summary>
        /// Gets or sets the number of tools that have been successfully executed.
        /// </summary>
        public int ToolProgressCount { get; set; }

        /// <summary>
        /// gets or sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo ToolProgressCulture
        {
            get
            {
                return _toolProgressCulture;
            }

            set
            {
                if (_toolProgressCulture == value) return;

                _toolProgressCulture = value;

                if (_toolProgressCulture == null) _toolProgressCulture = new CultureInfo(string.Empty);

                Thread.CurrentThread.CurrentCulture = _toolProgressCulture;
                Thread.CurrentThread.CurrentUICulture = _toolProgressCulture;
                UpdateResources();
                Refresh();
            }
        }

        #endregion

        #region Methods

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
                BeginInvoke(prg, key, percent, message);
            }
            else
            {
                UpdateProgress(key, percent, message);
            }
        }

        private void BtnCancelClick(object sender, EventArgs e)
        {
            if (_executionComplete)
                Close();
            else
                Cancel = true;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
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

        private void UpdateResources()
        {
            resources.ApplyResources(_progressBarTool, "_progressBarTool");
            resources.ApplyResources(_lblTool, "_lblTool");
            resources.ApplyResources(_txtBoxStatus, "_txtBoxStatus");
            resources.ApplyResources(_btnCancel, "_btnCancel");
            resources.ApplyResources(this, "$this");
        }

        #endregion
    }
}