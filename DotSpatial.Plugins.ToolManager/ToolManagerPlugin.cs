using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Plugins.ToolManager
{
    public class ToolManagerPlugin : Extension, IPartImportsSatisfiedNotification
    {
        private Controls.ToolManager toolManager;

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        /// <summary>
        /// Gets the list tools available.
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        private IEnumerable<ITool> Tools { get; set; }

        #region IPartImportsSatisfiedNotification Members

        public void OnImportsSatisfied()
        {
            if (IsActive)
            {
                // This method may be called on another thread after recomposition.
                if (Shell.InvokeRequired)
                {
                    Shell.Invoke((MethodInvoker)ShowToolsPanel);
                }
                else
                {
                    ShowToolsPanel();
                }
            }
        }

        #endregion

        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem("Show Tools", ButtonClick));
            ShowToolsPanel();
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            this.App.DockManager.Remove("kTools");
            this.toolManager = null;
            base.Deactivate();
        }

        public void ButtonClick(object sender, EventArgs e)
        {
            ShowToolsPanel();
        }

        private void ShowToolsPanel()
        {
            if (Tools != null && Tools.Count() > 0)
            {
                if (this.toolManager == null)
                {
                    this.toolManager = new Controls.ToolManager();
                    this.toolManager.Legend = App.Legend;
                    this.toolManager.Location = new Point(208, 12);
                    this.toolManager.Name = "toolManager";
                    this.toolManager.Size = new Size(192, 308);
                    this.toolManager.TabIndex = 1;

                    App.CompositionContainer.ComposeParts(toolManager);

                    Shell.Controls.Add(this.toolManager);

                    this.App.DockManager.Add(new DockablePanel("kTools", "Tools", toolManager, DockStyle.Left) { SmallImage = this.toolManager.ImageList.Images["Hammer"] });
                }
            }
            else
            {
                this.toolManager = null;
                this.App.DockManager.Remove("kTools");
            }
        }
    }
}