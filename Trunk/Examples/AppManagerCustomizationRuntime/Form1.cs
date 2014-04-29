using System.ComponentModel.Composition;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace DotSpatial.Examples.AppManagerCustomizationRuntime
{
    public partial class Form1 : Form
    {
        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl Shell;

        public Form1()
        {
            InitializeComponent();

            if (DesignMode) return;

            //------ These 2 lines required to load extensions
            Shell = this;
            appManager.LoadExtensions();
            //-----------

            appManager.UpdateProgress("Ready"); // Show some status message

            // Add some menus
            new DefaultMenuBars(appManager).Initialize(appManager.HeaderControl);
        }
    }
}
