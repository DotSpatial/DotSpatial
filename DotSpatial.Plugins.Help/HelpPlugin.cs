using System;
using System.Diagnostics;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.Help.Properties;

namespace DotSpatial.Plugins.Help
{
    public class HelpPlugin : Extension
    {
        private const string Url = "http://dotspatial.codeplex.com/documentation";
        private const string HelpMenu = HeaderControl.HeaderHelpItemKey;

        public override void Activate()
        {
            App.HeaderControl.Add(new RootItem(HelpMenu, "Help"));
            App.HeaderControl.Add(new SimpleActionItem(HelpMenu, "View Help", ButtonClick) { GroupCaption = HeaderControl.HeaderHelpItemKey, SmallImage = Resources.help_16x16, LargeImage = Resources.help });
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        public void ButtonClick(object sender, EventArgs e)
        {
            Process.Start(Url);
        }
    }
}