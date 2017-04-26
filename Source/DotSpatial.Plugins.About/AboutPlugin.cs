using System;
using System.Drawing;
using System.Reflection;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.About.Properties;

namespace DotSpatial.Plugins.About
{
    /// <summary>
    /// This plugin adds an about window button to the application menu.
    /// </summary>
    public class AboutPlugin : Extension
    {
        /// <inheritdoc/>
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem(HeaderControl.ApplicationMenuKey, "About", ButtonClick) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 500, SmallImage = Resources.information_16x16, LargeImage = Resources.information });
            base.Activate();
        }

        /// <inheritdoc/>
        public override void Deactivate()
        {
            App.HeaderControl?.RemoveAll();
            base.Deactivate();
        }

        private static void ButtonClick(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            var form = new AboutBox { AppEntryAssembly = assembly };

            if (assembly != null && assembly.Location != null)
            {
                Icon icon = Icon.ExtractAssociatedIcon(assembly.Location);
                if (icon != null)
                    form.AppImage = icon;
            }

            form.Show();
        }
    }
}