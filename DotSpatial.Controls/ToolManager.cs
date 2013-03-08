// ********************************************************************************************************
// Product Name: DotSpatial.Tools.ToolManager
// Description:  Deals with loading and finding tools from a specific folder
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is Toolbox.dll for the DotSpatial 4.6/6 ToolManager project
//
// The Initializeializeial Developer of this Original Code is Brian Marchionni. Created in Oct, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |------------|
// Ted Dunsford      | 8/28/2009  | Cleaned up some code formatting using resharper
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Symbology;
using System.Diagnostics;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This class provides a ToolManager for loading tools from .dll's
    /// </summary>
    public class ToolManager : TreeView, IPartImportsSatisfiedNotification
    {
        #region Constants and Fields

        private readonly ToolTip _toolTipTree;
        private ILegend _legend;
        private ITool toolToExecute;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Creates a new instance of the ToolManager, scans the executables root path\tools
        /// </summary>
        public ToolManager()
        {
            //Sets up some initial variables
            _toolTipTree = new ToolTip();
            Tools = new List<ITool>();

            base.ImageList = new ImageList();
            base.ImageList.Images.Add("Hammer", Images.HammerSmall);

            base.NodeMouseDoubleClick += ToolManager_NodeMouseDoubleClick;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Sets the data that are available by default to tools
        /// </summary>
        [Category("ToolManager Appearance"), Description("Sets the data that are available by default to tools")]
        public List<DataSetArray> DataSets
        {
            get
            {
                List<DataSetArray> dataSets = new List<DataSetArray>();
                if (_legend != null)
                {
                    for (int i = 0; i < _legend.RootNodes.Count; i++)
                    {
                        dataSets.AddRange(populateDataSets(_legend.RootNodes[i] as IGroup));
                    }
                }
                return dataSets;
            }
        }

        //Recursive function to add all datasets.
        private List<DataSetArray> populateDataSets(IGroup root)
        {
            List<DataSetArray> dataSets = new List<DataSetArray>();
            if (root != null)
            {
                foreach (ILayer ml in root)
                {
                    if (ml.DataSet != null)
                    {
                        dataSets.Add(new DataSetArray(ml.LegendText, ml.DataSet));
                    }
                    if(ml.GetType().Equals(Type.GetType("DotSpatial.Controls.MapGroup")))
                    {
                        dataSets.AddRange(populateDataSets(ml as IGroup));
                    }
                }
            }
            return dataSets;
        }

        /// <summary>
        /// Gets or Sets the legend object. This is needed to automatically populate the list of data layers in tool dialogs.
        /// </summary>
        [Category("ToolManager Appearance"), Description("Gets or Sets the legend object. This is needed to automatically populate the list of data layers in tool dialogs.")]
        public ILegend Legend
        {
            get { return _legend; }
            set
            {
                _legend = value;
            }
        }

        public AppManager App { get; set; }

        /// <summary>
        /// Gets the list tools available.
        /// </summary>
        [Browsable(false)]
        [ImportMany(AllowRecomposition = true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEnumerable<ITool> Tools { get; protected set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use. Refreshes the tree of tools.
        /// </summary>
        public void OnImportsSatisfied()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate
                                             {
                                                 RefreshTree();
                                             }));
            }
            else
            {
                RefreshTree();
            }
        }

        /// <summary>
        /// Returns true if the Tool Manager can create the tool specified by the Name
        /// </summary>
        /// <param name="name">The unique name of a tool</param>
        /// <returns>true if the tool can be created otherwise false</returns>
        public bool CanCreateTool(string name)
        {
            return Tools.Where(tool => tool.AssemblyQualifiedName == name).Any();
        }

        /// <summary>
        /// Creates a new instance of a tool based on its Name
        /// </summary>
        /// <param name="name">The unique name of the tool</param>
        /// <returns>Returns an new instance of the tool or NULL if the tools unique name doesn't exist in the manager</returns>
        public ITool GetTool(string name)
        {
            ITool tool = Tools.Where(t => t.AssemblyQualifiedName == name).FirstOrDefault();
            if (tool != null)
            {
                tool.Initialize();
            }
            return tool;
        }

        /// <summary>
        /// Highlights the next tool
        /// </summary>
        /// <param name="toolName"></param>
        public void HighlightNextTool(string toolName)
        {
            TreeNode selectedNode = SelectedNode;
            CollapseAll();

            if (toolName == string.Empty || selectedNode == null)
                return;

            bool foundSelected = false;
            for (int i = 0; i < Nodes.Count; i++)
            {
                for (int j = 0; j < Nodes[i].Nodes.Count; j++)
                {
                    if (Nodes[i].Nodes[j] == selectedNode)
                    {
                        foundSelected = true;
                        continue;
                    }
                    if (foundSelected && Nodes[i].Nodes[j].Text.ToLower().Contains(toolName.ToLower()))
                    {
                        Nodes[i].Nodes[j].Expand();
                        SelectedNode = Nodes[i].Nodes[j];
                        return;
                    }
                }
            }

            HighlightTool(toolName);
        }

        /// <summary>
        /// Locates a tool by its name in the tree and highlights it
        /// </summary>
        /// <param name="toolName"></param>
        public void HighlightTool(string toolName)
        {
            CollapseAll();
            if (toolName == string.Empty)
                return;

            for (int i = 0; i < Nodes.Count; i++)
            {
                for (int j = 0; j < Nodes[i].Nodes.Count; j++)
                {
                    if (Nodes[i].Nodes[j].Text.ToLower().Contains(toolName.ToLower()))
                    {
                        Nodes[i].Nodes[j].Expand();
                        SelectedNode = Nodes[i].Nodes[j];
                        return;
                    }
                }
            }
            CollapseAll();
            return;
        }

        /// <summary>
        /// This clears the list of available tools and loads them from file again
        /// </summary>
        public virtual void RefreshTree()
        {
            //We clear the list of tools and providers
            Nodes.Clear();

            //Re-populate the tool treeview
            foreach (ITool tool in Tools)
            {
                //If the tool's category doesn't exist we add it
                string category = tool.Category;
                if (category == null)
                    category = "Default Category";

                if (Nodes[category] == null)
                    Nodes.Add(category, category);

                //we add the tool with the default icon
                Nodes[category].Nodes.Add(tool.AssemblyQualifiedName, tool.Name, "Hammer", "Hammer");
            }
            this.Refresh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the NodeMouseDoubleClick event of the ToolManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        protected void ToolManager_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            DoDoubleClick(e.Node);
        }

        /// <summary>
        /// Runs when and item gets dragged
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);

            TreeNode theNode = e.Item as TreeNode;
            if (theNode != null)
            {
                // Verify that the tag property is not "null".
                if ((theNode.Parent != null) && Tools.Where(tool => tool.Name == theNode.Name).Any())
                {
                    DoDragDrop("ITool: " + theNode.Name, DragDropEffects.Copy);
                }
            }
        }

        /// <summary>
        /// Thie event fires when the mouse moves to change the ToolTip
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Get the node at the current mouse pointer location.
            TreeNode theNode = GetNodeAt(e.X, e.Y);

            // Set a ToolTip only if the mouse pointer is actually paused on a node.
            if (theNode != null)
            {
                // Verify that the tag property is not "null".
                var tool = Tools.Where(t => t.Name == theNode.Name).FirstOrDefault();
                if (tool != null)
                {
                    // Change the ToolTip only if the pointer moved to a new node.
                    if (tool.ToolTip != _toolTipTree.GetToolTip(this))
                    {
                        _toolTipTree.SetToolTip(this, tool.ToolTip);
                    }
                }
                else
                {
                    _toolTipTree.SetToolTip(this, string.Empty);
                }
            }
            else
            {
                // Pointer is not over a node so clear the ToolTip.
                _toolTipTree.SetToolTip(this, string.Empty);
            }
        }

        private static void BwDoWork(object sender, DoWorkEventArgs e)
        {
            object[] threadParameter = e.Argument as object[];
            if (threadParameter == null) return;
            ITool toolToExecute = threadParameter[0] as ITool;
            ToolProgress progForm = threadParameter[1] as ToolProgress;

            if (progForm == null) return;
            if (toolToExecute == null) return;

            progForm.Progress(String.Empty, 0, "==================");
            progForm.Progress(String.Empty, 0, String.Format("Executing Tool: {0}", toolToExecute.Name));
            progForm.Progress(String.Empty, 0, "==================");
            toolToExecute.Execute(progForm);
            progForm.ExecutionComplete();
            progForm.Progress(String.Empty, 100, "==================");
            progForm.Progress(String.Empty, 100, String.Format("Done Executing Tool: {0}", toolToExecute.Name));
            progForm.Progress(String.Empty, 100, "==================");
        }

        private void executionComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (toolToExecute.OutputParameters != null)
            {
                foreach (Parameter p in toolToExecute.OutputParameters)
                {
                    IFeatureSet featureset = p.Value as IFeatureSet;
                    IRaster rasterset = p.Value as IRaster;
                    if (featureset != null)
                    {
                        try
                        {
                            App.Map.AddLayer(featureset.Filename);
                        }
                        catch
                        {
                            MessageBox.Show("Error opening layer file.");
                        }
                    }
                    else if (rasterset != null)
                    {
                        try
                        {
                            App.Map.AddLayer(rasterset.Filename);
                        }
                        catch
                        {
                            MessageBox.Show("Error opening layer file.");
                        }
                    }
                }
            }
        }

        private void DoDoubleClick(TreeNode theNode)
        {
            // Checks if the user double clicked a node and not a white spot
            if (theNode == null)
                return;

            // checks if the user clicked a tool or a category

            // Get an instance of the tool and dialog box to go with it
            toolToExecute = GetTool(theNode.Name);
            if (toolToExecute != null)
            {
                Extent ex = new Extent(-180, -90, 180, 90);

                // it wasn't a category?
                if (_legend != null)
                {
                    IMapFrame mf = _legend.RootNodes[0] as IMapFrame;
                    if (mf != null) ex = mf.ViewExtents;
                }

                ToolDialog td = new ToolDialog(toolToExecute, DataSets, ex);
                DialogResult tdResult = td.ShowDialog(this);
                while (tdResult == DialogResult.OK && td.ToolStatus != ToolStatus.Ok)
                {
                    MessageBox.Show(MessageStrings.ToolSetupIncorectly);
                    tdResult = td.ShowDialog(this);
                }
                if (tdResult == DialogResult.OK && td.ToolStatus == ToolStatus.Ok)
                {
                    //This fires when the user clicks the "OK" button on a tool dialog
                    //First we create the progress form
                    ToolProgress progForm = new ToolProgress(1);

                    //We create a background worker thread to execute the tool
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += BwDoWork;
                    bw.RunWorkerCompleted += executionComplete;

                    object[] threadParamerter = new object[2];
                    threadParamerter[0] = toolToExecute;
                    threadParamerter[1] = progForm;

                    // Show the progress dialog and kick off the Async thread
                    progForm.Show(this);
                    bw.RunWorkerAsync(threadParamerter);
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // ToolManager
            //
            this.ResumeLayout(false);
        }

        #endregion
    }
}