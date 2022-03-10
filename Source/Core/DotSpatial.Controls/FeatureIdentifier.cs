// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Feature Identifier form used to display output from MapFunctionIdentify.
    /// </summary>
    public partial class FeatureIdentifier : Form
    {
        #region Fields

        private readonly Dictionary<string, string> _featureIdFields;
        private readonly ToolStripMenuItem _mnuAssignIdField;
        private readonly ToolStripMenuItem _mnuSelectMenu;

        private readonly ContextMenuStrip _mnuTreeContext;
        private Extent _activeRegion;
        private string _previouslySelectedLayerName;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureIdentifier"/> class.
        /// </summary>
        public FeatureIdentifier()
        {
            InitializeComponent();
            treFeatures.MouseUp += TreFeaturesMouseUp;
            _mnuTreeContext = new ContextMenuStrip();
            _mnuSelectMenu = new ToolStripMenuItem("Select Feature");
            _mnuSelectMenu.Click += SelectMenuClick;

            // The "ID Field" seems more like a display caption.
            _mnuAssignIdField = new ToolStripMenuItem("Assign ID Field");
            _mnuAssignIdField.Click += MnuAssignIdFieldClick;
            _featureIdFields = new Dictionary<string, string>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new node to the tree view with the layer name.
        /// </summary>
        /// <param name="layer">Layer that gets added.</param>
        /// <param name="bounds">Region the features get selected from.</param>
        /// <returns>True, if the node was added to the tree.</returns>
        public virtual bool Add(IFeatureLayer layer, Extent bounds)
        {
            var result = ((FeatureSet)layer.DataSet).Select(bounds);
            if (result.Count == 0)
            {
                return false;
            }

            _activeRegion = bounds;
            treFeatures.SuspendLayout();

            var nodeLayer = treFeatures.Nodes.Add(layer.LegendText);
            nodeLayer.Tag = layer;
            nodeLayer.Name = layer.LegendText;

            foreach (var feature in result)
            {
                var dr = feature.DataRow;
                var name = feature.Fid.ToString(CultureInfo.InvariantCulture);
                if (_featureIdFields.ContainsKey(layer.LegendText))
                {
                    if (dr != null) name += " - " + dr[_featureIdFields[layer.LegendText]];
                }

                var node = nodeLayer.Nodes.Add(name);
                node.Tag = feature;
            }

            treFeatures.ResumeLayout();
            return true;
        }

        /// <summary>
        /// Clears the items in the tree.
        /// </summary>
        public virtual void Clear()
        {
            if (treFeatures.SelectedNode != null)
            {
                var node = treFeatures.SelectedNode;
                _previouslySelectedLayerName = node.Parent?.Text ?? node.Text;
            }

            treFeatures.SuspendLayout();
            treFeatures.Nodes.Clear();
            treFeatures.ResumeLayout();
        }

        /// <summary>
        /// Re-selects the same layer that was being investigated before.
        /// </summary>
        public virtual void ReSelect()
        {
            if (_previouslySelectedLayerName != null)
            {
                var parent = treFeatures.Nodes[_previouslySelectedLayerName];
                if (parent != null)
                {
                    if (parent.FirstNode == null)
                    {
                        treFeatures.SelectedNode = parent;
                    }
                    else
                    {
                        parent.Expand();
                        var child = parent.FirstNode;
                        treFeatures.SelectedNode = child;
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
            else
            {
                if (treFeatures.Nodes.Count > 0)
                {
                    treFeatures.Nodes[0].Expand();
                    treFeatures.SelectedNode = treFeatures.Nodes[0].FirstNode;
                }
            }

            treFeatures.ExpandAll();
            treFeatures.HideSelection = false;
        }

        /// <summary>
        /// Adds a new node to the tree view with the layer name.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="bounds">The bounds.</param>
        internal void Add(IMapRasterLayer layer, Extent bounds)
        {
            treFeatures.SuspendLayout();

            var index = layer.DataSet.ProjToCell(bounds.Center);
            if (index == RcIndex.Empty) return;
            var val = layer.DataSet.Value[index.Row, index.Column];

            var text = string.Format("{0} = {1} ({2},{3})", layer.LegendText, val, index.Column, index.Row);
            var nodeLayer = treFeatures.Nodes.Add(text);
            nodeLayer.Tag = layer;
            nodeLayer.Name = layer.LegendText;

            treFeatures.ResumeLayout();
        }

        private void MnuAssignIdFieldClick(object sender, EventArgs e)
        {
            if (treFeatures.SelectedNode.Tag is IFeatureLayer fl)
            {
                var lstBox = new ListBoxDialog();

                var count = fl.DataSet.DataTable.Columns.Count;
                var obj = new object[count];
                for (var i = 0; i < count; i++)
                {
                    obj[i] = fl.DataSet.DataTable.Columns[i].ColumnName;
                }

                lstBox.Clear();
                lstBox.Add(obj);
                if (lstBox.ShowDialog(this) != DialogResult.OK) return;
                if (_featureIdFields.ContainsKey(fl.LegendText) == false)
                {
                    _featureIdFields.Add(fl.LegendText, (string)lstBox.SelectedItem);
                }
                else
                {
                    _featureIdFields[fl.LegendText] = (string)lstBox.SelectedItem;
                }

                SuspendLayout();

                var oldLayers = new List<IFeatureLayer>();
                foreach (TreeNode node in treFeatures.Nodes)
                {
                    oldLayers.Add(node.Tag as IFeatureLayer);
                }

                Clear();
                foreach (var layer in oldLayers)
                {
                    Add(layer, _activeRegion);
                }

                ReSelect();
                ResumeLayout();
            }
        }

        private void SelectMenuClick(object sender, EventArgs e)
        {
            var layer = treFeatures.SelectedNode.Parent.Tag as IFeatureLayer;
            if (treFeatures.SelectedNode.Tag is IFeature feature)
            {
                layer?.Select(feature);
            }
        }

        private void TreFeaturesAfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is not IFeature f)
            {
                dgvAttributes.DataSource = null;
                return;
            }

            var dt = new DataTable();
            dt.Columns.Add("Field Name");
            dt.Columns.Add("Value");

            if (f.DataRow == null)
            {
                f.ParentFeatureSet.FillAttributes();
            }

            var columns = f.ParentFeatureSet.GetColumns();
            foreach (var fld in columns)
            {
                var dr = dt.NewRow();
                dr["Field Name"] = fld.ColumnName;
                if (f.DataRow != null) dr["Value"] = f.DataRow[fld.ColumnName].ToString();
                dt.Rows.Add(dr);
            }

            dgvAttributes.DataSource = dt;
        }

        private void TreFeaturesMouseUp(object sender, MouseEventArgs e)
        {
            // Create a customized context menu on right click in the tree.
            if (e.Button == MouseButtons.Right)
            {
                var clickedNode = treFeatures.GetNodeAt(e.X, e.Y);

                if (clickedNode != null)
                {
                    if (clickedNode.Tag is IFeature)
                    {
                        treFeatures.SelectedNode = clickedNode;
                        _mnuTreeContext.Items.Clear();
                        _mnuTreeContext.Items.Add(_mnuSelectMenu);
                        _mnuTreeContext.Show(treFeatures, e.Location);
                    }

                    if (clickedNode.Tag is IFeatureLayer)
                    {
                        treFeatures.SelectedNode = clickedNode;
                        _mnuTreeContext.Items.Clear();
                        _mnuTreeContext.Items.Add(_mnuAssignIdField);
                        _mnuTreeContext.Show(treFeatures, e.Location);
                    }
                }
            }
        }

        #endregion
    }
}