using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.WFSClient
{
    public class WfsPlugin : Extension
    {
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem("WFS", ButtonClick));

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
