using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.LiDAR
{
    public class LiDARProvider : Extension
    {
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem("LiDARProvider", ButtonClick));
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        public void ButtonClick(object sender, EventArgs e)
        {
            LiDARLayer lay = new LiDARLayer();
            lay.LegendText = "LiDARLayer";
            App.Map.Layers.Add(lay);
        }
    }
}