using System;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace DotSpatial.Examples.BasicDesktopMapping
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            appManager1.LoadExtensions();
        }

        private void uxOpenFile_Click(object sender, EventArgs e)
        {
            uxMap.AddLayer();
        }

        private void uxZoomIn_Click(object sender, EventArgs e)
        {
            uxMap.ZoomIn();
        }

        private void uxZoomWide_Click(object sender, EventArgs e)
        {
            uxMap.ZoomToMaxExtent();
        }

        private void uxPan_Click(object sender, EventArgs e)
        {
            uxMap.FunctionMode = FunctionMode.Pan;
        }
    }
}