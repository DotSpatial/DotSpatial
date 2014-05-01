using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace DemoMap
{
    public partial class MainForm : Form
    {
        [Export("Shell", typeof(ContainerControl))]
        private static ContainerControl Shell;

        public MainForm()
        {
            InitializeComponent();

            if (DesignMode) return;
            Shell = this;
            appManager.LoadExtensions();
        }
    }
}