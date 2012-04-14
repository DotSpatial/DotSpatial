namespace DotSpatial.Plugins.WFSClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DotSpatial.Controls;
    using DotSpatial.Controls.Header;
    using WFSPlugin;
    public class MyPlugin1 : Extension
    {
        private const string FileMenuKey = "WFS Client";
        // private const string HomeMenuKey = HeaderControl.HomeRootItemKey;

        public override void Activate()
        {
            App.HeaderControl.Add(new RootItem(FileMenuKey, "WFS Client") { SortOrder = -20 });
            // App.HeaderControl.Add(new SimpleActionItem(HeaderControl.ApplicationMenuKey, "About", ButtonClick) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 500 });
            IHeaderControl header = App.HeaderControl;
            header.Add(new SimpleActionItem(FileMenuKey, "WFS", ButtonClick) { GroupCaption = HeaderControl.ApplicationMenuKey, SortOrder = 5 });

            base.Activate();
        }

        public override void Deactivate()
        {
            if (App.HeaderControl != null) { App.HeaderControl.RemoveAll(); }
            base.Deactivate();
        }

        public void ButtonClick(object sender, EventArgs e)
        {
            var form = new WFSServerParameters();
            form.map = App.Map as Map;
            form.Show();
        }
    }
}
