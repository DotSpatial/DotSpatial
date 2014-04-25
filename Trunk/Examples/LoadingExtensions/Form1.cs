using System;
using System.ComponentModel.Composition;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace DotSpatial.Examples.LoadingExtensions
{
    public partial class Form1 : Form
    {
        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl Shell;

        public Form1()
        {
            InitializeComponent();

            if (DesignMode) return;
            Shell = this;  // Required step to support GUI extensions
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