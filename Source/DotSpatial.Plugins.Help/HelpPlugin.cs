// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Diagnostics;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.Help.Properties;

namespace DotSpatial.Plugins.Help
{
    /// <summary>
    /// Plugin that adds a help button.
    /// </summary>
    public class HelpPlugin : Extension
    {
        private const string Url = "https://github.com/DotSpatial/DotSpatial/wiki";
        private const string HelpMenu = HeaderControl.HeaderHelpItemKey;

        /// <inheritdoc />
        public override void Activate()
        {
            App.HeaderControl.Add(new RootItem(HelpMenu, "Help"));
            App.HeaderControl.Add(new SimpleActionItem(HelpMenu, "View Help", ButtonClick) { GroupCaption = HeaderControl.HeaderHelpItemKey, SmallImage = Resources.help_16x16, LargeImage = Resources.help });
            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        /// <summary>
        /// Opens the url of the documentation page.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        public void ButtonClick(object sender, EventArgs e)
        {
            Process.Start(Url);
        }
    }
}