// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.NTSExtension.Voronoi
{
    /// <summary>
    /// The VNode.
    /// </summary>
    internal abstract class VNode
    {
        #region Fields

        private VNode _left;

        private VNode _right;

        #endregion

        #region Properties

        private VNode Left
        {
            get
            {
                return _left;
            }

            set
            {
                _left = value;
                value.Parent = this;
            }
        }

        private VNode Parent { get; set; }

        private VNode Right
        {
            get
            {
                return _right;
            }

            set
            {
                _right = value;
                value.Parent = this;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks the VDataNode for circles.
        /// </summary>
        /// <param name="n">The VDataNode.</param>
        /// <param name="ys">The ys.</param>
        /// <returns>The resulting VCircleEvent.</returns>
        public static VCircleEvent CircleCheckDataNode(VDataNode n, double ys)
        {
            VDataNode l = LeftDataNode(n);
            VDataNode r = RightDataNode(n);
            if (l == null || r == null || l.DataPoint == r.DataPoint || l.DataPoint == n.DataPoint || n.DataPoint == r.DataPoint) return null;
            if (MathTools.Ccw(l.DataPoint.X, l.DataPoint.Y, n.DataPoint.X, n.DataPoint.Y, r.DataPoint.X, r.DataPoint.Y, false) <= 0) return null;
            Vector2 center = Fortune.CircumCircleCenter(l.DataPoint, n.DataPoint, r.DataPoint);
            VCircleEvent vc = new VCircleEvent
            {
                NodeN = n,
                NodeL = l,
                NodeR = r,
                Center = center,
                Valid = true
            };
            return vc.Y >= ys ? vc : null;
        }

        /// <summary>
        /// Cleans the tree.
        /// </summary>
        /// <param name="root">The root node.</param>
        public static void CleanUpTree(VNode root)
        {
            if (root is VDataNode) return;
            VEdgeNode ve = root as VEdgeNode;
            if (ve != null)
            {
                while (ve.Edge.VVertexB == Fortune.VVUnkown)
                {
                    ve.Edge.AddVertex(Fortune.VVInfinite);
                }

                if (ve.Flipped)
                {
                    Vector2 t = ve.Edge.LeftData;
                    ve.Edge.LeftData = ve.Edge.RightData;
                    ve.Edge.RightData = t;
                }

                ve.Edge.Done = true;
            }

            CleanUpTree(root.Left);
            CleanUpTree(root.Right);
        }

        /// <summary>
        /// Processes the VCircleEvent.
        /// </summary>
        /// <param name="e">The VCircleEvent.</param>
        /// <param name="root">The root node.</param>
        /// <param name="vg">The VoronoiGraph.</param>
        /// <param name="circleCheckList">The circle check list.</param>
        /// <returns>The resulting root.</returns>
        public static VNode ProcessCircleEvent(VCircleEvent e, VNode root, VoronoiGraph vg, out VDataNode[] circleCheckList)
        {
            VEdgeNode eo;
            VDataNode b = e.NodeN;
            VDataNode a = LeftDataNode(b);
            VDataNode c = RightDataNode(b);
            if (a == null || b.Parent == null || c == null || !a.DataPoint.Equals(e.NodeL.DataPoint) || !c.DataPoint.Equals(e.NodeR.DataPoint))
            {
                circleCheckList = new VDataNode[] { };
                return root; // abbort because graph changed
            }

            VEdgeNode eu = (VEdgeNode)b.Parent;
            circleCheckList = new[] { a, c };

            // 1. Create the new Vertex
            Vector2 vNew = new Vector2(e.Center.X, e.Center.Y);

            vg.Vertices.Add(vNew);

            // 2. Find out if a or c are in a distand part of the tree (the other is then b's sibling) and assign the new vertex
            if (eu.Left == b)
            {
                // c is sibling
                eo = EdgeToRightDataNode(a);

                // replace eu by eu's Right
                eu.Parent.Replace(eu, eu.Right);
            }
            else
            {
                // a is sibling
                eo = EdgeToRightDataNode(b);

                // replace eu by eu's Left
                eu.Parent.Replace(eu, eu.Left);
            }

            eu.Edge.AddVertex(vNew);
            eo.Edge.AddVertex(vNew);

            // 2. Replace eo by new Edge
            VoronoiEdge ve = new VoronoiEdge { LeftData = a.DataPoint, RightData = c.DataPoint };
            ve.AddVertex(vNew);
            vg.Edges.Add(ve);

            VEdgeNode ven = new VEdgeNode(ve, false) { Left = eo.Left, Right = eo.Right };
            if (eo.Parent == null) return ven;
            eo.Parent.Replace(eo, ven);
            return root;
        }

        /// <summary>
        /// Will return the new root (unchanged except in start-up).
        /// </summary>
        /// <param name="e">The VDataEvent.</param>
        /// <param name="root">The root node.</param>
        /// <param name="vg">The VoronoiGraph.</param>
        /// <param name="ys">The ys.</param>
        /// <param name="circleCheckList">The circle check list.</param>
        /// <returns>The new root.</returns>
        public static VNode ProcessDataEvent(VDataEvent e, VNode root, VoronoiGraph vg, double ys, out VDataNode[] circleCheckList)
        {
            if (root == null)
            {
                root = new VDataNode(e.DataPoint);
                circleCheckList = new[] { (VDataNode)root };
                return root;
            }

            // 1. Find the node to be replaced
            VNode c = FindDataNode(root, ys, e.DataPoint.X);

            // 2. Create the subtree (ONE Edge, but two VEdgeNodes)
            VoronoiEdge ve = new VoronoiEdge { LeftData = ((VDataNode)c).DataPoint, RightData = e.DataPoint, VVertexA = Fortune.VVUnkown, VVertexB = Fortune.VVUnkown };
            vg.Edges.Add(ve);

            VNode subRoot;
            if (Math.Abs(ve.LeftData.Y - ve.RightData.Y) < 1e-10)
            {
                if (ve.LeftData.X < ve.RightData.X)
                {
                    subRoot = new VEdgeNode(ve, false) { Left = new VDataNode(ve.LeftData), Right = new VDataNode(ve.RightData) };
                }
                else
                {
                    subRoot = new VEdgeNode(ve, true) { Left = new VDataNode(ve.RightData), Right = new VDataNode(ve.LeftData) };
                }

                circleCheckList = new[] { (VDataNode)subRoot.Left, (VDataNode)subRoot.Right };
            }
            else
            {
                subRoot = new VEdgeNode(ve, false) { Left = new VDataNode(ve.LeftData), Right = new VEdgeNode(ve, true) { Left = new VDataNode(ve.RightData), Right = new VDataNode(ve.LeftData) } };
                circleCheckList = new[] { (VDataNode)subRoot.Left, (VDataNode)subRoot.Right.Left, (VDataNode)subRoot.Right.Right };
            }

            // 3. Apply subtree
            if (c.Parent == null) return subRoot;
            c.Parent.Replace(c, subRoot);
            return root;
        }

        private static VEdgeNode EdgeToRightDataNode(VNode current)
        {
            VNode c = current;

            // 1. Up
            do
            {
                if (c.Parent == null) throw new Exception("No Left Leaf found!");
                if (c.Parent.Right == c)
                {
                    c = c.Parent;
                    continue;
                }

                c = c.Parent;
                break;
            }
            while (true);
            return (VEdgeNode)c;
        }

        private static VDataNode FindDataNode(VNode root, double ys, double x)
        {
            VNode c = root;
            do
            {
                var node = c as VDataNode;
                if (node != null) return node;
                c = ((VEdgeNode)c).Cut(ys, x) < 0 ? c.Left : c.Right;
            }
            while (true);
        }

        private static VDataNode LeftDataNode(VNode current)
        {
            VNode c = current;

            // 1. Up
            do
            {
                if (c.Parent == null) return null;
                if (c.Parent.Left == c)
                {
                    c = c.Parent;
                    continue;
                }

                c = c.Parent;
                break;
            }
            while (true);

            // 2. One Left
            c = c.Left;

            // 3. Down
            while (c.Right != null) c = c.Right;
            return (VDataNode)c; // Cast instead of 'as' to cause an exception
        }

        private static VDataNode RightDataNode(VNode current)
        {
            VNode c = current;

            // 1. Up
            do
            {
                if (c.Parent == null) return null;
                if (c.Parent.Right == c)
                {
                    c = c.Parent;
                    continue;
                }

                c = c.Parent;
                break;
            }
            while (true);

            // 2. One Right
            c = c.Right;

            // 3. Down
            while (c.Left != null) c = c.Left;
            return (VDataNode)c; // Cast instead of 'as' to cause an exception
        }

        private void Replace(VNode childOld, VNode childNew)
        {
            if (Left == childOld) Left = childNew;
            else if (Right == childOld) Right = childNew;
            else throw new Exception("Child not found!");
            childOld.Parent = null;
        }

        #endregion
    }
}