using System;
using System.Net;
using System.Windows.Forms;
using BruTile.Web.Wms;
using Exception = System.Exception;

namespace DotSpatial.Plugins.WebMap.WMS_New
{
    public partial class WMSServerParameters : Form
    {
        private WmsCapabilities _wmsCapabilities;

        public WMSServerParameters(WmsInfo data)
        {
            WmsInfo = data;
            InitializeComponent();

            if (WmsInfo != null)
            {
                _wmsCapabilities = WmsInfo.WmsCapabilities;
                tbServerUrl.Text = WmsInfo.ServerUrl;
                InitLayers(WmsInfo.WmsCapabilities);
                tvLayers.SelectedNode = FindNodeByLayer(tvLayers.Nodes, WmsInfo.Layer);
                if (tvLayers.SelectedNode != null) tvLayers.SelectedNode.EnsureVisible();

                var toFind = new BoundingBoxWrapper(WmsInfo.BoundingBox).ToString();
                foreach (BoundingBoxWrapper bbw in lbBoundingBox.Items)
                {
                    if (bbw.ToString() == toFind)
                    {
                        lbBoundingBox.SelectedItem = bbw;
                        break;
                    }
                }

                tvLayers.Select();
                tvLayers.Focus();
            }
        }

        private static TreeNode FindNodeByLayer(TreeNodeCollection nodes, Layer layer)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag == layer) return node;
                var ch = FindNodeByLayer(node.Nodes, layer);
                if (ch != null) return ch;
            }
            return null;
        }

        public WmsInfo WmsInfo { get; private set; }

        private void btnGetCapabilities_Click(object sender, EventArgs e)
        {
            var serverUrl = tbServerUrl.Text;
            if (String.IsNullOrWhiteSpace(serverUrl)) return;

            if (serverUrl.IndexOf("Request=GetCapabilities", StringComparison.OrdinalIgnoreCase) < 0)
            {
                serverUrl = serverUrl + "&Request=GetCapabilities";
            }

            WmsCapabilities capabilities;
            try
            {
                var myRequest = WebRequest.Create(serverUrl);
                using (var myResponse = myRequest.GetResponse())
                using (var stream = myResponse.GetResponseStream())
                    capabilities = new WmsCapabilities(stream);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to read capabilities: " + ex.Message);
                return;
            }
            WmsInfo = null;
            _wmsCapabilities = capabilities;
            InitLayers(capabilities);
        }

        private void InitLayers(WmsCapabilities capabilities)
        {
            tvLayers.Nodes.Clear();
            FillTree(tvLayers.Nodes, capabilities.Capability.Layer);
            tvLayers.ExpandAll();   
        }

        private static void FillTree(TreeNodeCollection collection, Layer root)
        {
            var node = new TreeNode(root.Title) { Tag = root };
            collection.Add(node);
            foreach (var childLayer in root.ChildLayers)
            {
                FillTree(node.Nodes, childLayer);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_wmsCapabilities == null)
            {
                MessageBox.Show("Select server to view.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tvLayers.SelectedNode == null)
            {
                MessageBox.Show("Select layer to view.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (lbBoundingBox.SelectedItem == null)
            {
                MessageBox.Show("Select BoundingBox to view.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            WmsInfo = new WmsInfo(tbServerUrl.Text, _wmsCapabilities,
                (Layer) tvLayers.SelectedNode.Tag,
                ((BoundingBoxWrapper) lbBoundingBox.SelectedItem).Box
                );
            DialogResult = DialogResult.OK;
        }

        private void tvLayers_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!e.Node.IsSelected) return;
            var layer = (Layer)e.Node.Tag;

            tbTitle.Text = layer.Title.Trim();
            lblQuerable.Text = "Queryable: " + (layer.Queryable ? "Yes" : "No");
            lblOpaque.Text = "Opaque: " + (layer.Opaque ? "Yes" : "No");
            lblNoSubsets.Text = "NoSubsets: " + (layer.NoSubsets ? "Yes" : "No");
            tbAbstract.Text = layer.Abstract != null? layer.Abstract.Trim() : null;

            lbStyles.Items.Clear();
            foreach (var style in layer.Style)
            {
                lbStyles.Items.Add(style);
            }

            lbCRS.Items.Clear();
            foreach (var crs in layer.CRS)
            {
                lbCRS.Items.Add(crs);
            }

            lbBoundingBox.Items.Clear();
            foreach (var boundingBox in layer.BoundingBox)
            {
                lbBoundingBox.Items.Add(new BoundingBoxWrapper(boundingBox));
            }
        }

        class BoundingBoxWrapper
        {
            public BoundingBox Box { get; private set; }

            public BoundingBoxWrapper(BoundingBox box)
            {
                Box = box;
            }

            public override string ToString()
            {
                return string.Format("CRS={0} MinX={1} MinY={2} MaxX={3} MaxY={4} ResX={5} ResY={6}",
                    Box.CRS, Box.MinX, Box.MinY, Box.MaxX, Box.MaxY, Box.ResX, Box.ResY);
            }
        }
    }
}
