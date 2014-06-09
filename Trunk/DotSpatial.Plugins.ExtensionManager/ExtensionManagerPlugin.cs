// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionManagerPlugin.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.ExtensionManager.Properties;

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
            form.App = App;

            var updateThread = new Thread(() => Update.autoUpdateController(App, form));
            var timeStarted = DateTime.UtcNow;
            updateThread.Start();

            // Update splash screen's progress bar while thread is active or 10 seconds have past.
            var span = TimeSpan.FromMilliseconds(0);
            while (updateThread.IsAlive && span.TotalMilliseconds < 10000)
            {
                App.UpdateSplashScreen("Looking for updates");
                span = DateTime.UtcNow - timeStarted;
            }

            // Join the threads. If the thread is still active, wait a full second before giving up.
            updateThread.Join(1000);
            App.UpdateSplashScreen("Finished.");

            base.Activate();
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
            switch (App.ShowExtensionsDialogMode)
            {
                case ShowExtensionsDialogMode.Default:
                    var simpleAction = new SimpleActionItem(HeaderControl.ApplicationMenuKey, "Extension Manager...", ExtensionManager_Click)
                    {
                        GroupCaption = HeaderControl.ApplicationMenuKey,
                        SmallImage = Resources.plugin_16x16,
                        LargeImage = Resources.plugin_32x32,
                        SortOrder = 100
                    };
                    App.HeaderControl.Add(simpleAction);

                    //sample projects menu
                    var simpleActionItem = new SimpleActionItem(HeaderControl.ApplicationMenuKey, "Open sample project..", OpenSampleProjects_Click)
                    {
                        GroupCaption = HeaderControl.ApplicationMenuKey,
                        SmallImage = Resources.plugin_16x16,
                        LargeImage = Resources.plugin_32x32
                    };
                    App.HeaderControl.Add(simpleActionItem);

                    break;

                case ShowExtensionsDialogMode.MapGlyph:
                    var fun = new AppFunction { Manager = App, Map = App.Map };
                    App.Map.MapFunctions.Insert(0, fun);
                    fun.Activate();
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