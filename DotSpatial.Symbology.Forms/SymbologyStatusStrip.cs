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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/12/2009 10:46:15 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Data;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A pre-configured status strip with a thread safe Progress function
    /// </summary>
    [ToolboxItem(false)]
    public partial class SymbologyStatusStrip : StatusStrip, IProgressHandler
    {
        /// <summary>
        /// Creates a new instance of the StatusStrip which has a built in, thread safe Progress handler
        /// </summary>
        public SymbologyStatusStrip()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the progress bar. By default, the first ToolStripProgressBar that is added to the tool strip.
        /// </summary>
        /// <value>
        /// The progress bar.
        /// </value>
        public ToolStripProgressBar ProgressBar { get; set; }

        /// <summary>
        /// Gets or sets the progress label. By default, the first ToolStripStatusLabel that is added to the tool strip.
        /// </summary>
        /// <value>
        /// The progress label.
        /// </value>
        public ToolStripStatusLabel ProgressLabel { get; set; }

        #region IProgressHandler Members

        /// <summary>
        /// This method is thread safe so that people calling this method don't cause a cross-thread violation
        /// by updating the progress indicator from a different thread
        /// </summary>
        /// <param name="key">A string message with just a description of what is happening, but no percent completion information</param>
        /// <param name="percent">The integer percent from 0 to 100</param>
        /// <param name="message">A message</param>
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

        #endregion

        private void UpdateProgress(string key, int percent, string message)
        {
            if (ProgressBar != null)
                ProgressBar.Value = percent;
            if (ProgressLabel != null)
                ProgressLabel.Text = message;

            // hack: I think there is a bug somewhere if we need to call DoEvents at the end of this event handler.
            Application.DoEvents();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.ToolStrip.ItemAdded"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.ToolStripItemEventArgs"/> that contains the event data.</param>
        protected override void OnItemAdded(ToolStripItemEventArgs e)
        {
            base.OnItemAdded(e);

            if (ProgressBar != null)
            {
                ToolStripProgressBar pb = e.Item as ToolStripProgressBar;
                if (pb != null)
                {
                    ProgressBar = pb;
                }
            }

            if (ProgressLabel == null)
            {
                ToolStripStatusLabel sl = e.Item as ToolStripStatusLabel;
                if (sl != null)
                {
                    ProgressLabel = sl;
                }
            }
        }

        #region Nested type: UpdateProg

        private delegate void UpdateProg(string key, int percent, string message);

        #endregion
    }
}