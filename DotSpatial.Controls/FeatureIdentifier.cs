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
        private Extent _activeRegion;
        private readonly Dictionary<string, string> _featureIDFields;
        private readonly MenuItem _mnuAssignIdField;
        private readonly MenuItem _mnuSelectMenu;
        
        private readonly ContextMenu mnuTreeContext;
        private string _previouslySelectedLayerName;

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
            // The "ID Field" seems more like a display caption.
            _mnuAssignIdField = new MenuItem("Assign ID Field");
            _mnuAssignIdField.Click += _mnuAssignIdField_Click;
            _featureIDFields = new Dictionary<string, string>();
        }

        #endregion

        #region Methods

        private void _mnuAssignIdField_Click(object sender, EventArgs e)
        {
            var fl = treFeatures.SelectedNode.Tag as IFeatureLayer;
            if (fl != null)
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
                if (_featureIDFields.ContainsKey(fl.LegendText) == false)
                {
                    _featureIDFields.Add(fl.LegendText, (string)lstBox.SelectedItem);
                }
                else
                {
                    _featureIDFields[fl.LegendText] = (string)lstBox.SelectedItem;
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

        private void selectMenu_Click(object sender, EventArgs e)
        {
            var feature = treFeatures.SelectedNode.Tag as IFeature;
            var layer = treFeatures.SelectedNode.Parent.Tag as IFeatureLayer;
            if (feature != null && layer != null)
            {
                layer.Select(feature);
            }
        }

        private void treFeatures_MouseUp(object sender, MouseEventArgs e)
        {
            // Create a customized context menu on right click in the tree.
            if (e.Button == MouseButtons.Right)
            {
                var clickedNode = treFeatures.GetNodeAt(e.X, e.Y);
                if (clickedNode != null)
                {
                    var f = clickedNode.Tag as IFeature;
                    if (f != null)
                    {
                        treFeatures.SelectedNode = clickedNode;
                        mnuTreeContext.MenuItems.Clear();
                        mnuTreeContext.MenuItems.Add(_mnuSelectMenu);
                        mnuTreeContext.Show(treFeatures, e.Location);
                    }
                    var fl = clickedNode.Tag as IFeatureLayer;
                    if (fl != null)
                    {
                        treFeatures.SelectedNode = clickedNode;
                        mnuTreeContext.MenuItems.Clear();
                        mnuTreeContext.MenuItems.Add(_mnuAssignIdField);
                        mnuTreeContext.Show(treFeatures, e.Location);
                    }
                }  
            }
        }

        /// <summary>
        /// Clears the items in the tree
        /// </summary>
        public virtual void Clear()
        {
            if (treFeatures.SelectedNode != null)
            {
                var node = treFeatures.SelectedNode;
                _previouslySelectedLayerName = node.Parent != null ? node.Parent.Text : node.Text;
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
        public virtual bool Add(IFeatureLayer layer, Extent bounds)
        {
            // CGX
            try
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
                    if (_featureIDFields.ContainsKey(layer.LegendText))
                    {
                        if (dr != null) name += " - " + dr[_featureIDFields[layer.LegendText]];
                    }
                    var node = nodeLayer.Nodes.Add(name);
                    node.Tag = feature;
                }
                treFeatures.ResumeLayout();
            }
            catch (Exception)
            { }
            return true;
        }

        /// <summary>
        /// Adds a new node to the tree view with the layer name
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="bounds">The bounds.</param>
        internal void Add(IMapRasterLayer layer, Extent bounds)
        {
            treFeatures.SuspendLayout();

            var index = layer.DataSet.ProjToCell(bounds.Center);
            if (index == RcIndex.Empty) return;
            var val = layer.DataSet.Value[index.Row, index.Column];

            var text = String.Format("{0} = {1} ({2},{3})", layer.LegendText, val, index.Column, index.Row);
            var nodeLayer = treFeatures.Nodes.Add(text);
            nodeLayer.Tag = layer;
            nodeLayer.Name = layer.LegendText;

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
           // treFeatures.Focus();
        }

        private void treFeatures_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var f = e.Node.Tag as IFeature;
            if (f == null)
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

        #endregion
    }
}