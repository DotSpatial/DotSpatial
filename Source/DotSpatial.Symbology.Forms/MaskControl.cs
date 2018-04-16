using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace DotSpatial.Symbology.Forms
{
    public partial class MaskControl : UserControl
    {
        //=============================================================================
        #region Properties / Attributes

        IFeatureLayer _ActiveLayer = null;
        List<TreeNode> _l_SelectedItems = null;
        #endregion

        //=============================================================================
        #region Accessor

        public IFeatureLayer ActiveLayer
        {
            set { _ActiveLayer = value; }
        }

        public bool UseMask
        {
            get { return cb_UseMask.Checked; }       
        }

        public string[] MaskedLayers
        {
            get
            {
                if (UseMask)
                {
                    return GetMaskedLayers();
                }
                else
                {
                    return new string[0];
                }
            }
        }

        public double MaskMargin_Top
        {
            get { return doubleBox_TopMargin.IsValid ? doubleBox_TopMargin.Value : 0.0f; }
        }

        public double MaskMargin_Bottom
        {
            get { return doubleBox_BottomMargin.IsValid ? doubleBox_BottomMargin.Value : 0.0f; }
        }

        public double MaskMargin_Left
        {
            get { return doubleBox_LeftMargin.IsValid ? doubleBox_LeftMargin.Value : 0.0f; }
        }

        public double MaskMargin_Right
        {
            get { return doubleBox_RightMargin.IsValid ? doubleBox_RightMargin.Value : 0.0f; }
        }

        #endregion

        //=============================================================================
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public MaskControl()
        {
            InitializeComponent();            
        }

        #endregion

        //=============================================================================
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        public void initControl(ILabelCategory iLabelCat)
        {
            cb_UseMask.Checked = false;
            groupBox_Layers.Enabled = false;
            groupBox_Margins.Enabled = false;
            tv_layers.Nodes.Clear();
            _l_SelectedItems = new List<TreeNode>();

            updateTable();
            if (iLabelCat != null)
            {
                cb_UseMask.Checked = iLabelCat.UseMask;
                doubleBox_TopMargin.Value = iLabelCat.MaskMargin_Top;
                doubleBox_BottomMargin.Value = iLabelCat.MaskMargin_Bottom;
                doubleBox_LeftMargin.Value = iLabelCat.MaskMargin_Left;
                doubleBox_RightMargin.Value = iLabelCat.MaskMargin_Right;
                checkNode(iLabelCat.MaskedLayers);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_UseMask_CheckedChanged(object sender, EventArgs e)
        {
            groupBox_Layers.Enabled = cb_UseMask.Checked;
            groupBox_Margins.Enabled = cb_UseMask.Checked;
        }

        /// <summary>
        /// 
        /// </summary>
        private void updateTable()
        {
            tv_layers.Nodes.Clear();

            TreeNode tn = new TreeNode("Layers");
            if (_ActiveLayer.MapFrame != null)
            {
                foreach (ILayer layer in _ActiveLayer.MapFrame.GetLayers())
                {
                    ConstructTreeNode(layer, tn);
                }
            }

            tv_layers.Nodes.Add(tn);
            tv_layers.ExpandAll();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="tn"></param>
        private void ConstructTreeNode(ILayer layer, TreeNode tn)
        {
            Group gr = layer as Group;
            if (gr != null)
            {
                TreeNode tn_group = new TreeNode(gr.LegendText);
                foreach (ILayer sublayer in gr.GetLayers())
                {
                    ConstructTreeNode(sublayer, tn_group);
                }
                if (tn_group.Nodes.Count > 0)
                {
                    tn.Nodes.Add(tn_group);
                }
            }
            else
            {
                FeatureLayer iFL = layer as FeatureLayer;
                if(iFL != null)
                {
                    if (iFL.GetType().Name == "MapLineLayer")
                    {
                        tn.Nodes.Add(iFL.Name, iFL.Name);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string[] GetMaskedLayers()
        {
            List<string> lMaskedLayers = new List<string>();

            foreach(TreeNode selectedTreeNode in _l_SelectedItems)
            {
                lMaskedLayers.Add(selectedTreeNode.Text);
            }

            return lMaskedLayers.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_layers_AfterCheck(object sender, TreeViewEventArgs e)
        {
            selectChildrenTreeNodes(e.Node.Nodes, e.Node.Checked);
            getSelectedNodes();
        }

        /// <summary>
        /// Select all the tree boxes
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="check"></param>
        private void selectChildrenTreeNodes(TreeNodeCollection nodes, bool check)
        {
            if (check)
            {
                foreach (TreeNode node in nodes)
                {
                    if (!node.Checked)
                        node.Checked = check;
                    selectChildrenTreeNodes(node.Nodes, check);
                }
            }
            else if (!check)
            {
                foreach (TreeNode node in nodes)
                {
                    if (node.Checked)
                        node.Checked = check;
                    selectChildrenTreeNodes(node.Nodes, check);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void getSelectedNodes()
        {
            if (_l_SelectedItems == null)
            {
                _l_SelectedItems = new List<TreeNode>();
            }

            _l_SelectedItems.Clear();
            _l_SelectedItems = GetAllLeafNodes(tv_layers.Nodes[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_self"></param>
        /// <returns></returns>
        private List<TreeNode> GetAllLeafNodes(TreeNode _self)
        {
            List<TreeNode> result = new List<TreeNode>();
            if (_self.Checked && _self.Nodes.Count == 0)
            {
                result.Add(_self);
            }

            foreach (TreeNode child in _self.Nodes)
            {
                result.AddRange(GetAllLeafNodes(child));
            }

            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        private void checkNode(string[] pMaskedLayers)
        {
            foreach (string sLayerName in pMaskedLayers)
            {
                foreach (TreeNode selectedTN in tv_layers.Nodes.Find(sLayerName, true))
                {
                    selectedTN.Checked = true;
                }
            }
            //checkParentIfAllChildrenAreChecked(tv_layers.Nodes[0]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tnCol"></param>
        /// <returns></returns>
        private bool checkParentIfAllChildrenAreChecked(TreeNode treeNode)
        {
            bool bChecked = true;


            if (treeNode.Nodes.Count == 0)
            {
                bChecked = treeNode.Checked;
            }
            else
            {
                foreach (TreeNode tnChild in treeNode.Nodes)
                {
                    bChecked &= checkParentIfAllChildrenAreChecked(tnChild);
                    if (!bChecked)
                    {
                        break;
                    }
                }

                treeNode.Checked = bChecked;
            }

            return bChecked;
        }

        #endregion

    }
}
