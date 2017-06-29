// *****************************************************************************
// 
//  Copyright 2004, Coder's Lab
//  All rights reserved. The software and associated documentation 
//  supplied hereunder are the proprietary information of Coder's Lab
//  and are supplied subject to licence terms.
//  
//
//  You can use this control freely in your projects, but let me know if you
//  are using it so I can add you to a list of references. 
//
//  Email: ludwig.stuyck@coders-lab.be
//  Home page: http://www.coders-lab.be
//
//  History
//		18/07/2004	
//			- Control creation	
//		24/07/2004	
//			- Implemented rubberband selection; also combination keys work: 
//			  ctrl, shift, ctrl+shift	
//		25/08/2004	
//			- Rubberband selection temporary removed due to scrolling problems. 
//			- Renamed TreeViewSelectionMode property to SelectionMode.
//			- Renamed SelectionModes enumeration to TreeViewSelectionMode.
//			- Added MultiSelectSameParent selection mode.
//			- Added keyboard functionality.
//			- Enhanced selection drawing.
//			- Added SelectionBackColor property.	
//		02/09/2004	
//			- When shift/ctrl was pressed, treeview scrolled to last selected 
//			  node. Fixed.
//			- Moved TreeViewSelectionMode outside the TreeView class.
//			- BeforeSelect was fired multiple times, AfterSelect was never 
//			  fired. Fixed.
//			- Collapsing/Expanding node changed selection. This does not happen 
//			  anymore, except if a node that has selected descendants is 
//			  collapsed; then all descendants are unselected and the collapsed 
//			  node becomes selected.
//			- If in the BeforeSelect event, e.Cancel is set to true, then node 
//			  will not be selected
//			- SHIFT selection sometimes didn’t behave correctly. Fixed.
//		04/09/2004	
//			- SelectedNodes is no longer an array of tree nodes, but a 
//			  SelectedNodesCollection
//			- In the AfterSelect event, the SelectedNodes contained two tree 
//			  nodes; the old one and the new one. Fixed.
//		05/09/2004	
//			- Added Home, End, PgUp and PgDwn keys functionality	
//		08/10/2004
//			- SelectedNodeCollection renamed to NodeCollection
//			- Fixes by GKM
//
//		18/8/2005
//			- Added events BeforeDeselect and AfterDeselect
//		09/5/2007
//			- Added an InvokeRequired check to Flashnode()
//		16/5/2007
//			- Gave the document a consistant format
//			- Created a new event 'SelectionsChanged'
//		12/2010
//			-bug fixes to multi select when shift-click used
// 
// *****************************************************************************

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace CodersLab.Windows.Controls
{
    //*****************************************************************************
	#region TreeViewSelectionMode enumeration

	/// <summary>
	/// Selection mode for the treeview.	
	/// </summary>
	/// <remarks>
	/// The Selection mode determines how treeview nodes can be selected.
	/// </remarks>
	public enum TreeViewSelectionMode
	{
		/// <summary>
		/// Only one node can be selected at a time.
		/// </summary>
		SingleSelect,
		/// <summary>
		/// Multiple nodes can be selected at the same time without restriction.
		/// </summary>
		MultiSelect,
		/// <summary>
		/// Multiple nodes that belong to the same root branch can be selected at the same time.
		/// </summary>
		MultiSelectSameRootBranch,
		/// <summary>
		/// Multiple nodes that belong to the same level can be selected at the same time.
		/// </summary>
		MultiSelectSameLevel,
		/// <summary>
		/// Multiple nodes that belong to the same level and same root branch can be selected at the same time.
		/// </summary>
		MultiSelectSameLevelAndRootBranch,
		/// <summary>
		/// Only nodes that belong to the same direct parent can be selected at the same time.
		/// </summary>
		MultiSelectSameParent
	}

	#endregion

    //*****************************************************************************
	#region Delegates

	/// <summary>
	/// Delegate used for tree node events.
	/// </summary>
	public delegate void TreeNodeEventHandler(TreeNode tn);

	#endregion

	/// <summary>
	/// The TreeView control is a regular treeview with multi-selection capability.
	/// </summary>
	[ToolboxItem(true)]
	public class TreeView : System.Windows.Forms.TreeView
    {
        //=========================================================================
        #region Properties

        public event TreeViewEventHandler AfterDeselect;
		public event TreeViewEventHandler BeforeDeselect;
		public event EventHandler SelectionsChanged;

        #endregion

        //=========================================================================
        #region Events

        /// <summary>
        /// 
        /// </summary>
		protected void OnAfterDeselect(TreeNode tn)
		{
            try
            {
                if (AfterDeselect != null)
                {
                    AfterDeselect(this, new TreeViewEventArgs(tn));
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

        /// <summary>
        /// 
        /// </summary>
		protected void OnBeforeDeselect(TreeNode tn)
        {
            try
            {
                if (BeforeDeselect != null)
                {
                    BeforeDeselect(this, new TreeViewEventArgs(tn));
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

        /// <summary>
        /// 
        /// </summary>
		protected void OnSelectionsChanged()
		{
            try
            {
                if (blnSelectionChanged)
                {
                    if (SelectionsChanged != null)
                    {
                        SelectionsChanged(this, new EventArgs());
                    }
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

        #endregion

        //=========================================================================
        #region Private variables

        /// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Used to make sure that SelectedNode can only be used from within this class.
		/// </summary>
		private bool blnInternalCall = false;

		/// <summary>
		/// Hashtable that contains all selected nodes.
		/// </summary>
		private Hashtable htblSelectedNodes = new Hashtable();

		/// <summary>
		/// Track whether the total SelectedNodes changed across multiple operations
		/// for SelectionsChanged event
		/// </summary>
		private bool blnSelectionChanged = false;

		/// <summary>
		/// Hashtable to preserve Node's original colors (colors can be set on the TreeView, or individual nodes)
		/// (GKM)
		/// </summary>
		private Hashtable htblSelectedNodesOrigColors = new Hashtable();

		/// <summary>
		/// Keeps track of node that has to be pu in edit mode.
		/// </summary>
		private TreeNode tnNodeToStartEditOn = null;

		/// <summary>
		/// Remembers whether mouse click on a node was single or double click.
		/// </summary>
		private bool blnWasDoubleClick = false;

		/// <summary>
		/// Keeps track of most recent selected node.
		/// </summary>
		private TreeNode tnMostRecentSelectedNode = null;

		/// <summary>
		/// Keeps track of the selection mirror point; this is the last selected node without SHIFT key pressed.
		/// It is used as the mirror node during SHIFT selection.
		/// </summary>
		private TreeNode tnSelectionMirrorPoint = null;

		/// <summary>
		/// Keeps track of the number of mouse clicks.
		/// </summary>
		private int intMouseClicks = 0;

		/// <summary>
		/// Selection mode.
		/// </summary>
		private TreeViewSelectionMode selectionMode = TreeViewSelectionMode.SingleSelect;

		/// <summary>
		/// Backcolor for selected nodes.
		/// </summary>
		private Color selectionBackColor = System.Drawing.SystemColors.Highlight;

		/// <summary>
		/// Keeps track whether a node click has been handled by the mouse down event. This is almost always the
		/// case, except when a selected node has been clicked again. Then, it will not be handled in the mouse
		/// down event because we might want to drag the node and if that's the case, node should not go in edit 
		/// mode.
		/// </summary>
		private bool blnNodeProcessedOnMouseDown = false;

		/// <summary>
		/// Holds node that needs to be flashed.
		/// </summary>
		private TreeNode tnToFlash = null;

		/// <summary>
		/// Keeps track of the first selected node when selection has begun with the keyboard.
		/// </summary>
		private TreeNode tnKeysStartNode = null;

		#endregion

        //=========================================================================
		#region SelectedNode, SelectionMode, SelectionBackColor, SelectedNodes + events

		/// <summary>
		/// This property is for internal use only. Use SelectedNodes instead.
		/// </summary>
		public new TreeNode SelectedNode
		{
			get
			{
				if (!blnInternalCall)
				{
					throw new NotSupportedException("Use SelectedNodes instead of SelectedNode.");
				}
				else
				{
					return base.SelectedNode;
				}
			}
			set
			{
				if (!blnInternalCall)
				{
					throw new NotSupportedException("Use SelectedNodes instead of SelectedNode.");
				}
				else
				{
					base.SelectedNode = value;
				}
			}
		}

		/// <summary>
		/// Gets/sets selection mode.
		/// </summary>
		public TreeViewSelectionMode SelectionMode
		{
			get
			{
				return selectionMode;
			}
			set
			{
				selectionMode = value;
			}
		}

		/// <summary>
		/// Gets/sets backcolor for selected nodes.
		/// </summary>
		public Color SelectionBackColor
		{
			get
			{
				return selectionBackColor;
			}
			set
			{
				selectionBackColor = value;
			}
		}

		/// <summary>
		/// Gets selected nodes.
		/// </summary>
		public NodesCollection SelectedNodes
		{
			get
			{
				// Create a SelectedNodesCollection to return, and add event handlers to catch actions on it
				NodesCollection selectedNodesCollection = new NodesCollection();
				foreach (TreeNode tn in htblSelectedNodes.Values)
				{
					selectedNodesCollection.Add(tn);
				}

				selectedNodesCollection.TreeNodeAdded += new TreeNodeEventHandler(SelectedNodes_TreeNodeAdded);
				selectedNodesCollection.TreeNodeInserted += new TreeNodeEventHandler(SelectedNodes_TreeNodeInserted);
				selectedNodesCollection.TreeNodeRemoved += new TreeNodeEventHandler(SelectedNodes_TreeNodeRemoved);
				selectedNodesCollection.SelectedNodesCleared += new EventHandler(SelectedNodes_SelectedNodesCleared);

				return selectedNodesCollection;
			}
		}

		/// <summary>
		/// Occurs when a tree node is added to the SelectedNodes collection.
		/// </summary>
		/// <param name="tn">Tree node that was added.</param>
		private void SelectedNodes_TreeNodeAdded(TreeNode tn)
		{
            try
            {
                blnSelectionChanged = false;

                SelectNode(tn, true, TreeViewAction.Unknown);
                //ProcessNodeRange(null, tn, new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X,  Cursor.Position.Y, 0), Keys.None, TreeViewAction.ByKeyboard, false); 

                OnSelectionsChanged();
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Occurs when a tree node is inserted to the SelectedNodes collection.
		/// </summary>
		/// <param name="tn">tree node that was inserted.</param>
		private void SelectedNodes_TreeNodeInserted(TreeNode tn)
		{
            try
            {
                blnSelectionChanged = false;

                SelectNode(tn, true, TreeViewAction.Unknown);

                OnSelectionsChanged();
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Occurs when a tree node is removed from the SelectedNodes collection.
		/// </summary>
		/// <param name="tn">Tree node that was removed.</param>
		private void SelectedNodes_TreeNodeRemoved(TreeNode tn)
		{
            try
            {
                blnSelectionChanged = false;

                SelectNode(tn, false, TreeViewAction.Unknown);

                OnSelectionsChanged();

                if (tnSelectionMirrorPoint == tn)
                    tnSelectionMirrorPoint = null;
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Occurs when the SelectedNodes collection was cleared.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SelectedNodes_SelectedNodesCleared(object sender, EventArgs e)
		{
            try
            {
                blnSelectionChanged = false;

                UnselectAllNodes(TreeViewAction.Unknown);

                OnSelectionsChanged();
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region Node selection methods

		/// <summary>
		/// Unselects all selected nodes.
		/// </summary>
		/// <param name="tva">Specifies the action that caused the selection change.</param>
		private void UnselectAllNodes(TreeViewAction tva)
		{
            try
            {
                UnselectAllNodesExceptNode(null, tva);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Unselects all selected nodes that don't belong to the specified level.
		/// </summary>
		/// <param name="level">Node level.</param>
		/// <param name="tva">Specifies the action that caused the selection change.</param>
		private void UnselectAllNodesNotBelongingToLevel(int level, TreeViewAction tva)
		{
            try
            {
                // First, build list of nodes that need to be unselected
                ArrayList arrNodesToDeselect = new ArrayList();
                foreach (TreeNode selectedTreeNode in htblSelectedNodes.Values)
                {
                    if (GetNodeLevel(selectedTreeNode) != level)
                    {
                        arrNodesToDeselect.Add(selectedTreeNode);
                    }
                }

                // Do the actual unselect
                foreach (TreeNode tnToDeselect in arrNodesToDeselect)
                {
                    SelectNode(tnToDeselect, false, tva);
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Unselects all selected nodes that don't belong directly to the specified parent.
		/// </summary>
		/// <param name="parent">Parent node.</param>
		/// <param name="tva">Specifies the action that caused the selection change.</param>
		private void UnselectAllNodesNotBelongingDirectlyToParent(TreeNode parent, TreeViewAction tva)
		{
            try
            {
                // First, build list of nodes that need to be unselected
                ArrayList arrNodesToDeselect = new ArrayList();
                foreach (TreeNode selectedTreeNode in htblSelectedNodes.Values)
                {
                    if (selectedTreeNode.Parent != parent)
                    {
                        arrNodesToDeselect.Add(selectedTreeNode);
                    }
                }

                // Do the actual unselect
                foreach (TreeNode tnToDeselect in arrNodesToDeselect)
                {
                    SelectNode(tnToDeselect, false, tva);
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Unselects all selected nodes that don't belong directly or indirectly to the specified parent.
		/// </summary>
		/// <param name="parent">Parent node.</param>
		/// <param name="tva">Specifies the action that caused the selection change.</param>
		private void UnselectAllNodesNotBelongingToParent(TreeNode parent, TreeViewAction tva)
		{
            try
            {
                // First, build list of nodes that need to be unselected
                ArrayList arrNodesToDeselect = new ArrayList();
                foreach (TreeNode selectedTreeNode in htblSelectedNodes.Values)
                {
                    if (!IsChildOf(selectedTreeNode, parent))
                    {
                        arrNodesToDeselect.Add(selectedTreeNode);
                    }
                }

                // Do the actual unselect
                foreach (TreeNode tnToDeselect in arrNodesToDeselect)
                {
                    SelectNode(tnToDeselect, false, tva);
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Unselects all selected nodes, except for the specified node which should not be touched.
		/// </summary>
		/// <param name="nodeKeepSelected">Node not to touch.</param>
		/// <param name="tva">Specifies the action that caused the selection change.</param>
		private void UnselectAllNodesExceptNode(TreeNode nodeKeepSelected, TreeViewAction tva)
		{
            try
            {
                // First, build list of nodes that need to be unselected
                ArrayList arrNodesToDeselect = new ArrayList();
                foreach (TreeNode selectedTreeNode in htblSelectedNodes.Values)
                {
                    if (nodeKeepSelected == null)
                    {
                        arrNodesToDeselect.Add(selectedTreeNode);
                    }
                    else if ((nodeKeepSelected != null) && (selectedTreeNode != nodeKeepSelected))
                    {
                        arrNodesToDeselect.Add(selectedTreeNode);
                    }
                }

                // Do the actual unselect
                foreach (TreeNode tnToDeselect in arrNodesToDeselect)
                {
                    SelectNode(tnToDeselect, false, tva);
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// occurs when a node is about to be selected.
		/// </summary>
		/// <param name="e">TreeViewCancelEventArgs.</param>
		protected override void OnBeforeSelect(TreeViewCancelEventArgs e)
		{
            try
            {
                // We don't want the base TreeView to handle the selection, because it can only handle single selection. 
                // Instead, we'll handle the selection ourselves by keeping track of the selected nodes and drawing the 
                // selection ourselves.
                e.Cancel = true;
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Determines whether the specified node is selected or not.
		/// </summary>
		/// <param name="tn">Node to check.</param>
		/// <returns>True if specified node is selected, false if not.</returns>
		private bool IsNodeSelected(TreeNode tn)
		{
            bool bRet = false;
            try
            {
                if (tn != null)
                    bRet = htblSelectedNodes.ContainsKey(tn.GetHashCode());
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

            return bRet;
		}

		private void PreserveNodeColors(TreeNode tn)
		{
            try
            {
                if (tn == null)
                    return;

                System.Diagnostics.Debug.WriteLine(tn.BackColor.ToString());

                if (htblSelectedNodesOrigColors.ContainsKey(tn.GetHashCode()))
                {
                    //				Color[] color = (Color[])htblSelectedNodesOrigColors[tn.GetHashCode()];
                    //				color[0]=tn.BackColor;
                    //				color[1]=tn.ForeColor;
                }
                else
                {
                    htblSelectedNodesOrigColors.Add(tn.GetHashCode(), new Color[] { tn.BackColor, tn.ForeColor });
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// (Un)selects the specified node.
		/// </summary>
		/// <param name="tn">Node to (un)select.</param>
		/// <param name="select">True to select node, false to unselect node.</param>
		/// <param name="tva">Specifies the action that caused the selection change.</param>
		/// <returns>True if node was selected, false if not.</returns>
		private bool SelectNode(TreeNode tn, bool select, TreeViewAction tva)
		{
			bool blnSelected = false;

            try
            {
                if (tn == null)
                    return false;

                if (select)
                {
                    // Only try to select node if it was not already selected																		
                    if (!IsNodeSelected(tn))
                    {
                        // Check if node selection is cancelled
                        TreeViewCancelEventArgs tvcea = new TreeViewCancelEventArgs(tn, false, tva);
                        base.OnBeforeSelect(tvcea);
                        if (tvcea.Cancel)
                        {
                            // This node selection was cancelled!						
                            return false;
                        }

                        PreserveNodeColors(tn);

                        tn.BackColor = SelectionBackColor; // GKM moved from above
                        tn.ForeColor = BackColor; // GKM moved from above									

                        htblSelectedNodes.Add(tn.GetHashCode(), tn);
                        blnSelected = true;
                        blnSelectionChanged = true;

                        base.OnAfterSelect(new TreeViewEventArgs(tn, tva));
                    }

                    tnMostRecentSelectedNode = tn;
                }
                else
                {
                    // Only unselect node if it was selected
                    if (IsNodeSelected(tn))
                    {
                        OnBeforeDeselect(tn);

                        Color[] originalColors = (Color[])this.htblSelectedNodesOrigColors[tn.GetHashCode()];
                        if (originalColors != null)
                        {
                            htblSelectedNodes.Remove(tn.GetHashCode());
                            blnSelectionChanged = true;
                            htblSelectedNodesOrigColors.Remove(tn.GetHashCode());

                            // GKM - Restore original node colors
                            tn.BackColor = originalColors[0]; // GKM - was BackColor;
                            tn.ForeColor = originalColors[1]; // GKM - was ForeColor;
                        }

                        OnAfterDeselect(tn);
                    }
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

			return blnSelected;
		}

		/// <summary>
		/// Selects nodes within the specified range.
		/// </summary>
		/// <param name="startNode">Start node.</param>
		/// <param name="endNode">End Node.</param>
		/// <param name="tva">Specifies the action that caused the selection change.</param>
		private void SelectNodesInsideRange(TreeNode startNode, TreeNode endNode, TreeViewAction tva)
		{
            try
            {
                // Calculate start node and end node
                TreeNode firstNode = null;
                TreeNode lastNode = null;
                if (startNode.Bounds.Y < endNode.Bounds.Y)
                {
                    firstNode = startNode;
                    lastNode = endNode;
                }
                else
                {
                    firstNode = endNode;
                    lastNode = startNode;
                }

                // Select each node in range
                SelectNode(firstNode, true, tva);
                TreeNode tnTemp = firstNode;
                while (tnTemp != lastNode)
                {
                    tnTemp = tnTemp.NextVisibleNode;
                    if (tnTemp != null)
                    {
                        SelectNode(tnTemp, true, tva);
                    }
                }
                SelectNode(lastNode, true, tva);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Unselects nodes outside the specified range.
		/// </summary>
		/// <param name="startNode">Start node.</param>
		/// <param name="endNode">End node.</param>
		/// <param name="tva">Specifies the action that caused the selection change.</param>
		private void UnselectNodesOutsideRange(TreeNode startNode, TreeNode endNode, TreeViewAction tva)
		{
            try
            {
                // Calculate start node and end node
                TreeNode firstNode = null;
                TreeNode lastNode = null;
                if (startNode.Bounds.Y < endNode.Bounds.Y)
                {
                    firstNode = startNode;
                    lastNode = endNode;
                }
                else
                {
                    firstNode = endNode;
                    lastNode = startNode;
                }

                // Unselect each node outside range
                TreeNode tnTemp = firstNode;
                while (tnTemp != null)
                {
                    tnTemp = tnTemp.PrevVisibleNode;
                    if (tnTemp != null)
                    {
                        SelectNode(tnTemp, false, tva);
                    }
                }

                tnTemp = lastNode;
                while (tnTemp != null)
                {
                    tnTemp = tnTemp.NextVisibleNode;
                    if (tnTemp != null)
                    {
                        SelectNode(tnTemp, false, tva);
                    }
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Recursively unselect node.
		/// </summary>
		/// <param name="tn">Node to recursively unselect.</param>
		/// <param name="tva">Specifies the action that caused the selection change.</param>
		private void UnselectNodesRecursively(TreeNode tn, TreeViewAction tva)
		{
            try
            {
                SelectNode(tn, false, tva);
                foreach (TreeNode child in tn.Nodes)
                {
                    UnselectNodesRecursively(child, tva);
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region Helper methods

		/// <summary>
		/// Determines whether a mouse click was inside the node bounds or outside the node bounds..
		/// </summary>
		/// <param name="tn">TreeNode to check.</param>
		/// <param name="e">MouseEventArgs.</param>
		/// <returns>True is mouse was clicked inside the node bounds, false if it was clicked ouside the node bounds.</returns>
		private bool IsClickOnNode(TreeNode tn, MouseEventArgs e)
		{
			if (tn == null)
				return false;

			// GKM
			// Determine the rightmost position we'll process clicks (so that the click has to be on the node's bounds, 
			// like the .NET treeview
			int rightMostX = tn.Bounds.X + tn.Bounds.Width;
			return (tn != null && e.X < rightMostX); // GKM
		}

		/// <summary>
		/// Gets level of specified node.
		/// </summary>
		/// <param name="node">Node.</param>
		/// <returns>Level of node.</returns>
		public int GetNodeLevel(TreeNode node)
		{
			int level = 0;
			while ((node = node.Parent) != null)
				level++;
			return level;
		}

		/// <summary>
		/// Determines whether the specified node is a child (indirect or direct) of the specified parent.
		/// </summary>
		/// <param name="child">Node to check.</param>
		/// <param name="parent">Parent node.</param>
		/// <returns>True if specified node is a direct or indirect child of parent node, false if not.</returns>
		private bool IsChildOf(TreeNode child, TreeNode parent)
		{
			bool blnChild = false;

            try
            {
                TreeNode tnTemp = child;
                while (tnTemp != null)
                {
                    if (tnTemp == parent)
                    {
                        blnChild = true;
                        break;
                    }
                    else
                    {
                        tnTemp = tnTemp.Parent;
                    }
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

			return blnChild;
		}

		/// <summary>
		/// Gets root parent of specified node.
		/// </summary>
		/// <param name="child">Node.</param>
		/// <returns>Root parent of specified node.</returns>
		public TreeNode GetRootParent(TreeNode child)
		{
			TreeNode tnParent = child;

            try
            {
                while (tnParent.Parent != null)
                {
                    tnParent = tnParent.Parent;
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

			return tnParent;
		}

		/// <summary>
		/// Gets number of visible nodes.
		/// </summary>
		/// <returns>Number of visible nodes.</returns>
		private int GetNumberOfVisibleNodes()
		{
			int intCounter = 0;

            try
            {
                TreeNode tnTemp = this.Nodes[0];

                while (tnTemp != null)
                {
                    if (tnTemp.IsVisible)
                    {
                        intCounter++;
                    }

                    tnTemp = tnTemp.NextVisibleNode;
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

			return intCounter;
		}

		/// <summary>
		/// Gets last visible node.
		/// </summary>
		/// <returns>Last visible node.</returns>
		private TreeNode GetLastVisibleNode()
		{
			TreeNode tnTemp = this.Nodes[0];

            try
            {
                while (tnTemp.NextVisibleNode != null)
                {
                    tnTemp = tnTemp.NextVisibleNode;
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

			return tnTemp;
		}

		/// <summary>
		/// Gets next tree node(s), starting from the specified node and direction.
		/// </summary>
		/// <param name="start">Node to start from.</param>
		/// <param name="down">True to go down, false to go up.</param>
		/// <param name="intNumber">Number of nodes to go down or up.</param>
		/// <returns>Next node.</returns>
		private TreeNode GetNextTreeNode(TreeNode start, bool down, int intNumber)
		{
			int intCounter = 0;
			TreeNode tnTemp = start;

            try
            {
                while (intCounter < intNumber)
                {
                    if (down)
                    {
                        if (tnTemp.NextVisibleNode != null)
                            tnTemp = tnTemp.NextVisibleNode;
                        else
                            break;
                    }
                    else
                    {
                        if (tnTemp.PrevVisibleNode != null)
                            tnTemp = tnTemp.PrevVisibleNode;
                        else
                            break;
                    }

                    intCounter++;
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

			return tnTemp;
		}

		/// <summary>
		/// makes focus rectangle visible or hides it.
		/// </summary>
		/// <param name="tn">Node to make focus rectangle (in)visible for.</param>
		/// <param name="visible">True to make focus rectangle visible, false to hide it.</param>
		private void SetFocusToNode(TreeNode tn, bool visible)
		{
            try
            {
                Graphics g = this.CreateGraphics();
                Rectangle rect = new Rectangle(tn.Bounds.X, tn.Bounds.Y, tn.Bounds.Width, tn.Bounds.Height);
                if (visible)
                {
                    this.Invalidate(rect, false);
                    Update();
                    if (tn.BackColor != SelectionBackColor)
                    {
                        g.DrawRectangle(new Pen(new SolidBrush(SelectionBackColor), 1), rect);
                    }
                }
                else
                {
                    if (tn.BackColor != SelectionBackColor)
                    {
                        g.DrawRectangle(new Pen(new SolidBrush(BackColor), 1), tnMostRecentSelectedNode.Bounds.X, tnMostRecentSelectedNode.Bounds.Y, tnMostRecentSelectedNode.Bounds.Width, tnMostRecentSelectedNode.Bounds.Height);
                    }
                    this.Invalidate(rect, false);
                    Update();
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
            try
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                base.Dispose(disposing);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            try
            {
                components = new System.ComponentModel.Container();
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region OnMouseUp, OnMouseDown

		/// <summary>
		/// Occurs when mouse button is up after a click.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			try
			{
				if (!this.blnNodeProcessedOnMouseDown)
				{
					TreeNode tn = this.GetNodeAt(e.X, e.Y);

					// Mouse click has not been handled by the mouse down event, so do it here. This is the case when
					// a selected node was clicked again; in that case we handle that click here because in case the
					// user is dragging the node, we should not put it in edit mode.					

					if (IsClickOnNode(tn, e))
					{
						this.ProcessNodeRange(this.tnMostRecentSelectedNode, tn, e, Control.ModifierKeys, TreeViewAction.ByMouse, true);
					}
				}

				this.blnNodeProcessedOnMouseDown = false;

				base.OnMouseUp(e);
			}
			catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

        /// <summary>
        /// 
        /// </summary>
		private bool IsPlusMinusClicked(TreeNode tn, MouseEventArgs e)
		{
			bool blnPlusMinusClicked = false;

            try
            {
                int intNodeLevel = GetNodeLevel(tn);
                if (e.X < 20 + (intNodeLevel * 20))
                    blnPlusMinusClicked = true;
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }

			return blnPlusMinusClicked;
		}

		/// <summary>
		/// Occurs when mouse is down.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
            try
            {
                tnKeysStartNode = null;

                // Store number of mouse clicks in OnMouseDown event, because here we also get e.Clicks = 2 when an item was doubleclicked
                // in OnMouseUp we seem to get always e.Clicks = 1, also when item is doubleclicked
                intMouseClicks = e.Clicks;

                TreeNode tn = this.GetNodeAt(e.X, e.Y);

                if (tn == null)
                    return;

                // Preserve colors here, because if you do it later then it will already have selected colors 
                // Don't know why...!
                PreserveNodeColors(tn);

                // If +/- was clicked, we should not process the node.
                if (!IsPlusMinusClicked(tn, e))
                {
                    // If mouse down on a node that is already selected, then we should process this node in the mouse up event, because we
                    // might want to drag it and it should not be put in edit mode.
                    // Also, only process node if click was in node's bounds.
                    if ((tn != null) && (IsClickOnNode(tn, e)) && (!IsNodeSelected(tn)))
                    {
                        // Flash node. In case the node selection is cancelled by the user, this gives the effect that it
                        // was selected and unselected again.
                        tnToFlash = tn;
                        System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(FlashNode));
                        t.Start();

                        blnNodeProcessedOnMouseDown = true;

                        System.Diagnostics.Debug.WriteLine("*** " + tn.BackColor);
                        ProcessNodeRange(tnMostRecentSelectedNode, tn, e, Control.ModifierKeys, TreeViewAction.ByMouse, true);
                    }
                }

                base.OnMouseDown(e);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region FlashNode, StartEdit

		/// <summary>
		/// Flashes node.
		/// </summary>
		private void FlashNode()
		{
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate { FlashNode(); }));
                    return;
                }

                TreeNode tn = tnToFlash;
                // Only flash node is it's not yet selected
                if (!IsNodeSelected(tn))
                {
                    tn.BackColor = SelectionBackColor;
                    tn.ForeColor = this.BackColor;
                    this.Invalidate();
                    this.Refresh();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(200);
                }

                // If node is not selected yet, restore default colors to end flashing
                if (!IsNodeSelected(tn))
                {
                    tn.BackColor = BackColor;
                    tn.ForeColor = this.ForeColor;
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Starts edit on a node.
		/// </summary>
		private void StartEdit()
		{
            try
            {
                System.Threading.Thread.Sleep(200);
                if (!blnWasDoubleClick)
                {
                    blnInternalCall = true;
                    SelectedNode = tnNodeToStartEditOn;
                    blnInternalCall = false;
                    tnNodeToStartEditOn.BeginEdit();
                }
                else
                {
                    blnWasDoubleClick = false;
                }
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region ProcessNodeRange

		/// <summary>
		/// Processes a node range.
		/// </summary>
		/// <param name="startNode">Start node of range.</param>
		/// <param name="endNode">End node of range.</param>
		/// <param name="e">MouseEventArgs.</param>
		/// <param name="keys">Keys.</param>
		/// <param name="tva">TreeViewAction.</param>
		/// <param name="allowStartEdit">True if node can go to edit mode, false if not.</param>
		private void ProcessNodeRange(TreeNode startNode, TreeNode endNode, MouseEventArgs e, Keys keys, TreeViewAction tva, bool allowStartEdit)
		{
            try
            {
                blnSelectionChanged = false; // prepare for OnSelectionsChanged

                if (e.Button == MouseButtons.Left)
                {
                    blnWasDoubleClick = (intMouseClicks == 2);

                    TreeNode tnTemp = null;
                    int intNodeLevelStart;

                    if (((keys & Keys.Control) == 0) && ((keys & Keys.Shift) == 0))
                    {
                        // CTRL and SHIFT not held down							
                        tnSelectionMirrorPoint = endNode;
                        int intNumberOfSelectedNodes = SelectedNodes.Count;

                        // If it was a double click, select node and suspend further processing					
                        if (blnWasDoubleClick)
                        {
                            base.OnMouseDown(e);
                            return;
                        }

                        if (!IsPlusMinusClicked(endNode, e))
                        {
                            bool blnNodeWasSelected = false;
                            if (IsNodeSelected(endNode))
                                blnNodeWasSelected = true;


                            UnselectAllNodesExceptNode(endNode, tva);
                            SelectNode(endNode, true, tva);


                            if ((blnNodeWasSelected) && (LabelEdit) && (allowStartEdit) && (!blnWasDoubleClick) && (intNumberOfSelectedNodes <= 1))
                            {
                                // Node should be put in edit mode					
                                tnNodeToStartEditOn = endNode;
                                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartEdit));
                                t.Start();
                            }
                        }
                    }
                    else if (((keys & Keys.Control) != 0) && ((keys & Keys.Shift) == 0))
                    {
                        // CTRL held down
                        tnSelectionMirrorPoint = null;

                        if (!IsNodeSelected(endNode))
                        {
                            switch (selectionMode)
                            {
                                case TreeViewSelectionMode.SingleSelect:
                                    UnselectAllNodesExceptNode(endNode, tva);
                                    break;

                                case TreeViewSelectionMode.MultiSelectSameRootBranch:
                                    TreeNode tnAbsoluteParent2 = GetRootParent(endNode);
                                    UnselectAllNodesNotBelongingToParent(tnAbsoluteParent2, tva);
                                    break;

                                case TreeViewSelectionMode.MultiSelectSameLevel:
                                    UnselectAllNodesNotBelongingToLevel(GetNodeLevel(endNode), tva);
                                    break;

                                case TreeViewSelectionMode.MultiSelectSameLevelAndRootBranch:
                                    TreeNode tnAbsoluteParent = GetRootParent(endNode);
                                    UnselectAllNodesNotBelongingToParent(tnAbsoluteParent, tva);
                                    UnselectAllNodesNotBelongingToLevel(GetNodeLevel(endNode), tva);
                                    break;

                                case TreeViewSelectionMode.MultiSelectSameParent:
                                    TreeNode tnParent = endNode.Parent;
                                    UnselectAllNodesNotBelongingDirectlyToParent(tnParent, tva);
                                    break;
                            }

                            SelectNode(endNode, true, tva);
                        }
                        else
                        {
                            SelectNode(endNode, false, tva);
                        }
                    }
                    else if (((keys & Keys.Control) == 0) && ((keys & Keys.Shift) != 0))
                    {
                        // SHIFT pressed  

                        //if startNode is null, we can't select a range, must act as if selection mode is singleSelect
                        if (startNode == null)
                        {
                            UnselectAllNodesExceptNode(endNode, tva);
                            SelectNode(endNode, true, tva);
                        }
                        else
                        {

                            if (tnSelectionMirrorPoint == null)
                            {
                                tnSelectionMirrorPoint = startNode;
                            }

                            switch (selectionMode)
                            {
                                case TreeViewSelectionMode.SingleSelect:
                                    UnselectAllNodesExceptNode(endNode, tva);
                                    SelectNode(endNode, true, tva);
                                    break;

                                case TreeViewSelectionMode.MultiSelectSameRootBranch:
                                    TreeNode tnAbsoluteParentStartNode = GetRootParent(startNode);
                                    tnTemp = startNode;
                                    // Check each visible node from startNode to endNode and select it if needed
                                    while ((tnTemp != null) && (tnTemp != endNode))
                                    {
                                        if (startNode.Bounds.Y > endNode.Bounds.Y)
                                            tnTemp = tnTemp.PrevVisibleNode;
                                        else
                                            tnTemp = tnTemp.NextVisibleNode;
                                        if (tnTemp != null)
                                        {
                                            TreeNode tnAbsoluteParent = GetRootParent(tnTemp);
                                            if (tnAbsoluteParent == tnAbsoluteParentStartNode)
                                            {
                                                SelectNode(tnTemp, true, tva);
                                            }
                                        }
                                    }
                                    UnselectAllNodesNotBelongingToParent(tnAbsoluteParentStartNode, tva);
                                    UnselectNodesOutsideRange(tnSelectionMirrorPoint, endNode, tva);
                                    break;

                                case TreeViewSelectionMode.MultiSelectSameLevel:
                                    intNodeLevelStart = GetNodeLevel(startNode);
                                    tnTemp = startNode;
                                    // Check each visible node from startNode to endNode and select it if needed
                                    while ((tnTemp != null) && (tnTemp != endNode))
                                    {
                                        if (startNode.Bounds.Y > endNode.Bounds.Y)
                                            tnTemp = tnTemp.PrevVisibleNode;
                                        else
                                            tnTemp = tnTemp.NextVisibleNode;
                                        if (tnTemp != null)
                                        {
                                            int intNodeLevel = GetNodeLevel(tnTemp);
                                            if (intNodeLevel == intNodeLevelStart)
                                            {
                                                SelectNode(tnTemp, true, tva);
                                            }
                                        }
                                    }
                                    UnselectAllNodesNotBelongingToLevel(intNodeLevelStart, tva);
                                    UnselectNodesOutsideRange(tnSelectionMirrorPoint, endNode, tva);
                                    break;

                                case TreeViewSelectionMode.MultiSelectSameLevelAndRootBranch:
                                    TreeNode tnAbsoluteParentStart = GetRootParent(startNode);
                                    intNodeLevelStart = GetNodeLevel(startNode);
                                    tnTemp = startNode;
                                    // Check each visible node from startNode to endNode and select it if needed
                                    while ((tnTemp != null) && (tnTemp != endNode))
                                    {
                                        if (startNode.Bounds.Y > endNode.Bounds.Y)
                                            tnTemp = tnTemp.PrevVisibleNode;
                                        else
                                            tnTemp = tnTemp.NextVisibleNode;
                                        if (tnTemp != null)
                                        {
                                            int intNodeLevel = GetNodeLevel(tnTemp);
                                            TreeNode tnAbsoluteParent = GetRootParent(tnTemp);
                                            if ((intNodeLevel == intNodeLevelStart) && (tnAbsoluteParent == tnAbsoluteParentStart))
                                            {
                                                SelectNode(tnTemp, true, tva);
                                            }
                                        }
                                    }
                                    UnselectAllNodesNotBelongingToParent(tnAbsoluteParentStart, tva);
                                    UnselectAllNodesNotBelongingToLevel(intNodeLevelStart, tva);
                                    UnselectNodesOutsideRange(tnSelectionMirrorPoint, endNode, tva);
                                    break;

                                case TreeViewSelectionMode.MultiSelect:
                                    SelectNodesInsideRange(tnSelectionMirrorPoint, endNode, tva);
                                    UnselectNodesOutsideRange(tnSelectionMirrorPoint, endNode, tva);
                                    break;

                                case TreeViewSelectionMode.MultiSelectSameParent:
                                    TreeNode tnParentStartNode = startNode.Parent;
                                    tnTemp = startNode;
                                    // Check each visible node from startNode to endNode and select it if needed
                                    while ((tnTemp != null) && (tnTemp != endNode))
                                    {
                                        if (startNode.Bounds.Y > endNode.Bounds.Y)
                                            tnTemp = tnTemp.PrevVisibleNode;
                                        else
                                            tnTemp = tnTemp.NextVisibleNode;
                                        if (tnTemp != null)
                                        {
                                            TreeNode tnParent = tnTemp.Parent;
                                            if (tnParent == tnParentStartNode)
                                            {
                                                SelectNode(tnTemp, true, tva);
                                            }
                                        }
                                    }
                                    UnselectAllNodesNotBelongingDirectlyToParent(tnParentStartNode, tva);
                                    UnselectNodesOutsideRange(tnSelectionMirrorPoint, endNode, tva);

                                    break;
                            }
                        }
                    }
                    else if (((keys & Keys.Control) != 0) && ((keys & Keys.Shift) != 0))
                    {
                        // SHIFT AND CTRL pressed
                        switch (selectionMode)
                        {
                            case TreeViewSelectionMode.SingleSelect:
                                UnselectAllNodesExceptNode(endNode, tva);
                                SelectNode(endNode, true, tva);
                                break;

                            case TreeViewSelectionMode.MultiSelectSameRootBranch:
                                TreeNode tnAbsoluteParentStartNode = GetRootParent(startNode);
                                tnTemp = startNode;
                                // Check each visible node from startNode to endNode and select it if needed
                                while ((tnTemp != null) && (tnTemp != endNode))
                                {
                                    if (startNode.Bounds.Y > endNode.Bounds.Y)
                                        tnTemp = tnTemp.PrevVisibleNode;
                                    else
                                        tnTemp = tnTemp.NextVisibleNode;
                                    if (tnTemp != null)
                                    {
                                        TreeNode tnAbsoluteParent = GetRootParent(tnTemp);
                                        if (tnAbsoluteParent == tnAbsoluteParentStartNode)
                                        {
                                            SelectNode(tnTemp, true, tva);
                                        }
                                    }
                                }
                                UnselectAllNodesNotBelongingToParent(tnAbsoluteParentStartNode, tva);
                                break;

                            case TreeViewSelectionMode.MultiSelectSameLevel:
                                intNodeLevelStart = GetNodeLevel(startNode);
                                tnTemp = startNode;
                                // Check each visible node from startNode to endNode and select it if needed
                                while ((tnTemp != null) && (tnTemp != endNode))
                                {
                                    if (startNode.Bounds.Y > endNode.Bounds.Y)
                                        tnTemp = tnTemp.PrevVisibleNode;
                                    else
                                        tnTemp = tnTemp.NextVisibleNode;
                                    if (tnTemp != null)
                                    {
                                        int intNodeLevel = GetNodeLevel(tnTemp);
                                        if (intNodeLevel == intNodeLevelStart)
                                        {
                                            SelectNode(tnTemp, true, tva);
                                        }
                                    }
                                }
                                UnselectAllNodesNotBelongingToLevel(intNodeLevelStart, tva);
                                break;

                            case TreeViewSelectionMode.MultiSelectSameLevelAndRootBranch:
                                TreeNode tnAbsoluteParentStart = GetRootParent(startNode);
                                intNodeLevelStart = GetNodeLevel(startNode);
                                tnTemp = startNode;
                                // Check each visible node from startNode to endNode and select it if needed
                                while ((tnTemp != null) && (tnTemp != endNode))
                                {
                                    if (startNode.Bounds.Y > endNode.Bounds.Y)
                                        tnTemp = tnTemp.PrevVisibleNode;
                                    else
                                        tnTemp = tnTemp.NextVisibleNode;
                                    if (tnTemp != null)
                                    {
                                        int intNodeLevel = GetNodeLevel(tnTemp);
                                        TreeNode tnAbsoluteParent = GetRootParent(tnTemp);
                                        if ((intNodeLevel == intNodeLevelStart) && (tnAbsoluteParent == tnAbsoluteParentStart))
                                        {
                                            SelectNode(tnTemp, true, tva);
                                        }
                                    }
                                }
                                UnselectAllNodesNotBelongingToParent(tnAbsoluteParentStart, tva);
                                UnselectAllNodesNotBelongingToLevel(intNodeLevelStart, tva);
                                break;

                            case TreeViewSelectionMode.MultiSelect:
                                tnTemp = startNode;
                                // Check each visible node from startNode to endNode and select it if needed
                                while ((tnTemp != null) && (tnTemp != endNode))
                                {
                                    if (startNode.Bounds.Y > endNode.Bounds.Y)
                                        tnTemp = tnTemp.PrevVisibleNode;
                                    else
                                        tnTemp = tnTemp.NextVisibleNode;
                                    if (tnTemp != null)
                                    {
                                        SelectNode(tnTemp, true, tva);
                                    }
                                }
                                break;

                            case TreeViewSelectionMode.MultiSelectSameParent:
                                TreeNode tnParentStartNode = startNode.Parent;
                                tnTemp = startNode;
                                // Check each visible node from startNode to endNode and select it if needed
                                while ((tnTemp != null) && (tnTemp != endNode))
                                {
                                    if (startNode.Bounds.Y > endNode.Bounds.Y)
                                        tnTemp = tnTemp.PrevVisibleNode;
                                    else
                                        tnTemp = tnTemp.NextVisibleNode;
                                    if (tnTemp != null)
                                    {
                                        TreeNode tnParent = tnTemp.Parent;
                                        if (tnParent == tnParentStartNode)
                                        {
                                            SelectNode(tnTemp, true, tva);
                                        }
                                    }
                                }
                                UnselectAllNodesNotBelongingDirectlyToParent(tnParentStartNode, tva);
                                break;
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    // if right mouse button clicked, clear selection and select right-clicked node
                    if (!IsNodeSelected(endNode))
                    {
                        UnselectAllNodes(tva);
                        SelectNode(endNode, true, tva);
                    }
                }
                OnSelectionsChanged();
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region OnBeforeLabelEdit

		/// <summary>
		/// Occurs before node goes into edit mode.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnBeforeLabelEdit(NodeLabelEditEventArgs e)
		{
            try
            {
                blnSelectionChanged = false; // prepare for OnSelectionsChanged

                // Make sure that it's the only selected node
                SelectNode(e.Node, true, TreeViewAction.ByMouse);
                UnselectAllNodesExceptNode(e.Node, TreeViewAction.ByMouse);

                OnSelectionsChanged();

                base.OnBeforeLabelEdit(e);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region OnKeyDown

		/// <summary>
		/// occurs when a key is down.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
            try
            {
                Keys kMod = Keys.None;
                switch (e.Modifiers)
                {
                    case Keys.Shift:
                    case Keys.Control:
                    case Keys.Control | Keys.Shift:
                        kMod = Keys.Shift;
                        if (tnKeysStartNode == null)
                            tnKeysStartNode = tnMostRecentSelectedNode;
                        break;
                    default:
                        tnKeysStartNode = null;
                        break;
                }

                int intNumber = 0;

                TreeNode tnNewlySelectedNodeWithKeys = null;
                switch (e.KeyCode)
                {
                    case Keys.Down:
                        tnNewlySelectedNodeWithKeys = tnMostRecentSelectedNode.NextVisibleNode;
                        break;

                    case Keys.Up:
                        tnNewlySelectedNodeWithKeys = tnMostRecentSelectedNode.PrevVisibleNode;
                        break;

                    case Keys.Left:
                        if (tnMostRecentSelectedNode.IsExpanded)
                            tnMostRecentSelectedNode.Collapse();
                        else
                            tnNewlySelectedNodeWithKeys = tnMostRecentSelectedNode.Parent;
                        break;

                    case Keys.Right:
                        if (!tnMostRecentSelectedNode.IsExpanded)
                            tnMostRecentSelectedNode.Expand();
                        else
                            if (tnMostRecentSelectedNode.Nodes != null)
                                tnNewlySelectedNodeWithKeys = tnMostRecentSelectedNode.Nodes[0];
                        break;

                    case Keys.Home:
                        tnNewlySelectedNodeWithKeys = this.Nodes[0];
                        break;

                    case Keys.End:
                        tnNewlySelectedNodeWithKeys = GetLastVisibleNode();
                        break;

                    case Keys.PageDown:

                        intNumber = GetNumberOfVisibleNodes();
                        tnNewlySelectedNodeWithKeys = GetNextTreeNode(tnMostRecentSelectedNode, true, intNumber);
                        break;

                    case Keys.PageUp:

                        intNumber = GetNumberOfVisibleNodes();
                        tnNewlySelectedNodeWithKeys = GetNextTreeNode(tnMostRecentSelectedNode, false, intNumber);
                        break;

                    default:
                        base.OnKeyDown(e); // GKM
                        return;
                }

                if ((tnNewlySelectedNodeWithKeys != null))
                {
                    SetFocusToNode(tnMostRecentSelectedNode, false);
                    ProcessNodeRange(tnKeysStartNode, tnNewlySelectedNodeWithKeys, new MouseEventArgs(MouseButtons.Left, 1, Cursor.Position.X, Cursor.Position.Y, 0), kMod, TreeViewAction.ByKeyboard, false);
                    tnMostRecentSelectedNode = tnNewlySelectedNodeWithKeys;
                    SetFocusToNode(tnMostRecentSelectedNode, true);
                }

                // Ensure visibility
                if (tnMostRecentSelectedNode != null)
                {
                    TreeNode tnToMakeVisible = null;
                    switch (e.KeyCode)
                    {
                        case Keys.Down:
                        case Keys.Right:
                            tnToMakeVisible = GetNextTreeNode(tnMostRecentSelectedNode, true, 5);
                            break;

                        case Keys.Up:
                        case Keys.Left:
                            tnToMakeVisible = GetNextTreeNode(tnMostRecentSelectedNode, false, 5);
                            break;

                        case Keys.Home:
                        case Keys.End:
                            tnToMakeVisible = tnMostRecentSelectedNode;
                            break;

                        case Keys.PageDown:
                            tnToMakeVisible = GetNextTreeNode(tnMostRecentSelectedNode, true, intNumber - 2);
                            break;

                        case Keys.PageUp:
                            tnToMakeVisible = GetNextTreeNode(tnMostRecentSelectedNode, false, intNumber - 2);
                            break;
                    }

                    if (tnToMakeVisible != null)
                        tnToMakeVisible.EnsureVisible();
                }

                base.OnKeyDown(e);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region OnAfterCollapse

		/// <summary>
		/// Occurs after a node is collapsed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnAfterCollapse(TreeViewEventArgs e)
		{
            try
            {
                blnSelectionChanged = false;

                // All child nodes should be deselected
                bool blnChildSelected = false;
                foreach (TreeNode tn in e.Node.Nodes)
                {
                    if (IsNodeSelected(tn))
                    {
                        blnChildSelected = true;
                    }
                    UnselectNodesRecursively(tn, TreeViewAction.Collapse);
                }

                if (blnChildSelected)
                {
                    SelectNode(e.Node, true, TreeViewAction.Collapse);
                }

                OnSelectionsChanged();

                base.OnAfterCollapse(e);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion

        //=========================================================================
		#region OnItemDrag

		/// <summary>
		/// Occurs when an item is being dragged.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnItemDrag(ItemDragEventArgs e)
		{
            try
            {
                e = new ItemDragEventArgs(MouseButtons.Left, this.SelectedNodes);
                base.OnItemDrag(e);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion
	}

    //*****************************************************************************
	#region SelectedNodesCollection

	/// <summary>
	/// Collection of selected nodes.
	/// </summary>
	public class NodesCollection : CollectionBase
	{
        //=========================================================================
		#region Events

		/// <summary>
		/// Event fired when a tree node has been added to the collection.
		/// </summary>
		internal event TreeNodeEventHandler TreeNodeAdded;

		/// <summary>
		/// Event fired when a tree node has been removed to the collection.
		/// </summary>
		internal event TreeNodeEventHandler TreeNodeRemoved;

		/// <summary>
		/// Event fired when a tree node has been inserted to the collection.
		/// </summary>
		internal event TreeNodeEventHandler TreeNodeInserted;

		/// <summary>
		/// Event fired the collection has been cleared.
		/// </summary>
		internal event EventHandler SelectedNodesCleared;

		#endregion

        //=========================================================================
		#region CollectionBase members

		/// <summary>
		/// Gets tree node at specified index.
		/// </summary>
		public TreeNode this[int index]
		{
			get { return ((TreeNode)List[index]); }
		}

		/// <summary>
		/// Adds a tree node to the collection.
		/// </summary>
		/// <param name="treeNode">Tree node to add.</param>
		/// <returns>The position into which the new element was inserted.</returns>
		public int Add(TreeNode treeNode)
		{
			if (TreeNodeAdded != null)
				TreeNodeAdded(treeNode);

			return List.Add(treeNode);
		}

		/// <summary>
		/// Inserts a tree node at specified index.
		/// </summary>
		/// <param name="index">The position into which the new element has to be inserted.</param>
		/// <param name="treeNode">Tree node to insert.</param>
		public void Insert(int index, TreeNode treeNode)
		{
            try
            {
                if (TreeNodeInserted != null)
                    TreeNodeInserted(treeNode);

                List.Add(treeNode);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Removed a tree node from the collection.
		/// </summary>
		/// <param name="treeNode">Tree node to remove.</param>
		public void Remove(TreeNode treeNode)
		{
            try
            {
                if (TreeNodeRemoved != null)
                    TreeNodeRemoved(treeNode);

                List.Remove(treeNode);
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		/// <summary>
		/// Determines whether treenode belongs to the collection.
		/// </summary>
		/// <param name="treeNode">Tree node to check.</param>
		/// <returns>True if tree node belongs to the collection, false if not.</returns>
		public bool Contains(TreeNode treeNode)
		{
			return List.Contains(treeNode);
		}

		/// <summary>
		/// Gets index of tree node in the collection.
		/// </summary>
		/// <param name="treeNode">Tree node to get index of.</param>
		/// <returns>Index of tree node in the collection.</returns>
		public int IndexOf(TreeNode treeNode)
		{
			return List.IndexOf(treeNode);
		}

		#endregion

        //=========================================================================
		#region OnClear

		/// <summary>
		/// Occurs when collection is being cleared.
		/// </summary>
		protected override void OnClear()
		{
            try
            {
                if (SelectedNodesCleared != null)
                    SelectedNodesCleared(this, EventArgs.Empty);

                base.OnClear();
            }
            catch (Exception ex)
            { System.Diagnostics.Debug.WriteLine(ex.ToString()); }
		}

		#endregion
	}

	#endregion
}
