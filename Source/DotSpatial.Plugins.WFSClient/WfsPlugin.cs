using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.WFSClient
{
    /// <summary>
    /// WfsPlugin
    /// </summary>
    public class WfsPlugin : Extension
    {
        #region Methods

        /// <inheritdoc />
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem("WFS", ButtonClick));

            base.Activate();
        }

        /// <summary>
        /// Opens a windows for entering wfs server parameters.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        public void ButtonClick(object sender, EventArgs e)
        {
            var form = new WfsServerParameters
            {
                Map = App.Map as Map
            };
            form.Show();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.HeaderControl?.RemoveAll();

            base.Deactivate();
        }

        #endregion
    }
}