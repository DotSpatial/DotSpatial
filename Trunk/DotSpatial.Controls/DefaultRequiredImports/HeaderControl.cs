using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls.Header;
using DotSpatial.Extensions;

namespace DotSpatial.Controls.DefaultRequiredImports
{
    /// <summary>
    /// Default Header control. It will used when no custom implementation of IHeaderControl where found.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    [DefaultRequiredImport]
    internal class HeaderControl : MenuBarHeaderControl, ISatisfyImportsExtension
    {
        private bool _isActivated;

        [Import]
        private AppManager App { get; set; }

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        public int Priority { get { return 1; } }

        public void Activate()
        {
            if (_isActivated) return;

            var headerControls = App.CompositionContainer.GetExportedValues<IHeaderControl>().ToList();

            // Activate only if there are no other IHeaderControl implementations and
            // custom HeaderControl not yet set
            if (App.HeaderControl == null &&
                headerControls.Count == 1 && headerControls[0].GetType() == GetType())
            {
                _isActivated = true;

                var toolStripContainer1 = new ToolStripContainer();
                toolStripContainer1.ContentPanel.SuspendLayout();
                toolStripContainer1.SuspendLayout();

                toolStripContainer1.Dock = DockStyle.Fill;
                toolStripContainer1.Name = "toolStripContainer1";

                // place all of the controls that were on the form originally inside of our content panel.
                foreach (Control control in Shell.Controls)
                {
                    toolStripContainer1.ContentPanel.Controls.Add(control);
                }

                Shell.Controls.Add(toolStripContainer1);

                toolStripContainer1.ContentPanel.ResumeLayout(false);
                toolStripContainer1.ResumeLayout(false);
                toolStripContainer1.PerformLayout();

                Initialize(toolStripContainer1);
            }
        }
    }
}
