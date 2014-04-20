using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Examples.AppManagerCustomizationDesignTime
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

            appManager.UpdateProgress("Control is Ready"); // Show some status message
        }
    }
}
