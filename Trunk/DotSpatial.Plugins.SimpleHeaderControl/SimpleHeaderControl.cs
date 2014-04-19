// -----------------------------------------------------------------------
// <copyright file="SimpleHeaderControl.cs" company="DotSpatial Team">
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Windows.Forms;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.SimpleHeaderControl
{
    /// <summary>
    /// Creates a ToolStripContainer that hosts a MenuBarHeaderControl.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    public class SimpleHeaderControl : MenuBarHeaderControl, IPartImportsSatisfiedNotification
    {
        private ToolStripContainer toolStripContainer1;

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        #region IPartImportsSatisfiedNotification Members

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use. (Shell will have a value)
        /// </summary>
        public void OnImportsSatisfied()
        {
            toolStripContainer1 = new ToolStripContainer();
            toolStripContainer1.ContentPanel.SuspendLayout();
            toolStripContainer1.SuspendLayout();

            toolStripContainer1.Dock = DockStyle.Fill;
            toolStripContainer1.Name = "toolStripContainer1";

            // place all of the controls that were on the form originally inside of our content panel.
            while (Shell.Controls.Count > 0)
            {
                foreach (Control control in Shell.Controls)
                {
                    toolStripContainer1.ContentPanel.Controls.Add(control);
                }
            }

            Shell.Controls.Add(toolStripContainer1);

            toolStripContainer1.ContentPanel.ResumeLayout(false);
            toolStripContainer1.ResumeLayout(false);
            toolStripContainer1.PerformLayout();

            Initialize(toolStripContainer1);
        }

        #endregion
    }
}