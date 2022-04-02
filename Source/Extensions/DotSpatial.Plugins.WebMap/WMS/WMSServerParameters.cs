// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using BruTile.Wms;
using DotSpatial.Plugins.WebMap;
using DotSpatial.Projections;
using Exception = System.Exception;

namespace DotSpatial.Plugins.WebMap.WMS
{
    /// <summary>
    /// WMS server parameters.
    /// </summary>
    public partial class WmsServerParameters : Form
    {
        #region Fields

        private WmsCapabilities _wmsCapabilities;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WmsServerParameters"/> class.
        /// </summary>
        /// <param name="data">The WmsInfo needed to initialize this object.</param>
        public WmsServerParameters(WmsInfo data)
        {
            InitializeComponent();

            WmsInfo = data;
            if (WmsInfo == null) return;

            _wmsCapabilities = WmsInfo.WmsCapabilities;
            tbServerUrl.Text = WmsInfo.ServerUrl;
            if (WmsInfo.Credentials != null)
            {
                tbLogin.Text = WmsInfo.Credentials.UserName;
                tbPassword.Text = WmsInfo.Credentials.Password;
            }

            ShowServerDetails(_wmsCapabilities);
            InitLayers(_wmsCapabilities);

            // Select layer
            tvLayers.SelectedNode = FindNodeByLayer(tvLayers.Nodes, WmsInfo.Layer);
            tvLayers.SelectedNode?.EnsureVisible();
            tvLayers.Select();
            tvLayers.Focus();

            // Select CRS
            lbCRS.SelectedItem = WmsInfo.Crs;

            // Select Style
            for (int i = 0; i < lbStyles.Items.Count; i++)
            {
                var style = (StyleWrapper)lbStyles.Items[i];
                if (style.Style.Name == WmsInfo.Style)
                {
                    lbStyles.SelectedIndex = i;
                    break;
                }
            }

            // Show custom parameters
            if (WmsInfo.CustomParameters != null)
            {
                tbCustomParameters.Text = string.Join(Environment.NewLine, WmsInfo.CustomParameters.Select(d => $"{d.Key}={d.Value}"));
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the WmsInfo.
        /// </summary>
        public WmsInfo WmsInfo { get; private set; }

        #endregion

        #region Methods

        private static void FillTree(TreeNodeCollection collection, Layer root)
        {
            var node = new TreeNode(root.Title)
                           {
                               Tag = root
                           };
            collection.Add(node);
            foreach (var childLayer in root.ChildLayers)
            {
                FillTree(node.Nodes, childLayer);
            }
        }

        private static TreeNode FindNodeByLayer(IEnumerable nodes, Layer layer)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag == layer) return node;
                var ch = FindNodeByLayer(node.Nodes, layer);
                if (ch != null) return ch;
            }

            return null;
        }

        private string PrepareUri(string uri)
        {
            if (!uri.Contains('?'))
            {
                uri += "?";
            }
            else if (!uri.EndsWith("?") && !uri.EndsWith("&"))
            {
                uri += "&";
            }

            return uri;
        }

        private void BtnGetCapabilitiesClick(object sender, EventArgs e)
        {
            var serverUrl = tbServerUrl.Text;
            if (string.IsNullOrWhiteSpace(serverUrl)) return;

            serverUrl = PrepareUri(serverUrl);

            var noCapabilities = serverUrl.IndexOf("Request=GetCapabilities", StringComparison.OrdinalIgnoreCase) < 0;
            var noService = serverUrl.IndexOf("SERVICE=WMS", StringComparison.OrdinalIgnoreCase) < 0;

            if (noCapabilities)
            {
                serverUrl += "Request=GetCapabilities&";
            }

            if (noService)
            {
                serverUrl += "SERVICE=WMS&";
            }

            WmsCapabilities capabilities;
            try
            {
                var myRequest = WebRequest.Create(serverUrl);
                myRequest.Credentials = GetUserCredentials();
                using var myResponse = myRequest.GetResponse();
                using var stream = myResponse.GetResponseStream();
                capabilities = new WmsCapabilities(stream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Resources.UnableToReadCapabilities, ex.Message));
                return;
            }

            WmsInfo = null;
            _wmsCapabilities = capabilities;

            ShowServerDetails(capabilities);
            InitLayers(capabilities);
        }

        private void BtnOkClick(object sender, EventArgs e)
        {
            if (_wmsCapabilities == null)
            {
                MessageBox.Show(Resources.SelectServerToView, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tvLayers.SelectedNode == null)
            {
                MessageBox.Show(Resources.SelectLayerToView, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (lbCRS.SelectedItem == null)
            {
                MessageBox.Show(Resources.SelectCrsToView, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ProjectionInfo projectionInfo;
            try
            {
                var crs = ((string)lbCRS.SelectedItem).ToUpper();
                if (string.Equals(crs, "CRS:84"))
                {
                    crs = "EPSG:4326";
                }

                var epsgCode = Convert.ToInt32(crs.Replace("EPSG:", string.Empty));
                projectionInfo = epsgCode switch
                {
                    3857 => KnownCoordinateSystems.Projected.World.WebMercator,
                    4326 => KnownCoordinateSystems.Geographic.World.WGS1984,
                    _ => ProjectionInfo.FromEpsgCode(epsgCode),
                };
            }
            catch (Exception)
            {
                MessageBox.Show(Resources.UnsupportedCrs, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Parse custom parameters
            var cs = string.IsNullOrWhiteSpace(tbCustomParameters.Text) ? new Dictionary<string, string>() : tbCustomParameters.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Select(d => d.Split('=')).ToDictionary(d => d[0], d => d[1]);

            WmsInfo = new WmsInfo(tbServerUrl.Text, _wmsCapabilities, (Layer)tvLayers.SelectedNode.Tag, cs, (string)lbCRS.SelectedItem, projectionInfo, ((StyleWrapper)lbStyles.SelectedItem)?.Style.Name, GetUserCredentials());
            DialogResult = DialogResult.OK;
        }

        private NetworkCredential GetUserCredentials()
        {
            if (string.IsNullOrEmpty(tbLogin.Text) && string.IsNullOrEmpty(tbPassword.Text))
                return null;
            return new NetworkCredential(tbLogin.Text, tbPassword.Text);
        }

        private void InitLayers(WmsCapabilities capabilities)
        {
            tvLayers.Nodes.Clear();
            FillTree(tvLayers.Nodes, capabilities.Capability.Layer);
            tvLayers.ExpandAll();
        }

        private void ShowServerDetails(WmsCapabilities capabilities)
        {
            tbServerTitle.Text = capabilities.Service.Title;
            tbServerAbstract.Text = capabilities.Service.Abstract;
            tbServerOnlineResource.Text = capabilities.Service.OnlineResource.Href;
            tbServerAccessConstraints.Text = capabilities.Service.AccessConstraints;
        }

        private void TvLayersAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!e.Node.IsSelected) return;
            var layer = (Layer)e.Node.Tag;

            tbTitle.Text = layer.Title;
            tbName.Text = layer.Name;
            lblQuerable.Text = Resources.Queryable + (layer.Queryable ? "Yes" : "No");
            lblOpaque.Text = Resources.Opaque + (layer.Opaque ? "Yes" : "No");
            lblNoSubsets.Text = Resources.NoSubsets + (layer.NoSubsets ? "Yes" : "No");
            lblCascaded.Text = Resources.Cascaded + layer.Cascaded;
            tbAbstract.Text = layer.Abstract?.Trim();
            lblFixedHeight.Text = Resources.FixedHeight + layer.FixedHeight;
            lblFixedWidth.Text = Resources.FixedWidth + layer.FixedWidth;

            var node = e.Node;
            var styles = new List<Style>();
            var crss = new List<string>();
            while (node != null)
            {
                var lr = (Layer)node.Tag;
                foreach (var style in lr.Style)
                {
                    if (styles.All(d => d.Name != style.Name))
                    {
                        styles.Add(style);
                    }
                }

                foreach (var list in new[] { lr.CRS, lr.SRS })
                {
                    foreach (var crs in list)
                    {
                        // Some services returns several CRS in one field, split them
                        foreach (var cr in crs.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            if (!crss.Contains(cr)) crss.Add(cr);
                        }
                    }
                }

                node = node.Parent;
            }

            lbStyles.DataSource = styles.Select(d => new StyleWrapper(d)).ToList();
            lbCRS.DataSource = crss;
        }

        #endregion

        #region Classes

        private class StyleWrapper
        {
            #region  Constructors

            public StyleWrapper(Style style)
            {
                Style = style;
            }

            #endregion

            #region Properties

            public Style Style { get; }

            #endregion

            #region Methods

            public override string ToString()
            {
                return string.IsNullOrEmpty(Style.Title) ? Style.Name : Style.Title;
            }

            #endregion
        }

        #endregion
    }
}