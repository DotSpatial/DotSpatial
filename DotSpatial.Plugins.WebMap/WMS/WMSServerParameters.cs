using System;
using System.Windows.Forms;

namespace DotSpatial.Plugins.WebMap.WMS
{
    public partial class WMSServerParameters : Form
    {
        public WMSServerParameters()
        {
            InitializeComponent();
            WmsServerInfo = new WmsServerInfo();
        }
      
        private void btnGetCapabilities_Click(object sender, EventArgs e)
        {
            var serverUrl = tbServerUrl.Text;

            var wmsClient = new WMSClient();
            wmsClient.ReadCapabilities(serverUrl);
            WmsServerInfo.Version = wmsClient.Version;
            WmsServerInfo.OnlineResource = wmsClient.ServiceDescription.OnlineResource;

            var layers = wmsClient.GetVisibleLayer();
            dgvLayers.DataSource = layers;
        }

        public WmsServerInfo WmsServerInfo { get; private set; }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgvLayers.SelectedRows.Count > 0)
            {
                WmsServerInfo.Layer = ((WMSClient.ServerLayer) dgvLayers.SelectedRows[0].DataBoundItem).Name;
            }
            DialogResult = DialogResult.OK;
        }
    }

    public class WmsServerInfo
    {
        public string OnlineResource { get; set; }
        public string Version { get; set; }
        public string Layer { get; set; }
    }
}
