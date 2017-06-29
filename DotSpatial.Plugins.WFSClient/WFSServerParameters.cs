using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Controls;
namespace WFSPlugin
{

    public partial class WFSServerParameters : Form
    {

        WFSClient wfsClient;
        public Map map;
        public WFSServerParameters()
        {
            InitializeComponent();
        }

        private void uxGetCapabilities_Click(object sender, EventArgs e)
        {
            uxGroupWPS.Enabled = true;
            uxOpen.Enabled = false;
            uxLayersList.DataSource = null;
            uxAttributesGrid.DataSource = null;
            var serverUrl = uxServer.Text;

             wfsClient = new WFSClient();
            wfsClient.ReadCapabilities(serverUrl);
            uxInfo.Text = "";
            if (wfsClient.wfs.ServiceProvider != null)
            {
                uxInfo.Text += "Provider Name: " + wfsClient.wfs.ServiceProvider.ProviderName + Environment.NewLine;
                uxInfo.Text += "Provider Site: " + wfsClient.wfs.ServiceProvider.ProviderSite + Environment.NewLine;
                uxInfo.Text += "Provider Contact: " + wfsClient.wfs.ServiceProvider.ServiceContact.IndividualName.ToString() + Environment.NewLine;
              
            }
            uxVersion.Text = "Server type: " + wfsClient.wfs.Version;
            DataGridViewRow newRow = new DataGridViewRow();
            DataTable table= new DataTable();
            DataColumn dLayer = new DataColumn("Layer");
            DataColumn dGeom = new DataColumn("Geometry");
            table.Columns.Add(dLayer);
            table.Columns.Add(dGeom);
            foreach(var feature in wfsClient.wfs.FeatureTypeList.FeatureTypes)
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
            uxRequest.Text = wfsClient.uri.AbsoluteUri;
        }

        private void FieldGrid()
        {
            if (wfsClient.fields == null) return;

            

            DataGridViewRow newRow = new DataGridViewRow();
            DataTable table = new DataTable();
            DataColumn dLayer = new DataColumn("Name");
            DataColumn dGeom = new DataColumn("Type");
            table.Columns.Add(dLayer);
            table.Columns.Add(dGeom);
            foreach (var feature in wfsClient.fields.Keys)
            {
                DataRow dr = table.NewRow();
                dr["name"] = feature;

                dr["Type"] = wfsClient.fields[feature];
                table.Rows.Add(dr);
            }

          uxAttributesGrid.DataSource = table;
        
        }

        

        private void WFSServerParameters_Load(object sender, EventArgs e)
        {
            uxTabWfs.SelectedIndex = 1;
            uxListServer.SelectedIndex = 0;
        }

        private void uxLayersList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void uxLayersList_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (uxLayersList.SelectedRows.Count >= 1)
            {
                uxLayer.Text = uxLayersList.SelectedRows[0].Cells["layer"].Value.ToString();
                wfsClient.TypeName = uxLayer.Text;
                var serverUrl = uxServer.Text;
                wfsClient.ReadDescribeFeatureType(serverUrl);
                FieldGrid();
                uxRequest.Text = wfsClient.uri.AbsoluteUri;
                uxOutput.Text = wfsClient.xml;
            }
        }

        private void uxListServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            uxServer.Text = uxListServer.Text;
            uxGroupWPS.Enabled = false;
        }

        string geographicField;
        private void uxAttributesGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (uxAttributesGrid.SelectedRows.Count >= 1)
            {

                geographicField = uxAttributesGrid.SelectedRows[0].Cells["Name"].Value.ToString();
                uxGeographicField.Text = "Geographic field: " + geographicField;
                wfsClient.Geometry = WFSClient.IsGeographicFieldValid(uxAttributesGrid.SelectedRows[0].Cells["Type"].Value.ToString()) ? geographicField : null;
                if (wfsClient.Geometry == null)
                {
                    MessageBox.Show("Please select a geographic field valid");
                    uxOpen.Enabled = false;
                    return;
                }
                
                wfsClient.TypeName = uxLayer.Text;
                uxOpen.Enabled = true;
               
            }
        }

        private void uxAttributesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void uxOpen_Click(object sender, EventArgs e)
        {
            //var serverUrl = "http://ogi.state.ok.us/geoserver/wfs";
            //wfsClient.TypeName = "quad100_centroids";

            var serverUrl = uxServer.Text;
             wfsClient.TypeName = uxLayer.Text;

            wfsClient.ReadFeature(serverUrl);
            uxRequest.Text = wfsClient.uri.AbsoluteUri;
            uxOutput.Text = wfsClient.xml;
            if (map != null && wfsClient.fea!=null)
            {
                var layer = map.Layers.Add(wfsClient.fea);
                layer.LegendText = wfsClient.TypeName;
            }

            
        }
    }
}
