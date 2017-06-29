using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ShapeEditor
{
    public partial class SnapSettingsDialog : Form
    {
        public SnapSettingsDialog()
        {
            InitializeComponent();
        }

        public bool DoSnapping
        {
            get { return this.cbPerformSnap.Checked; }
            set { this.cbPerformSnap.Checked = value; }
        }
    }
}
