// *******************************************************************************************************
// Product:  DotSpatial.Symbology.Forms.SelectByAttributes
// Description: Window for selecting features by attributes.

// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// *******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public class SelectByAttributes : Form
    {
        private Button btnApply;
        private Button btnOk;
        private ComboBox cmbLayers;
        private ComboBox cmbMethod;
        private Label lblLayer;
        private Label lblMethod;

        #region Private Variables

        private IFeatureLayer _activeLayer;
        private IFrame _mapFrame;
        private IFeatureLayer[] _layersToSelect;

        private Button btnClose;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private SQLQueryControl sqlQueryControl1;
        private ToolTip ttHelp;

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectByAttributes));
            this.lblLayer = new System.Windows.Forms.Label();
            this.cmbLayers = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.sqlQueryControl1 = new DotSpatial.Symbology.Forms.SQLQueryControl();
            this.SuspendLayout();
            // 
            // lblLayer
            // 
            resources.ApplyResources(this.lblLayer, "lblLayer");
            this.lblLayer.Name = "lblLayer";
            // 
            // cmbLayers
            // 
            resources.ApplyResources(this.cmbLayers, "cmbLayers");
            this.cmbLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLayers.FormattingEnabled = true;
            this.cmbLayers.Name = "cmbLayers";
            this.ttHelp.SetToolTip(this.cmbLayers, resources.GetString("cmbLayers.ToolTip"));
            this.cmbLayers.SelectedIndexChanged += new System.EventHandler(this.cmbLayers_SelectedIndexChanged);
            // 
            // lblMethod
            // 
            resources.ApplyResources(this.lblMethod, "lblMethod");
            this.lblMethod.Name = "lblMethod";
            // 
            // cmbMethod
            // 
            resources.ApplyResources(this.cmbMethod, "cmbMethod");
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
            resources.GetString("cmbMethod.Items"),
            resources.GetString("cmbMethod.Items1"),
            resources.GetString("cmbMethod.Items2"),
            resources.GetString("cmbMethod.Items3")});
            this.cmbMethod.Name = "cmbMethod";
            this.ttHelp.SetToolTip(this.cmbMethod, resources.GetString("cmbMethod.ToolTip"));
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // sqlQueryControl1
            // 
            this.sqlQueryControl1.AttributeSource = null;
            this.sqlQueryControl1.ExpressionText = "";
            resources.ApplyResources(this.sqlQueryControl1, "sqlQueryControl1");
            this.sqlQueryControl1.Name = "sqlQueryControl1";
            this.sqlQueryControl1.Table = null;
            // 
            // SelectByAttributes
            // 
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.sqlQueryControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbMethod);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.lblLayer);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectByAttributes";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of SelectByAttributes
        /// </summary>
        public SelectByAttributes()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Creates a new instance of SelectByAttributes
        /// </summary>
        /// <param name="mapFrame">The MapFrame containing the layers</param>
        public SelectByAttributes(IFrame mapFrame)
        {
            _mapFrame = mapFrame;

            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Creates a new instance of SelectByAttributes
        /// </summary>
        /// <param name="layersToSelect">Layers to select</param>
        public SelectByAttributes(params IFeatureLayer[] layersToSelect)
        {
            if (layersToSelect == null) throw new ArgumentNullException("layersToSelect");

            _layersToSelect = layersToSelect;

            InitializeComponent();
            Configure();
        }

        private void Configure()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(IFeatureLayer));

            IEnumerable<ILayer> layersSource;
            if (_layersToSelect != null)
            {
                layersSource = _layersToSelect;
            }
            else if (_mapFrame != null)
            {
                layersSource = _mapFrame;
            }
            else
            {
                layersSource = Enumerable.Empty<ILayer>();
            }
            
            foreach (var layer in layersSource.OfType<IFeatureLayer>())
            {
                DataRow dr = dt.NewRow();
                dr["Name"] = layer.LegendText;
                dr["Value"] = layer;
                dt.Rows.Add(dr);
            }
            cmbLayers.DataSource = dt;
            cmbLayers.DisplayMember = "Name";
            cmbLayers.ValueMember = "Value";

            cmbMethod.SelectedIndex = 0;

            if (cmbLayers.Items.Count > 0)
            {
                cmbLayers.SelectedIndex = 0;
            }
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the map frame to use for this control
        /// </summary>
        public IFrame MapFrame
        {
            get { return _mapFrame; }
            set
            {
                _layersToSelect = null;
                _mapFrame = value;
                Configure();
            }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        #region Private Functions

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        private void cmbLayers_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = cmbLayers.SelectedValue as DataRowView;
            if (drv != null)
            {
                _activeLayer = drv.Row["Value"] as IFeatureLayer;
            }
            else
            {
                _activeLayer = cmbLayers.SelectedValue as IFeatureLayer;
            }
            if (_activeLayer == null) return;
            if (!_activeLayer.DataSet.AttributesPopulated && _activeLayer.DataSet.NumRows() < 50000)
            {
                _activeLayer.DataSet.FillAttributes();
            }
            if (_activeLayer.EditMode || _activeLayer.DataSet.AttributesPopulated)
            {
                sqlQueryControl1.Table = _activeLayer.DataSet.DataTable;
            }
            else
            {
                sqlQueryControl1.AttributeSource = _activeLayer.DataSet;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            string filter = sqlQueryControl1.ExpressionText;
            if (_activeLayer != null)
            {
                try
                {
                    _activeLayer.SelectByAttribute(filter, GetSelectMode());
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    MessageBox.Show("There was an error attempting to apply this expression.");
                }
            }
        }

        private ModifySelectionMode GetSelectMode()
        {
            switch (cmbMethod.SelectedIndex)
            {
                case 0: return ModifySelectionMode.Replace;
                case 1: return ModifySelectionMode.Append;
                case 2: return ModifySelectionMode.Subtract;
                case 3: return ModifySelectionMode.SelectFrom;
            }
            return ModifySelectionMode.Replace;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            ApplyFilter();
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}