using System;
using System.Threading;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.ExtensionManager.Properties;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// This plugin is used to add the extension manager.
    /// </summary>
    public class ExtensionManagerPlugin : Extension
    {
        #region Fields

        private readonly ExtensionManagerForm _form = new ExtensionManagerForm();
        private Thread _updateThread;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionManagerPlugin"/> class.
        /// </summary>
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
            _form.App = App;

            _updateThread = new Thread(() => Update.AutoUpdateController(App));
            _updateThread.Start();
            App.UpdateSplashScreen(Resources.LookingForUpdates);
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
                    var simpleAction = new SimpleActionItem(HeaderControl.ApplicationMenuKey, Resources.ExtensionManager, ExtensionManagerClick)
                    {
                        GroupCaption = HeaderControl.ApplicationMenuKey,
                        SmallImage = Resources.plugin_16x16,
                        LargeImage = Resources.plugin_32x32,
                        SortOrder = 100
                    };
                    App.HeaderControl.Add(simpleAction);

                    // sample projects menu
                    var simpleActionItem = new SimpleActionItem(HeaderControl.ApplicationMenuKey, Resources.OpenSampleProject, OpenSampleProjectsClick)
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

        private void ExtensionManagerClick(object sender, EventArgs e)
        {
            _form.Show();
        }

        private void ExtensionsActivated(object sender, EventArgs e)
        {
            App.UpdateSplashScreen(Resources.LookingForUpdates);
            _updateThread.Join();
            App.UpdateSplashScreen(Resources.Finished);
        }

        private void OpenSampleProjectsClick(object sender, EventArgs e)
        {
            var sampleProjForm = new SampleProjectsForm(App);
            sampleProjForm.Show();
        }

        #endregion
    }
}