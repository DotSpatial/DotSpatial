// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionManagerPlugin.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.ExtensionManager.Properties;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class ExtensionManagerPlugin : Extension
    {
        ExtensionManagerForm form = new ExtensionManagerForm();

        public ExtensionManagerPlugin()
        {
            DeactivationAllowed = false;
        }

        #region Public Methods

        public override void Activate()
        {
            AddButtons();
            base.Activate();
            form.App = App;
            Update.autoUpdateController(App, form);
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        #endregion

        #region Methods

        private void AddButtons()
        {
            switch (App.ShowExtensionsDialog)
            {
                case ShowExtensionsDialog.Default:
                    var simpleAction = new SimpleActionItem(HeaderControl.ApplicationMenuKey, "Extension Manager...", ExtensionManager_Click);
                    simpleAction.SmallImage = Resources.plugin_16x16;
                    simpleAction.LargeImage = Resources.plugin_32x32;
                    simpleAction.GroupCaption = HeaderControl.ApplicationMenuKey;
                    simpleAction.SortOrder = 100;
                    App.HeaderControl.Add(simpleAction);

                    //sample projects menu
                    SimpleActionItem simpleActionItem = new SimpleActionItem(HeaderControl.ApplicationMenuKey, "Open sample project..", OpenSampleProjects_Click);
			        simpleActionItem.GroupCaption = "kApplicationMenu";
			        simpleActionItem.LargeImage = Resources.plugin_32x32;
			        simpleActionItem.SmallImage = Resources.plugin_16x16;
			        base.App.HeaderControl.Add(simpleActionItem);

                    break;

                case ShowExtensionsDialog.MapGlyph:
                    AppFunction fun = new AppFunction { Manager = App, Map = App.Map };
                    App.Map.MapFunctions.Insert(0, fun);
                    fun.Activate();
                    break;

                case ShowExtensionsDialog.None:
                default:
                    break;
            }
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose is called when a non-modal form is closed.")]
        private void ExtensionManager_Click(object sender, EventArgs e)
        {
            form.Show();
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Dispose is called when a non-modal form is closed.")]
        private void OpenSampleProjects_Click(object sender, EventArgs e)
        {
            SampleProjectsForm sampleProjForm = new SampleProjectsForm(App);
            sampleProjForm.Show();
        }

        #endregion
    }
}