using System;
using System.Drawing;
using System.Reflection;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.About.Properties;

namespace DotSpatial.Plugins.About
{
    public class AboutPlugin : Extension
    {
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem(HeaderControl.ApplicationMenuKey, "About", ButtonClick) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 500, SmallImage = Resources.information_16x16, LargeImage = Resources.information });
            base.Activate();
        }

        public override void Deactivate()
        {
            if (App.HeaderControl != null) { App.HeaderControl.RemoveAll(); }
            base.Deactivate();
        }

        public void ButtonClick(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            var form = new AboutBox { AppEntryAssembly = assembly };

            Icon icon = Icon.ExtractAssociatedIcon(assembly.Location);
            if (icon != null)
                form.AppImage = icon;

            form.Show();
        }
    }
}