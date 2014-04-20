using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace DotSpatial.Examples.CustomAppManagerControls
{
    public partial class Form1 : Form
    {
        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl Shell;

        public Form1()
        {
            InitializeComponent();

            if (DesignMode) return;
            Shell = this;
            appManager.LoadExtensions();


            appManager.UpdateProgress("Ready"); // Show some status message
        }
    }
}
