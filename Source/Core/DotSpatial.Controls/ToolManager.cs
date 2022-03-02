// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Modeling.Forms;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This class provides a ToolManager for loading tools from .dll's.
    /// </summary>
    // This control will no longer be visible
    [ToolboxItem(false)]
    public class ToolManager : TreeView, IPartImportsSatisfiedNotification
    {
        #region Fields

        private readonly ToolTip _toolTipTree;
        private ITool _toolToExecute;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ToolManager"/> class that scans the executables root path\ tools folder for tools.
        /// </summary>
        public ToolManager()
        {
            // Sets up some initial variables
            _toolTipTree = new ToolTip();
            Tools = new List<ITool>();

            ImageList = new ImageList();
            ImageList.Images.Add("Hammer", Images.HammerSmall);

            NodeMouseDoubleClick += ToolManagerNodeMouseDoubleClick;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current AppManager handle.
        /// </summary>
        public AppManager App { get; set; }

        /// <summary>
        /// Gets the data that are available by default to tools.
        /// </summary>
        [Category("ToolManager Appearance")]
        [Description("Sets the data that are available by default to tools")]
        public List<DataSetArray> DataSets
        {
            get
            {
                List<DataSetArray> dataSets = new List<DataSetArray>();
                if (Legend != null)
                {
                    foreach (ILegendItem node in Legend.RootNodes)
                    {
                        dataSets.AddRange(PopulateDataSets(node as IGroup));
                    }
                }

                return dataSets;
            }
        }

        /// <summary>
        /// Gets or Sets the legend object. This is needed to automatically populate the list of data layers in tool dialogs.
        /// </summary>
        [Category("ToolManager Appearance")]
        [Description("Gets or Sets the legend object. This is needed to automatically populate the list of data layers in tool dialogs.")]
        public ILegend Legend { get; set; }

        /// <summary>
        /// Gets or sets the list of the available tools.
        /// </summary>
        [Browsable(false)]
        [ImportMany(AllowRecomposition = true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEnumerable<ITool> Tools { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if the Tool Manager can create the tool specified by the Name.
        /// </summary>
        /// <param name="name">The unique name of a tool.</param>
        /// <returns>true if the tool can be created otherwise false.</returns>
        public bool CanCreateTool(string name)
        {
            return Tools.Any(tool => tool.AssemblyQualifiedName == name);
        }

        /// <summary>
        /// Creates a new instance of a tool based on its Name.
        /// </summary>
        /// <param name="name">The unique name of the tool.</param>
        /// <returns>Returns an new instance of the tool or NULL if the tools unique name doesn't exist in the manager.</returns>
        public ITool GetTool(string name)
        {
            ITool tool = Tools.FirstOrDefault(t => t.AssemblyQualifiedName == name);
            tool?.Initialize();
            return tool;
        }

        /// <summary>
        /// Highlights the next tool.
        /// </summary>
        /// <param name="toolName">The name of the tool.</param>
        public void HighlightNextTool(string toolName)
        {
            TreeNode selectedNode = SelectedNode;
            CollapseAll();

            if (string.IsNullOrEmpty(toolName) || selectedNode == null)
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
        /// Locates a tool by its name in the tree and highlights it.
        /// </summary>
        /// <param name="toolName">The name of the tool.</param>
        public void HighlightTool(string toolName)
        {
            CollapseAll();
            if (string.IsNullOrEmpty(toolName))
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
        }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use. Refreshes the tree of tools.
        /// </summary>
        public void OnImportsSatisfied()
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(RefreshTree));
            else
                RefreshTree();
        }

        /// <summary>
        /// This clears the list of available tools and loads them from file again.
        /// </summary>
        public virtual void RefreshTree()
        {
            // We clear the list of tools and providers
            Nodes.Clear();

            // Re-populate the tool treeview
            foreach (ITool tool in Tools)
            {
                // If the tool's category doesn't exist we add it
                string category = tool.Category ?? "Default Category";

                if (Nodes[category] == null)
                    Nodes.Add(category, category);

                // we add the tool with the default icon
                Nodes[category].Nodes.Add(tool.AssemblyQualifiedName, tool.Name, "Hammer", "Hammer");
            }

            Refresh();
        }

        /// <summary>
        /// Runs when an item gets dragged.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);

            TreeNode theNode = e.Item as TreeNode;

            // Verify that the tag property is not "null".
            if (theNode?.Parent != null && Tools.Any(tool => tool.Name == theNode.Name))
            {
                DoDragDrop("ITool: " + theNode.Name, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// Thie event fires when the mouse moves to change the ToolTip.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Get the node at the current mouse pointer location.
            TreeNode theNode = GetNodeAt(e.X, e.Y);

            // Set a ToolTip only if the mouse pointer is actually paused on a node.
            if (theNode != null)
            {
                // Verify that the tag property is not "null".
                var tool = Tools.FirstOrDefault(t => t.Name == theNode.Name);
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

        /// <summary>
        /// Handles the NodeMouseDoubleClick event of the ToolManager control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        protected void ToolManagerNodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            DoDoubleClick(e.Node);
        }

        private static void BwDoWork(object sender, DoWorkEventArgs e)
        {
            object[] threadParameter = e.Argument as object[];
            if (threadParameter == null) return;
            ITool toolToExecute = threadParameter[0] as ITool;
            ToolProgress progForm = threadParameter[1] as ToolProgress;

            if (progForm == null) return;
            if (toolToExecute == null) return;
            progForm.Progress(0, "==================");
            progForm.Progress(0, string.Format(MessageStrings.ToolManager_ExecutingTool, toolToExecute.Name));
            progForm.Progress(0, "==================");
            bool result = false;
            try
            {
                result = toolToExecute.Execute(progForm);
            }
            catch (Exception ex)
            {
                progForm.Progress(100, "Error: " + ex);
            }

            e.Result = result;
            progForm.ExecutionComplete();
            progForm.Progress(100, "==================");
            progForm.Progress(100, string.Format(MessageStrings.ToolManager_DoneExecutingTool, toolToExecute.Name));
            progForm.Progress(100, "==================");
        }

        private void DoDoubleClick(TreeNode theNode)
        {
            // Checks if the user double clicked a node and not a white spot
            if (theNode == null)
                return;

            // checks if the user clicked a tool or a category

            // Get an instance of the tool and dialog box to go with it
            _toolToExecute = GetTool(theNode.Name);
            if (_toolToExecute != null)
            {
                Extent ex = new Extent(-180, -90, 180, 90);

                // it wasn't a category?
                IMapFrame mf = Legend?.RootNodes[0] as IMapFrame;
                if (mf != null) ex = mf.ViewExtents;

                ToolDialog td = new ToolDialog(_toolToExecute, DataSets, ex);
                DialogResult tdResult = td.ShowDialog(this);
                while (tdResult == DialogResult.OK && td.ToolStatus != ToolStatus.Ok)
                {
                    MessageBox.Show(MessageStrings.ToolSetupIncorectly);
                    tdResult = td.ShowDialog(this);
                }

                if (tdResult == DialogResult.OK && td.ToolStatus == ToolStatus.Ok)
                {
                    // This fires when the user clicks the "OK" button on a tool dialog
                    // First we create the progress form
                    ToolProgress progForm = new ToolProgress(1);

                    // We create a background worker thread to execute the tool
                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += BwDoWork;
                    bw.RunWorkerCompleted += ExecutionComplete;

                    object[] threadParameter = new object[2];
                    threadParameter[0] = _toolToExecute;
                    threadParameter[1] = progForm;

                    // Show the progress dialog and kick off the Async thread
                    progForm.Show(this);
                    bw.RunWorkerAsync(threadParameter);
                }
            }
        }

        private void ExecutionComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_toolToExecute.OutputParameters == null || !(bool)e.Result) return;

            // has Parameter AddToMap set to false -> don't add to map
            if (_toolToExecute.OutputParameters.Any(_ => _.Name == "AddToMap" && !(bool)_.Value)) return;

            foreach (var p in _toolToExecute.OutputParameters)
            {
                try
                {
                    var featureset = p.Value as IFeatureSet;
                    if (featureset != null)
                    {
                        App.Map.AddLayer(featureset.Filename);
                    }
                    else
                    {
                        var rasterset = p.Value as IRaster;
                        if (rasterset != null)
                        {
                            App.Map.AddLayer(rasterset.Filename);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(MessageStrings.ToolManager_UnableToAddLayer, ex.Message));
                }
            }
        }

        /// <summary>
        /// Recursive function to add all datasets.
        /// </summary>
        /// <param name="root">Root item to start searching.</param>
        /// <returns>List of all the found datasets.</returns>
        private List<DataSetArray> PopulateDataSets(IGroup root)
        {
            List<DataSetArray> dataSets = new List<DataSetArray>();
            if (root != null)
            {
                foreach (ILayer ml in root)
                {
                    if (ml.DataSet != null)
                    {
                        dataSets.Add(new DataSetArray(ml.LegendText, ml.DataSet));
                        IFeatureLayer fl = ml as IFeatureLayer;
                        if (fl != null && fl.Selection.Count > 0)
                            dataSets.Add(new DataSetArray(fl.LegendText + " - Current Selection", fl.Selection.ToFeatureSet()));
                    }

                    if (ml.GetType() == Type.GetType("DotSpatial.Controls.MapGroup"))
                        dataSets.AddRange(PopulateDataSets(ml as IGroup));
                }
            }

            return dataSets;
        }

        #endregion
    }
}