// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Data;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace DotSpatial.Plugins.WFSClient
{
    /// <summary>
    /// WfsServerParameters.
    /// </summary>
    public partial class WfsServerParameters : Form
    {
        #region Fields
        private string _geographicField;
        private Classes.WfsClient _wfsClient;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WfsServerParameters"/> class.
        /// </summary>
        public WfsServerParameters()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        public IMap Map { get; set; }

        #endregion

        #region Methods

        private void FieldGrid()
        {
            if (_wfsClient.Fields == null) return;

            DataTable table = new();
            DataColumn dLayer = new("Name");
            DataColumn dGeom = new("Type");
            table.Columns.Add(dLayer);
            table.Columns.Add(dGeom);
            foreach (var feature in _wfsClient.Fields.Keys)
            {
                DataRow dr = table.NewRow();
                dr["name"] = feature;

                dr["Type"] = _wfsClient.Fields[feature];
                table.Rows.Add(dr);
            }

            uxAttributesGrid.DataSource = table;
        }

        private void UxAttributesGridCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void UxAttributesGridRowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (uxAttributesGrid.SelectedRows.Count >= 1)
            {
                _geographicField = uxAttributesGrid.SelectedRows[0].Cells["Name"].Value.ToString();
                uxGeographicField.Text = "Geographic field: " + _geographicField;
                _wfsClient.Geometry = Classes.WfsClient.IsGeographicFieldValid(uxAttributesGrid.SelectedRows[0].Cells["Type"].Value.ToString()) ? _geographicField : null;
                if (_wfsClient.Geometry == null)
                {
                    MessageBox.Show("Please select a geographic field valid");
                    uxOpen.Enabled = false;
                    return;
                }

                _wfsClient.TypeName = uxLayer.Text;
                uxOpen.Enabled = true;
            }
        }

        private void UxGetCapabilitiesClick(object sender, EventArgs e)
        {
            uxGroupWPS.Enabled = true;
            uxOpen.Enabled = false;
            uxLayersList.DataSource = null;
            uxAttributesGrid.DataSource = null;
            var serverUrl = uxServer.Text;

            _wfsClient = new Classes.WfsClient();
            _wfsClient.ReadCapabilities(serverUrl);
            uxInfo.Text = string.Empty;
            if (_wfsClient.Wfs.ServiceProvider != null)
            {
                uxInfo.Text += "Provider Name: " + _wfsClient.Wfs.ServiceProvider.ProviderName + Environment.NewLine;
                uxInfo.Text += "Provider Site: " + _wfsClient.Wfs.ServiceProvider.ProviderSite + Environment.NewLine;
                uxInfo.Text += "Provider Contact: " + _wfsClient.Wfs.ServiceProvider.ServiceContact.IndividualName + Environment.NewLine;
            }

            uxVersion.Text = "Server type: " + _wfsClient.Wfs.Version;
            DataTable table = new();
            DataColumn dLayer = new("Layer");
            DataColumn dGeom = new("Geometry");
            table.Columns.Add(dLayer);
            table.Columns.Add(dGeom);
            foreach (var feature in _wfsClient.Wfs.FeatureTypeList.FeatureTypes)
            {
                DataRow dr = table.NewRow();
                dr["Layer"] = feature.Name.Name;

                foreach (var f in feature.WGS84BoundingBoxes)
                {
                    dr["Geometry"] = f.LowerCorner + " " + f.UpperCorner;
                }

                dr["Geometry"] = feature.WGS84BoundingBoxes.Count;
                table.Rows.Add(dr);
            }

            uxLayersList.DataSource = table;
            uxGroupWPS.Enabled = true;
            uxRequest.Text = _wfsClient.Uri.AbsoluteUri;
        }

        private void UxLayersListCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void UxLayersListRowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (uxLayersList.SelectedRows.Count >= 1)
            {
                uxLayer.Text = uxLayersList.SelectedRows[0].Cells["layer"].Value.ToString();
                _wfsClient.TypeName = uxLayer.Text;
                var serverUrl = uxServer.Text;
                _wfsClient.ReadDescribeFeatureType(serverUrl);
                FieldGrid();
                uxRequest.Text = _wfsClient.Uri.AbsoluteUri;
                uxOutput.Text = _wfsClient.Xml;
            }
        }

        private void UxListServerSelectedIndexChanged(object sender, EventArgs e)
        {
            uxServer.Text = uxListServer.Text;
            uxGroupWPS.Enabled = false;
        }

        private void UxOpenClick(object sender, EventArgs e)
        {
            // var serverUrl = "http://ogi.state.ok.us/geoserver/wfs";
            // wfsClient.TypeName = "quad100_centroids";
            var serverUrl = uxServer.Text;
            _wfsClient.TypeName = uxLayer.Text;

            _wfsClient.ReadFeature(serverUrl);
            uxRequest.Text = _wfsClient.Uri.AbsoluteUri;
            uxOutput.Text = _wfsClient.Xml;
            if (Map != null && _wfsClient.Fea != null)
            {
                var layer = Map.Layers.Add(_wfsClient.Fea);
                layer.LegendText = _wfsClient.TypeName;
            }
        }

        private void WfsServerParametersLoad(object sender, EventArgs e)
        {
            uxTabWfs.SelectedIndex = 1;
            uxListServer.SelectedIndex = 0;
        }

        #endregion
    }
}