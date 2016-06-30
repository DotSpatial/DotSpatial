using System;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This allows the user to switch between stop zooming out on max extent and zooming out farther than max extent.
    /// </summary>
    public partial class OptionsForm : Form
    {
        private readonly IMap _map;

        /// <summary>
        /// Creates a new instance of options form.
        /// </summary>
        /// <param name="map">The IMap the options should be applied to.</param>
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
