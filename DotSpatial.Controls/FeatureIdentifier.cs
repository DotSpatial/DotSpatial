// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/2/2009 12:28:53 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// FeatureIdentifier
    /// </summary>
    public class FeatureIdentifier : Form
    {
        private Extent _activeRegion;
        private Dictionary<string, string> _featureIDFields;
        private ListBoxDialog _lstBox = new ListBoxDialog();
        private MenuItem _mnuAssignIdField;
        private MenuItem _mnuSelectMenu;
        private string _previouslySelectedLayerName;
        private DataGridView dgvAttributes;
        private ContextMenu mnuTreeContext;
        private SplitContainer splitContainer1;
        private TreeView treFeatures;

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureIdentifier));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treFeatures = new System.Windows.Forms.TreeView();
            this.dgvAttributes = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributes)).BeginInit();
            this.SuspendLayout();
            //
            // splitContainer1
            //
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.Controls.Add(this.treFeatures);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.dgvAttributes);
            //
            // treFeatures
            //
            resources.ApplyResources(this.treFeatures, "treFeatures");
            this.treFeatures.Name = "treFeatures";
            this.treFeatures.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treFeatures_AfterSelect);
            //
            // dgvAttributes
            //
            this.dgvAttributes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvAttributes, "dgvAttributes");
            this.dgvAttributes.Name = "dgvAttributes";
            //
            // FeatureIdentifier
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.Name = "FeatureIdentifier";
            this.TopMost = true;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttributes)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FeatureIdentifier
        /// </summary>
        public FeatureIdentifier()
        {
            InitializeComponent();
            treFeatures.MouseUp += treFeatures_MouseUp;
            mnuTreeContext = new ContextMenu();
            _mnuSelectMenu = new MenuItem("Select Feature");
            _mnuSelectMenu.Click += selectMenu_Click;
            _mnuAssignIdField = new MenuItem("Assign ID Field");
            _mnuAssignIdField.Click += _mnuAssignIdField_Click;
            //mnuTreeContext.MenuItems.Add(_mnuSelectMenu);

            _featureIDFields = new Dictionary<string, string>();
        }

        private void _mnuAssignIdField_Click(object sender, EventArgs e)
        {
            IFeatureLayer fl = treFeatures.SelectedNode.Tag as IFeatureLayer;
            if (fl != null)
            {
                _lstBox = new ListBoxDialog();

                int count = fl.DataSet.DataTable.Columns.Count;
                object[] obj = new object[count];
                for (int i = 0; i < count; i++)
                {
                    obj[i] = fl.DataSet.DataTable.Columns[i].ColumnName;
                }
                _lstBox.Clear();
                _lstBox.Add(obj);
                if (_lstBox.ShowDialog(this) != DialogResult.OK) return;
                if (_featureIDFields.ContainsKey(fl.LegendText) == false)
                {
                    _featureIDFields.Add(fl.LegendText, (string)_lstBox.SelectedItem);
                }
                else
                {
                    _featureIDFields[fl.LegendText] = (string)_lstBox.SelectedItem;
                }
                SuspendLayout();

                List<IFeatureLayer> oldLayers = new List<IFeatureLayer>();
                foreach (TreeNode node in treFeatures.Nodes)
                {
                    oldLayers.Add(node.Tag as IFeatureLayer);
                }

                Clear();
                foreach (IFeatureLayer layer in oldLayers)
                {
                    Add(layer, _activeRegion);
                }
                ReSelect();
                ResumeLayout();
            }
        }

        private void selectMenu_Click(object sender, EventArgs e)
        {
            IFeature feature = treFeatures.SelectedNode.Tag as IFeature;
            IFeatureLayer layer = treFeatures.SelectedNode.Parent.Tag as IFeatureLayer;
            if (feature != null && layer != null)
            {
                layer.Select(feature);
            }
        }

        private void treFeatures_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode clickedNode = treFeatures.GetNodeAt(e.X, e.Y);
                IFeature f = clickedNode.Tag as IFeature;
                if (f != null)
                {
                    treFeatures.SelectedNode = clickedNode;
                    mnuTreeContext.MenuItems.Clear();
                    mnuTreeContext.MenuItems.Add(_mnuSelectMenu);
                    mnuTreeContext.Show(treFeatures, e.Location);
                }
                IFeatureLayer fl = clickedNode.Tag as IFeatureLayer;
                if (fl != null)
                {
                    treFeatures.SelectedNode = clickedNode;
                    mnuTreeContext.MenuItems.Clear();
                    mnuTreeContext.MenuItems.Add(_mnuAssignIdField);
                    mnuTreeContext.Show(treFeatures, e.Location);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the items in the tree
        /// </summary>
        public virtual void Clear()
        {
            if (treFeatures.SelectedNode != null)
            {
                TreeNode node = treFeatures.SelectedNode;
                if (node.Parent != null)
                {
                    _previouslySelectedLayerName = node.Parent.Text;
                }
            }
            treFeatures.SuspendLayout();
            treFeatures.Nodes.Clear();
            treFeatures.ResumeLayout();
        }

        /// <summary>
        /// Adds a new node to the tree view with the layer name
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="bounds"></param>
        public virtual void Add(IFeatureLayer layer, Extent bounds)
        {
            _activeRegion = bounds;
            treFeatures.SuspendLayout();

            TreeNode nodeLayer = treFeatures.Nodes.Add(layer.LegendText);
            nodeLayer.Tag = layer;
            nodeLayer.Name = layer.LegendText;

            List<IFeature> result = layer.DataSet.Select(bounds);
            foreach (IFeature feature in result)
            {
                DataRow dr = null;
                if (!layer.DataSet.AttributesPopulated)
                {
                    int fid;
                    if (feature.ShapeIndex != null)
                        fid = layer.DataSet.ShapeIndices.IndexOf(feature.ShapeIndex);
                    else
                        fid = feature.Fid;

                    DataTable dt = layer.DataSet.GetAttributes(fid, 1);
                    if ((dt != null) && (dt.Rows.Count > 0))
                        dr = layer.DataSet.GetAttributes(fid, 1).Rows[0];

                    feature.DataRow = dr;
                }
                else
                {
                    dr = feature.DataRow;
                }
                string name = feature.Fid.ToString(CultureInfo.InvariantCulture);
                if (_featureIDFields.ContainsKey(layer.LegendText))
                {
                    if (dr != null) name += " - " + dr[_featureIDFields[layer.LegendText]];
                }
                TreeNode node = nodeLayer.Nodes.Add(name);
                node.Tag = feature;
            }
            treFeatures.ResumeLayout();
        }

        /// <summary>
        /// Re-selects the same layer that was being investigated before.
        /// </summary>
        public virtual void ReSelect()
        {
            if (_previouslySelectedLayerName != null)
            {
                TreeNode parent = treFeatures.Nodes[_previouslySelectedLayerName];
                if (parent != null)
                {
                    parent.Expand();
                    TreeNode child = parent.FirstNode;
                    treFeatures.SelectedNode = child;
                }
                else
                {
                    if (treFeatures.Nodes.Count > 0)
                    {
                        treFeatures.Nodes[0].Expand();
                        treFeatures.SelectedNode = treFeatures.Nodes[0].FirstNode;
                    }
                }
            }
            else
            {
                if (treFeatures.Nodes.Count > 0)
                {
                    treFeatures.Nodes[0].Expand();
                    treFeatures.SelectedNode = treFeatures.Nodes[0].FirstNode;
                }
            }
        }

        #endregion

        #region Properties

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

        private void treFeatures_AfterSelect(object sender, TreeViewEventArgs e)
        {
            IFeature f = e.Node.Tag as IFeature;
            if (f == null) return;
            DataTable dt = new DataTable();
            dt.Columns.Add("Field Name");
            dt.Columns.Add("Value");

            if (f.DataRow == null)
            {
                f.ParentFeatureSet.FillAttributes();
            }
            DataColumn[] columns = f.ParentFeatureSet.GetColumns();
            foreach (DataColumn fld in columns)
            {
                DataRow dr = dt.NewRow();
                dr["Field Name"] = fld.ColumnName;
                if (f.DataRow != null) dr["Value"] = f.DataRow[fld.ColumnName].ToString();
                dt.Rows.Add(dr);
            }
            dgvAttributes.DataSource = dt;
        }

        /// <summary>
        /// Overrides the normal closing behavior to prevent accidentally referencing a
        /// disposed form later.  This also allows the form to display slightly faster
        /// after the first time.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}