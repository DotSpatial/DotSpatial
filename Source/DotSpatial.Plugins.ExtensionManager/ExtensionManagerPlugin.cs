// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionManagerPlugin.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.ExtensionManager.Properties;

namespace DotSpatial.Plugins.ExtensionManager
{
    public class ExtensionManagerPlugin : Extension
    {
        #region Fields

        ExtensionManagerForm form = new ExtensionManagerForm();
        Thread updateThread;

        #endregion

        #region  Constructors

        public ExtensionManagerPlugin()
        {
            DeactivationAllowed = false;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Activate()
        {
            AddButtons();
            form.App = App;

            updateThread = new Thread(() => Update.AutoUpdateController(App));
            updateThread.Start();
            App.UpdateSplashScreen("Looking for updates");
            App.ExtensionsActivated += ExtensionsActivated;

            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

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

                    // sample projects menu
                    var simpleActionItem = new SimpleActionItem(HeaderControl.ApplicationMenuKey, "Open sample project..", OpenSampleProjects_Click)
                                               {
                                                   GroupCaption = HeaderControl.ApplicationMenuKey,
                                                   SmallImage = Resources.plugin_16x16,
                                                   LargeImage = Resources.plugin_32x32
                                               };
                    App.HeaderControl.Add(simpleActionItem);

                    break;

                case ShowExtensionsDialogMode.MapGlyph:
                    var fun = new AppFunction
                                  {
                                      Manager = App,
                                      Map = App.Map
                                  };
                    App.Map.MapFunctions.Insert(0, fun);
                    fun.Activate();
                    break;
            }
        }

        private void ExtensionManager_Click(object sender, EventArgs e)
        {
            form.Show();
        }

        private void ExtensionsActivated(object sender, EventArgs e)
        {
            App.UpdateSplashScreen("Looking for updates");
            updateThread.Join();
            App.UpdateSplashScreen("Finished.");
        }

        private void OpenSampleProjects_Click(object sender, EventArgs e)
        {
            var sampleProjForm = new SampleProjectsForm(App);
            sampleProjForm.Show();
        }

        #endregion
    }
}