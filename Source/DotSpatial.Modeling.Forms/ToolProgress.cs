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

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolProgress"/> class.
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

        #endregion
    }
}