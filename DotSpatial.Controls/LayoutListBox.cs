// ********************************************************************************************************
// Product Name: DotSpatial.Layout.LayoutListBox
// Description:  Shows a list of all the items in the layout control
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll Version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/28/2009 4:23:05 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is designed to automatically have add, subtract, up and down arrows for working with a simple collection of items.
    /// </summary>
    //This control will no longer be visible
    [ToolboxItem(false)]
    public class LayoutListBox : UserControl
    {
        #region ---------------- Class Variables

        //Internal Variables
        private Button _btnDown;
        private Panel _btnPanel;
        private Button _btnRemove;
        private Button _btnUp;
        private LayoutControl _layoutControl;
        private ListBox _lbxItems;
        private Panel _listPanel;
        private bool _suppressSelectionChange;
        // CGX
        private IContainer components;
        private ImageList IL;
        private CodersLab.Windows.Controls.TreeView TV_Items;
        private bool _bSuspendTVRefresh = false;
        // CGX END

        #endregion

        #region ---------------- Properties

        // CGX
        public void StopRefresh()
        {
            _bSuspendTVRefresh = true;
        }

        public void StartRefresh()
        {
            _bSuspendTVRefresh = false;
        }
        // CGX END

        /// <summary>
        /// Gets or sets the layoutControl
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set
            {
                if (value == null) return;
                _layoutControl = value;
                _layoutControl.SelectionChanged += _layoutControl_SelectionChanged;
                _layoutControl.ElementsChanged += _layoutControl_ElementsChanged;
                RefreshList();
            }
        }

        #endregion

        #region ---------------- Constructors

        /// <summary>
        /// Creates a new instance of the Collection Control
        /// </summary>
        public LayoutListBox()
        {
            InitializeComponent();
            _suppressSelectionChange = false;
            _lbxItems.DrawItem += lbxItems_DrawItem;
        }

        #endregion

        #region ---------------- Drawing Code

        private void lbxItems_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) return;
            Rectangle outer = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            Brush textBrush;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(SystemBrushes.Highlight, outer);
                textBrush = Brushes.White;
            }
            else
            {
                textBrush = Brushes.Black;
                e.Graphics.FillRectangle(Brushes.White, outer);
            }
            Rectangle thumbRect = new Rectangle(outer.X + 3, outer.Y + 3, 32, 32);
            e.Graphics.FillRectangle(Brushes.White, thumbRect);

            LayoutElement element = _lbxItems.Items[e.Index] as LayoutElement;
            if (element != null)
            {
                e.Graphics.DrawImage(element.ThumbNail, thumbRect);
            }
            thumbRect.X--;
            thumbRect.Y--;
            thumbRect.Width++;
            thumbRect.Height++;
            e.Graphics.DrawRectangle(Pens.Black, thumbRect);
            Rectangle textRectangle = new Rectangle(outer.X + 40, outer.Y, outer.Width - 40, outer.Height);
            using (StringFormat drawFormat = new StringFormat())
            {
                drawFormat.Alignment = StringAlignment.Near;
                drawFormat.FormatFlags = StringFormatFlags.NoWrap;
                drawFormat.LineAlignment = StringAlignment.Center;
                drawFormat.Trimming = StringTrimming.EllipsisCharacter;
                if (element != null)
                {
                    e.Graphics.DrawString(element.Name, Font, textBrush, textRectangle, drawFormat);
                }
            }
        }

        #endregion

        #region ---------------- Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(LayoutListBox));
            this._lbxItems = new ListBox();
            this._btnPanel = new Panel();
            this._btnDown = new Button();
            this._btnUp = new Button();
            this._btnRemove = new Button();
            this._listPanel = new Panel();
            this.TV_Items = new CodersLab.Windows.Controls.TreeView(); // CGX
            this.IL = new System.Windows.Forms.ImageList(this.components); // CGX
            this._btnPanel.SuspendLayout();
            this._listPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // _lbxItems
            //
            resources.ApplyResources(this._lbxItems, "_lbxItems");
            this._lbxItems.DrawMode = DrawMode.OwnerDrawFixed;
            this._lbxItems.FormattingEnabled = true;
            this._lbxItems.Name = "_lbxItems";
            this._lbxItems.SelectionMode = SelectionMode.MultiExtended;
            this._lbxItems.SelectedIndexChanged += lbxItems_SelectedIndexChanged;
            //
            // _btnPanel
            //
            resources.ApplyResources(this._btnPanel, "_btnPanel");
            this._btnPanel.Controls.Add(this._btnDown);
            this._btnPanel.Controls.Add(this._btnUp);
            this._btnPanel.Controls.Add(this._btnRemove);
            this._btnPanel.Name = "_btnPanel";
            //
            // _btnDown
            //
            resources.ApplyResources(this._btnDown, "_btnDown");
            this._btnDown.Image = Images.down;
            this._btnDown.Name = "_btnDown";
            this._btnDown.UseVisualStyleBackColor = true;
            this._btnDown.Click += btnDown_Click;
            //
            // _btnUp
            //
            resources.ApplyResources(this._btnUp, "_btnUp");
            this._btnUp.Image = Images.up;
            this._btnUp.Name = "_btnUp";
            this._btnUp.UseVisualStyleBackColor = true;
            this._btnUp.Click += btnUp_Click;
            //
            // _btnRemove
            //
            resources.ApplyResources(this._btnRemove, "_btnRemove");
            this._btnRemove.Image = Images.mnuLayerClear;
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += btnRemove_Click;
            //
            // _listPanel
            //
            resources.ApplyResources(this._listPanel, "_listPanel");
            this._listPanel.BackColor = Color.White;
            //this._listPanel.Controls.Add(this._lbxItems);
            this._listPanel.Controls.Add(this.TV_Items); // CGX
            this._listPanel.Name = "_listPanel";

            // CGX
            // 
            // TV_Items
            // 
            resources.ApplyResources(this.TV_Items, "TV_Items");
            this.TV_Items.FullRowSelect = true;
            this.TV_Items.HideSelection = false;
            this.TV_Items.ImageList = this.IL;
            this.TV_Items.LabelEdit = true;
            this.TV_Items.Name = "TV_Items";
            this.TV_Items.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.TV_Items.SelectionMode = CodersLab.Windows.Controls.TreeViewSelectionMode.MultiSelect;
            this.TV_Items.SelectionsChanged += new System.EventHandler(this.TV_Items_SelectionsChanged);
            this.TV_Items.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.TV_Items_AfterLabelEdit);
            this.TV_Items.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TV_Items_AfterSelect);
            this.TV_Items.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TV_Items_NodeMouseClick);
            this.TV_Items.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TV_Items_KeyUp);
            // 
            // IL
            // 
            this.IL.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IL.ImageStream")));
            this.IL.TransparentColor = System.Drawing.Color.Transparent;
            this.IL.Images.SetKeyName(0, "16Objects.png");
            this.IL.Images.SetKeyName(1, "FolderOpen.png");
            this.IL.Images.SetKeyName(2, "16Circle.png");
            this.IL.Images.SetKeyName(3, "16Line.png");
            this.IL.Images.SetKeyName(4, "16Rectangle.png");
            this.IL.Images.SetKeyName(5, "16Image.png");
            this.IL.Images.SetKeyName(6, "16Text.png");
            this.IL.Images.SetKeyName(7, "map.png");
            this.IL.Images.SetKeyName(8, "Legend.png");
            this.IL.Images.SetKeyName(9, "ScaleBar.png");
            this.IL.Images.SetKeyName(10, "16Compas.png");
            this.IL.Images.SetKeyName(11, "16DynamicText.png");
            this.IL.Images.SetKeyName(12, "16Graticule.png");
            // CGX END

            //
            // LayoutListBox
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._listPanel);
            this.Controls.Add(this._btnPanel);
            this.Name = "LayoutListBox";
            this._btnPanel.ResumeLayout(false);
            this._listPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        #region ---------------- Public Methods

        // CGX

        /// <summary>
        /// 
        /// </summary>
        private void AddTreeNodeToTreeView(LayoutElement le)
        {
            try
            {
                int iIndex = _layoutControl.LayoutElements.IndexOf(le);

                TreeNode TN = new TreeNode(le.Name);
                TN.Name = le.Name;
                TN.Tag = le;
                TN.ImageIndex = GetImageFromType(le);

                string sGroupName = le.Group;
                if (sGroupName != String.Empty)
                {
                    TreeNode TN_Group = new TreeNode(sGroupName);
                    TN_Group.Name = sGroupName;
                    TN_Group.ImageIndex = 1;
                    if (TV_Items.Nodes.Find(sGroupName, false).Length == 0)
                        TV_Items.Nodes.Add(TN_Group);

                    TreeNode[] lTN = TV_Items.Nodes.Find(sGroupName, false);
                    int iIndexGroup = TV_Items.Nodes.IndexOf(lTN[0]);
                    lTN[0].Nodes.Add(TN);
                }
                else
                    TV_Items.Nodes.Add(TN);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// 
        /// </summary>
        private int GetImageFromType(LayoutElement le)
        {
            //CGX Image par default
            int i = 0;

            try
            {
                string sType = le.GetType().ToString();
                if (sType.Contains("CircleElement")) i = 2;
                if (sType.Contains("LineLayout")) i = 3;
                if (sType.Contains("LayoutRectangle")) i = 4;
                if (sType.Contains("LayoutBitmap")) i = 5;
                if (sType.Contains("LayoutText")) i = 6;
                if (sType.Contains("LayoutMap")) i = 7;
                if (sType.Contains("LayoutLegend")) i = 8;
                if (sType.Contains("LayoutScaleBar")) i = 9;
                if (sType.Contains("LayoutNorthArrow")) i = 10;
                if (sType.Contains("DynamicText")) i = 11;
                if (sType.Contains("Graticule")) i = 12;

                if (i == -1)
                    System.Diagnostics.Debug.WriteLine("MISSING IMAGE --------------------> " + sType);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

            return i;
        }

        /// <summary>
        /// Refreshes the items in the list to accuratly reflect the current collection
        /// </summary>
        public void RefreshListSelection()
        {
            // Recuperation de la liste
            List<LayoutElement> lSelected = new List<LayoutElement>();
            foreach (LayoutElement le in _layoutControl.SelectedLayoutElements)
                lSelected.Add(le);

            //Updates the list of elements
            TV_Items.SuspendLayout();

            TV_Items.SelectedNodes.Clear();
            foreach (LayoutElement le in lSelected)
            {
                TreeNode[] lTN = TV_Items.Nodes.Find(le.Name, true);
                if (lTN.Length > 0)
                    foreach (TreeNode TN in lTN)
                        TV_Items.SelectedNodes.Add(TN);
            }

            // Selectionne les groupes dont TOUS les enfants sont sélectionés
            foreach (TreeNode TN in TV_Items.Nodes)
            {
                // Cas d'un groupe
                if (TN.Tag == null)
                {
                    bool bSelected = true;
                    foreach (TreeNode TN_Child in TN.Nodes)
                    {
                        if (!TV_Items.SelectedNodes.Contains(TN_Child))
                        {
                            bSelected = false;
                            break;
                        }
                    }

                    if (bSelected)
                        TV_Items.SelectedNodes.Add(TN);
                }
            }

            TV_Items.ResumeLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        private void RemoveTreeNodeFromTreeView(LayoutElement le)
        {
            try
            {
                TreeNode[] lTN = TV_Items.Nodes.Find(le.Name, true);
                if (lTN.Length > 0)
                    TV_Items.Nodes.Remove(lTN[0]);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        // CGX END

        /// <summary>
        /// Refreshes the items in the list to accuratly reflect the current collection
        /// </summary>
        public void RefreshList()
        {
            // CGX
            try
            {
                if (!_bSuspendTVRefresh)
                {
                    TV_Items.SuspendLayout();
                    TV_Items.Nodes.Clear();
                    foreach (LayoutElement le in _layoutControl.LayoutElements.ToArray())
                        AddTreeNodeToTreeView(le);
                    TV_Items.ResumeLayout();
                    RefreshListSelection();
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
            // CGX END
            /*
            _lbxItems.SuspendLayout();

            //We clear the old list
            _lbxItems.Items.Clear();

            //Updates the list of elements
            foreach (LayoutElement le in _layoutControl.LayoutElements.ToArray())
            {
                _lbxItems.Items.Add(le);
                le.ThumbnailChanged += le_ThumbnailChanged;
            }

            //Updates the selection list
            foreach (LayoutElement le in _layoutControl.SelectedLayoutElements.ToArray())
                _lbxItems.SelectedItems.Add(le);

            _lbxItems.ResumeLayout();*/
        }

        // CGX

        /// <summary>
        /// Retourne le composant liste
        /// </summary>
        //public ListBox ListBox
        //{
        //    get { return _lbxItems; }
        //}        

        /// <summary>
        /// Retourne le composant liste
        /// </summary>
        public TreeView TreeView
        {
            get { return TV_Items; }
        }

        //Fin CGX

        #endregion

        #region ---------------- Event Handlers

        private void le_ThumbnailChanged(object sender, EventArgs e)
        {
            _lbxItems.Invalidate();
        }

        private void _layoutControl_ElementsChanged(object sender, EventArgs e)
        {
            // CGX END
            if (_suppressSelectionChange) return;
            _suppressSelectionChange = true;

            Tuple<string, LayoutElement> t = sender as Tuple<string, LayoutElement>;
            //System.Diagnostics.Debug.WriteLine(t);

            if (t != null)
            {

                TV_Items.SuspendLayout();
                if (t.Item1 == "ADD") AddTreeNodeToTreeView(t.Item2);
                if (t.Item1 == "DELETE") RemoveTreeNodeFromTreeView(t.Item2);
                TV_Items.ResumeLayout();
            }
            RefreshList();


            //RefreshListSelection();
            _suppressSelectionChange = false;
            // CGX END
            /*if (_suppressSelectionChange) return;
            _suppressSelectionChange = true;
            RefreshList();
            _suppressSelectionChange = false;*/
        }

        private void _layoutControl_SelectionChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange) return;
            _suppressSelectionChange = true;
            RefreshList();
            _suppressSelectionChange = false;
        }

        private void lbxItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_suppressSelectionChange) return;
            _suppressSelectionChange = true;
            _layoutControl.SuspendLayout();
            _layoutControl.ClearSelection();
            _layoutControl.AddToSelection(new List<LayoutElement>(_lbxItems.SelectedItems.OfType<LayoutElement>()));
            _layoutControl.ResumeLayout();
            _suppressSelectionChange = false;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionDown();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            _layoutControl.MoveSelectionUp();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            _layoutControl.DeleteSelected();
        }

        private void LayoutListBox_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Delete:
                    _layoutControl.DeleteSelected();
                    break;
                case Keys.F5:
                    _layoutControl.RefreshElements();
                    break;
            }
        }

        #endregion

        // CGX
        /// <summary>
        /// 
        /// </summary>
        public TreeNode GetItem(Point p)
        {
            TreeNode TN = null;

            try
            {
                TN = TV_Items.GetNodeAt(p);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

            return TN;
        }

        /// <summary>
        /// 
        /// </summary>
        private void TV_Items_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (_suppressSelectionChange) return;

                _suppressSelectionChange = true;
                // Si on clique sur un groupe, on sélectionne ses enfants
                if (e.Node.Tag == null)
                {
                    List<LayoutElement> lLE = new List<LayoutElement>();

                    foreach (TreeNode TN in e.Node.Nodes)
                    {
                        if (TN.Tag != null)
                        {
                            lLE.Add(TN.Tag as LayoutElement);
                            TV_Items.SelectedNodes.Add(TN);
                        }
                    }
                    _layoutControl.AddToSelection(lLE);
                    //RefreshListSelection();
                }
                _suppressSelectionChange = false;
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Action de click sur un noeud du treeview
        /// </summary>
        private void TV_Items_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                //if (e.Button == System.Windows.Forms.MouseButtons.Right)
                //{
                //    //ContextMenuStrip CMS = new ContextMenuStrip();
                //    if (TV_Items.ContextMenuStrip == null)
                //        TV_Items.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

                //    if(TV_Items.SelectedNodes.Count > 1)
                //        TV_Items.ContextMenuStrip.Items.Add(CreateDynamicToolStripButton("Group", global::DotSpatial.Controls.Images.CreateFolder, OnGroupItems));
                //    if(TV_Items.SelectedNodes.Count == 1)
                //        if (TV_Items.SelectedNodes[0].Tag == null)
                //            TV_Items.ContextMenuStrip.Items.Add(CreateDynamicToolStripButton("Rename", global::DotSpatial.Controls.Images.RenameGroup, OnGroupRename));

                //    TV_Items.ContextMenuStrip.Show(TV_Items.PointToScreen(e.Location));
                //}
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Creation dynamique du context menu
        /// </summary>
        private ToolStripMenuItem CreateDynamicToolStripButton(string sName, Image iImg, EventHandler eEV)
        {
            ToolStripMenuItem TSB = new ToolStripMenuItem();

            try
            {
                TSB = new ToolStripMenuItem(sName);
                TSB.Image = iImg;
                TSB.Click += eEV;
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

            return TSB;
        }

        /// <summary>
        /// Action de groupage des elements
        /// </summary>
        public void OnGroupItems(object sender, EventArgs e)
        {
            try
            {
                // Creation du nom du groupe
                string sNewGroup = GetGroupName();

                // Stockage de la valeur max de l'index
                int iIndexMin = int.MaxValue;

                // Parcours des noeuds selectionnés
                List<LayoutElement> leSelected = _layoutControl.SelectedLayoutElements.OrderBy(o => _layoutControl.LayoutElements.IndexOf(o)).ToList();
                foreach (LayoutElement le in leSelected)
                {
                    int iIndexElement = _layoutControl.LayoutElements.IndexOf(le);
                    if (iIndexMin > iIndexElement)
                        iIndexMin = iIndexElement;
                    le.Group = sNewGroup;
                }

                // On regroupe les items à partir de l'index le plus bas
                foreach (LayoutElement le in leSelected)
                {
                    _layoutControl.LayoutElements.Remove(le);
                    _layoutControl.LayoutElements.Insert(iIndexMin, le);
                    iIndexMin++;
                }

                RefreshList();
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Retourne le premier nom de groupe automatique disponible
        /// </summary>
        private string GetGroupName()
        {
            string sGroup = "";

            try
            {
                int i = 1;
                bool bFind = true;

                while (bFind)
                {
                    sGroup = "Group " + i;
                    bFind = (TV_Items.Nodes.Find(sGroup, false).Length != 0);
                    i++;
                }

            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

            return sGroup;
        }

        /// <summary>
        /// Action de renommage des groupes
        /// </summary>
        public void OnGroupRename(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    TreeNode TN = sender as TreeNode;
                    if (TN.Tag == null)
                    {
                        if (!TN.IsEditing)
                        {
                            TN.BeginEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Action levée après l'dition d'un noeud (groupe)
        /// </summary>
        private void TV_Items_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                if (e.Label != null)
                {
                    if (e.Label.Length > 0)
                    {
                        if (e.Label.IndexOfAny(new char[] { '@', '.', ',', '!' }) == -1)
                        {
                            // Stop editing without canceling the label change.
                            e.Node.EndEdit(false);
                            foreach (TreeNode TN in e.Node.Nodes)
                                (TN.Tag as LayoutElement).Group = e.Label;
                        }
                        else
                        {
                            /* Cancel the label edit action, inform the user, and 
                               place the node in edit mode again. */
                            e.CancelEdit = true;
                            MessageBox.Show("Invalid tree node label.\n" +
                               "The invalid characters are: '@','.', ',', '!'",
                               "Node Label Edit");
                            e.Node.BeginEdit();
                        }
                    }
                    else
                    {
                        /* Cancel the label edit action, inform the user, and 
                           place the node in edit mode again. */
                        e.CancelEdit = true;
                        MessageBox.Show("Invalid tree node label.\nThe label cannot be blank",
                           "Node Label Edit");
                        e.Node.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
        }

        /// <summary>
        /// Action levée lorsque laa sélection du treeview change
        /// </summary>
        private void TV_Items_SelectionsChanged(object sender, EventArgs e)
        {
            try
            {
                if (_suppressSelectionChange) return;
                _suppressSelectionChange = true;
                _layoutControl.SuspendLayout();
                _layoutControl.ClearSelection();
                List<LayoutElement> lSelected = new List<LayoutElement>();
                foreach (TreeNode TN in TV_Items.SelectedNodes)
                    if (TN.Tag != null)
                        lSelected.Add(TN.Tag as LayoutElement);
                _layoutControl.AddToSelection(lSelected);
                _layoutControl.ResumeLayout();
                _suppressSelectionChange = false;
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

        }

        /// <summary>
        /// 
        /// </summary>
        private void TV_Items_KeyUp(object sender, KeyEventArgs e)
        {
            _layoutControl.SuspendLayout();
            try
            {
                switch (e.KeyCode)
                {
                    case Keys.Delete: _layoutControl.DeleteSelected(); break;
                    case Keys.F5: _layoutControl.RefreshElements(); break;
                    default: break;
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
            _layoutControl.ResumeLayout();
        }

        /// <summary>
        /// 
        /// </summary>
        public List<TreeNode> SelectedNodes()
        {
            List<TreeNode> lSelected = new List<TreeNode>();

            try
            {
                foreach (TreeNode TN in TV_Items.SelectedNodes)
                    lSelected.Add(TN);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

            return lSelected;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<TreeNode> SelectedRoot()
        {
            List<TreeNode> lSelected = new List<TreeNode>();

            try
            {
                foreach (TreeNode TN in TV_Items.SelectedNodes)
                    lSelected.Add(TN);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

            return lSelected;
        }
        // CGX END
    }
}