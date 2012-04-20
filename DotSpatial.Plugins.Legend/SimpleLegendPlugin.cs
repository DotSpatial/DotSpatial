using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.SimpleLegend
{
    public class SimpleLegendPlugin : Extension
    {
        private Legend legend1;

        public override void Activate()
        {
            ShowLegend();
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            this.App.DockManager.Remove("kLegend");
            base.Deactivate();
        }

        private void ShowLegend()
        {

            this.legend1 = new DotSpatial.Controls.Legend();
            // 
            // legend1
            // 
            this.legend1.BackColor = System.Drawing.Color.White;
            this.legend1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 176, 128);
            this.legend1.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 34, 114);
            this.legend1.HorizontalScrollEnabled = true;
            this.legend1.Indentation = 30;
            this.legend1.IsInitialized = false;
            this.legend1.Location = new System.Drawing.Point(217, 12);
            this.legend1.MinimumSize = new System.Drawing.Size(5, 5);
            this.legend1.Name = "legend1";
            this.legend1.ProgressHandler = null;
            this.legend1.ResetOnResize = false;
            this.legend1.SelectionFontColor = System.Drawing.Color.Black;
            this.legend1.SelectionHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.legend1.Size = new System.Drawing.Size(176, 128);
            this.legend1.TabIndex = 0;
            this.legend1.Text = "Legend";
            this.legend1.VerticalScrollEnabled = true;

            App.Map.Legend = legend1;
            App.Legend = this.legend1;
            App.DockManager.Add(new DockablePanel("kLegend", "Legend", legend1, DockStyle.Left) { SmallImage = Properties.Resources.legend_16x16 });
        }
    }
}