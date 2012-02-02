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

namespace DotSpatial.Plugins.ExtensionManager
{
    public class ExtensionManagerPlugin : Extension
    {
        #region Public Methods

        public override void Activate()
        {
            AddButtons();
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
            switch (App.ShowExtensionsDialog)
            {
                case ShowExtensionsDialog.Default:
                    var simpleAction = new SimpleActionItem(HeaderControl.ApplicationMenuKey, "Extension Manager ...", ExtensionManager_Click);
                    simpleAction.SmallImage = Resources.plugin_16x16;
                    simpleAction.LargeImage = Resources.plugin_32x32;
                    simpleAction.GroupCaption = HeaderControl.ApplicationMenuKey;
                    simpleAction.SortOrder = 100;
                    App.HeaderControl.Add(simpleAction);

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
            var form = new ExtensionManagerForm();
            form.App = App;
            form.Show();
        }

        #endregion
    }
}