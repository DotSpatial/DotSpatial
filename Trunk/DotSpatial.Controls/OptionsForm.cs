using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class OptionsForm : Form
    {
        IMap _map;

        public OptionsForm(IMap map)
        {
            InitializeComponent();
            _map = map;

            chkZoomOutFartherThanMaxExtent.Checked = _map.ZoomOutFartherThanMaxExtent;
        }

        /// <summary>
        /// Saves the changed settings.
        /// </summary>
        private void btOk_Click(object sender, EventArgs e)
        {
            _map.ZoomOutFartherThanMaxExtent = chkZoomOutFartherThanMaxExtent.Checked;
            if (!_map.ZoomOutFartherThanMaxExtent)
            {
                var maxExt = _map.GetMaxExtent();
                if (_map.Extent.Height > maxExt.Height || _map.Extent.Width > maxExt.Width)
                    _map.ZoomToMaxExtent();
            }
        }


    }
}
