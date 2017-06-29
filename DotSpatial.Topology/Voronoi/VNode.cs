// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/27/2009 4:55:32 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Name              |   Date             |   Comments
// ------------------|--------------------|---------------------------------------------------------
// Benjamin Dittes   | August 10, 2005    |  Authored original code for working with laser data
// Ted Dunsford      | August 26, 2009    |  Ported and cleaned up the raw source from code project
// ********************************************************************************************************

using System;

namespace DotSpatial.Topology.Voronoi
{
    internal abstract class VNode
    {
        private VNode _left;
        private VNode _parent;
        private VNode _right;

        private VNode Left
        {
            get { return _left; }
            set
            {
                _left = value;
                value.Parent = this;
            }
        }

        private VNode Right
        {
            get { return _right; }
            set
            {
                _right = value;
                value.Parent = this;
            }
        }

        private VNode Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        private void Replace(VNode childOld, VNode childNew)
        {
            if (Left == childOld)
                Left = childNew;
            else if (Right == childOld)
                Right = childNew;
            else throw new Exception("Child not found!");
            childOld.Parent = null;
        }

        private static VDataNode LeftDataNode(VNode current)
        {
            VNode c = current;
            //1. Up
            do
            {
                if (c.Parent == null)
                    return null;
                if (c.Parent.Left == c)
                {
                    c = c.Parent;
                    continue;
                }
                c = c.Parent;
                break;
            } while (true);
            //2. One Left
            c = c.Left;
            //3. Down
            while (c.Right != null)
                c = c.Right;
            return (VDataNode)c; // Cast statt 'as' damit eine Exception kommt
        }

        private static VDataNode RightDataNode(VNode current)
        {
            VNode c = current;
            //1. Up
            do
            {
                if (c.Parent == null)
                    return null;
                if (c.Parent.Right == c)
                {
                    c = c.Parent;
                    continue;
                }
                c = c.Parent;
                break;
            } while (true);
            //2. One Right
            c = c.Right;
            //3. Down
            while (c.Left != null)
                c = c.Left;
            return (VDataNode)c; // Cast statt 'as' damit eine Exception kommt
        }

        private static VEdgeNode EdgeToRightDataNode(VNode current)
        {
            VNode c = current;
            //1. Up
            do
            {
                if (c.Parent == null)
                    throw new Exception("No Left Leaf found!");
                if (c.Parent.Right == c)
                {
                    c = c.Parent;
                    continue;
                }
                c = c.Parent;
                break;
            } while (true);
            return (VEdgeNode)c;
        }

        private static VDataNode FindDataNode(VNode root, double ys, double x)
        {
            VNode c = root;
            do
            {
                if (c is VDataNode)
                    return (VDataNode)c;
                if (((VEdgeNode)c).Cut(ys, x) < 0)
                    c = c.Left;
                else
                    c = c.Right;
            } while (true);
        }

        /// <summary>
        /// Will return the new root (unchanged except in start-up)
        /// </summary>
        public static VNode ProcessDataEvent(VDataEvent e, VNode root, VoronoiGraph vg, double ys, out VDataNode[] circleCheckList)
        {
            if (root == null)
            {
                root = new VDataNode(e.DataPoint);
                circleCheckList = new[] { (VDataNode)root };
                return root;
            }
            //1. Find the node to be replaced
            VNode c = FindDataNode(root, ys, e.DataPoint.X);
            //2. Create the subtree (ONE Edge, but two VEdgeNodes)
            VoronoiEdge ve = new VoronoiEdge();
            ve.LeftData = ((VDataNode)c).DataPoint;
            ve.RightData = e.DataPoint;
            ve.VVertexA = Fortune.VVUnkown;
            ve.VVertexB = Fortune.VVUnkown;
            vg.Edges.Add(ve);

            VNode subRoot;
            if (Math.Abs(ve.LeftData.Y - ve.RightData.Y) < 1e-10)
            {
                if (ve.LeftData.X < ve.RightData.X)
                {
                    subRoot = new VEdgeNode(ve, false);
                    subRoot.Left = new VDataNode(ve.LeftData);
                    subRoot.Right = new VDataNode(ve.RightData);
                }
                else
                {
                    subRoot = new VEdgeNode(ve, true);
                    subRoot.Left = new VDataNode(ve.RightData);
                    subRoot.Right = new VDataNode(ve.LeftData);
                }
                circleCheckList = new[] { (VDataNode)subRoot.Left, (VDataNode)subRoot.Right };
            }
            else
            {
                subRoot = new VEdgeNode(ve, false);
                subRoot.Left = new VDataNode(ve.LeftData);
                subRoot.Right = new VEdgeNode(ve, true);
                subRoot.Right.Left = new VDataNode(ve.RightData);
                subRoot.Right.Right = new VDataNode(ve.LeftData);
                circleCheckList = new[] { (VDataNode)subRoot.Left, (VDataNode)subRoot.Right.Left, (VDataNode)subRoot.Right.Right };
            }

            //3. Apply subtree
            if (c.Parent == null)
                return subRoot;
            c.Parent.Replace(c, subRoot);
            return root;
        }

        public static VNode ProcessCircleEvent(VCircleEvent e, VNode root, VoronoiGraph vg, out VDataNode[] circleCheckList)
        {
            VEdgeNode eo;
            VDataNode b = e.NodeN;
            VDataNode a = LeftDataNode(b);
            VDataNode c = RightDataNode(b);
            if (a == null || b.Parent == null || c == null || !a.DataPoint.Equals(e.NodeL.DataPoint) || !c.DataPoint.Equals(e.NodeR.DataPoint))
            {
                circleCheckList = new VDataNode[] { };
                return root; // Abbruch da sich der Graph verändert hat
            }
            VEdgeNode eu = (VEdgeNode)b.Parent;
            circleCheckList = new[] { a, c };
            //1. Create the new Vertex
            Vector2 vNew = new Vector2(e.Center.X, e.Center.Y);
            //			VNew[0] = Fortune.ParabolicCut(a.DataPoint[0], a.DataPoint[1], c.DataPoint[0], c.DataPoint[1], ys);
            //			VNew[1] = (ys + a.DataPoint[1])/2 - 1/(2*(ys-a.DataPoint[1]))*(VNew[0]-a.DataPoint[0])*(VNew[0]-a.DataPoint[0]);
            vg.Vertices.Add(vNew);
            //2. Find out if a or c are in a distand part of the tree (the other is then b's sibling) and assign the new vertex
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
            //			///////////////////// uncertain
            //			if (eo==eu)
            //				return root;
            //			/////////////////////
            eo.Edge.AddVertex(vNew);
            //2. Replace eo by new Edge
            VoronoiEdge ve = new VoronoiEdge();
            ve.LeftData = a.DataPoint;
            ve.RightData = c.DataPoint;
            ve.AddVertex(vNew);
            vg.Edges.Add(ve);

            VEdgeNode ven = new VEdgeNode(ve, false);
            ven.Left = eo.Left;
            ven.Right = eo.Right;
            if (eo.Parent == null)
                return ven;
            eo.Parent.Replace(eo, ven);
            return root;
        }

        public static VCircleEvent CircleCheckDataNode(VDataNode n, double ys)
        {
            VDataNode l = LeftDataNode(n);
            VDataNode r = RightDataNode(n);
            if (l == null || r == null || l.DataPoint == r.DataPoint || l.DataPoint == n.DataPoint || n.DataPoint == r.DataPoint)
                return null;
            if (MathTools.Ccw(l.DataPoint.X, l.DataPoint.Y, n.DataPoint.X, n.DataPoint.Y, r.DataPoint.X, r.DataPoint.Y, false) <= 0)
                return null;
            Vector2 center = Fortune.CircumCircleCenter(l.DataPoint, n.DataPoint, r.DataPoint);
            VCircleEvent vc = new VCircleEvent();
            vc.NodeN = n;
            vc.NodeL = l;
            vc.NodeR = r;
            vc.Center = center;
            vc.Valid = true;
            if (vc.Y >= ys)
                return vc;
            return null;
        }

        public static void CleanUpTree(VNode root)
        {
            if (root is VDataNode)
                return;
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
    }

    internal class VDataNode : VNode
    {
        public readonly Vector2 DataPoint;

        public VDataNode(Vector2 dp)
        {
            DataPoint = dp;
        }
    }

    internal class VEdgeNode : VNode
    {
        public readonly VoronoiEdge Edge;
        public readonly bool Flipped;

        public VEdgeNode(VoronoiEdge e, bool flipped)
        {
            Edge = e;
            Flipped = flipped;
        }

        public double Cut(double ys, double x)
        {
            if (!Flipped)
                return x - Fortune.ParabolicCut(Edge.LeftData.X, Edge.LeftData.Y, Edge.RightData.X, Edge.RightData.Y, ys);
            return x - Fortune.ParabolicCut(Edge.RightData.X, Edge.RightData.Y, Edge.LeftData.X, Edge.LeftData.Y, ys);
        }
    }
}