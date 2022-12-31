using System.ComponentModel.Composition;
using System.Windows.Forms;

namespace SimpleApp
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
            appManager1.LoadExtensions();
        }
    }
}
