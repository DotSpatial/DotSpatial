// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Collections;
using System.IO;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Planargraph
{
    /// <summary>
    /// Represents a directed edge in a <c>PlanarGraph</c>. A DirectedEdge may or
    /// may not have a reference to a parent Edge (some applications of
    /// planar graphs may not require explicit Edge objects to be created). Usually
    /// a client using a <c>PlanarGraph</c> will subclass <c>DirectedEdge</c>
    /// to add its own application-specific data and methods.
    /// </summary>
    public class DirectedEdge : GraphComponent, IComparable
    {
        #region Private Variables

        private readonly double _angle;
        private readonly bool _edgeDirection;
        private readonly Node _from;
        private readonly Coordinate _p0;
        private readonly Coordinate _p1;
        private readonly int _quadrant;
        private readonly Node _to;
        private Edge _parentEdge;
        private DirectedEdge _sym;  // optional

        #endregion

        /// <summary>
        /// Constructs a DirectedEdge connecting the <c>from</c> node to the
        /// <c>to</c> node.
        /// </summary>
        /// <param name="inFrom"></param>
        /// <param name="inTo"></param>
        /// <param name="directionPt">
        /// Specifies this DirectedEdge's direction (given by an imaginary
        /// line from the <c>from</c> node to <c>directionPt</c>).
        /// </param>
        /// <param name="inEdgeDirection">
        /// Whether this DirectedEdge's direction is the same as or
        /// opposite to that of the parent Edge (if any).
        /// </param>
        public DirectedEdge(Node inFrom, Node inTo, Coordinate directionPt, bool inEdgeDirection)
        {
            _from = inFrom;
            _to = inTo;
            _edgeDirection = inEdgeDirection;
            _p0 = _from.Coordinate;
            _p1 = directionPt;
            double dx = _p1.X - _p0.X;
            double dy = _p1.Y - _p0.Y;
            _quadrant = QuadrantOp.Quadrant(dx, dy);
            _angle = Math.Atan2(dy, dx);
        }

        /// <summary>
        /// Returns this DirectedEdge's parent Edge, or null if it has none.
        /// Associates this DirectedEdge with an Edge (possibly null, indicating no associated
        /// Edge).
        /// </summary>
        public virtual Edge Edge
        {
            get
            {
                return _parentEdge;
            }
            set
            {
                _parentEdge = value;
            }
        }

        /// <summary>
        /// Returns 0, 1, 2, or 3, indicating the quadrant in which this DirectedEdge's
        /// orientation lies.
        /// </summary>
        public virtual int Quadrant
        {
            get
            {
                return _quadrant;
            }
        }

        /// <summary>
        /// returns a point representing the starting point for a line being drawn
        /// in order to indicate the directed edges vector direction.
        /// </summary>
        public virtual Coordinate StartPoint
        {
            get
            {
                return _p0;
            }
        }

        /// <summary>
        /// Returns a point to which an imaginary line is drawn from the from-node to
        /// specify this DirectedEdge's orientation.
        /// </summary>
        public virtual Coordinate EndPoint
        {
            get
            {
                return _p1;
            }
        }

        /// <summary>
        /// Returns whether the direction of the parent Edge (if any) is the same as that
        /// of this Directed Edge.
        /// </summary>
        public virtual bool EdgeDirection
        {
            get
            {
                return _edgeDirection;
            }
        }

        /// <summary>
        /// Returns the node from which this DirectedEdge leaves.
        /// </summary>
        public virtual Node FromNode
        {
            get
            {
                return _from;
            }
        }

        /// <summary>
        /// Returns the node to which this DirectedEdge goes.
        /// </summary>
        public virtual Node ToNode
        {
            get
            {
                return _to;
            }
        }

        /// <summary>
        /// Returns the coordinate of the from-node.
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                return _from.Coordinate;
            }
        }

        /// <summary>
        /// Returns the angle that the start of this DirectedEdge makes with the
        /// positive x-axis, in radians.
        /// </summary>
        public virtual double Angle
        {
            get
            {
                return _angle;
            }
        }

        /// <summary>
        /// Returns the symmetric DirectedEdge -- the other DirectedEdge associated with
        /// this DirectedEdge's parent Edge.
        /// Sets this DirectedEdge's symmetric DirectedEdge, which runs in the opposite
        /// direction.
        /// </summary>
        public virtual DirectedEdge Sym
        {
            get
            {
                return _sym;
            }
            set
            {
                _sym = value;
            }
        }

        /// <summary>
        /// Tests whether this component has been removed from its containing graph.
        /// </summary>
        /// <value></value>
        public override bool IsRemoved
        {
            get
            {
                return _parentEdge == null;
            }
        }

        #region IComparable Members

        /// <summary>
        /// Returns 1 if this DirectedEdge has a greater angle with the
        /// positive x-axis than b", 0 if the DirectedEdges are collinear, and -1 otherwise.
        /// Using the obvious algorithm of simply computing the angle is not robust,
        /// since the angle calculation is susceptible to roundoff. A robust algorithm
        /// is:
        /// first compare the quadrants. If the quadrants are different, it it
        /// trivial to determine which vector is "greater".
        /// if the vectors lie in the same quadrant, the robust
        /// <c>RobustCgAlgorithms.ComputeOrientation(Coordinate, Coordinate, Coordinate)</c>
        /// function can be used to decide the relative orientation of the vectors.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int CompareTo(Object obj)
        {
            DirectedEdge de = (DirectedEdge)obj;
            return CompareDirection(de);
        }

        #endregion

        /// <summary>
        /// Returns a List containing the parent Edge (possibly null) for each of the given
        /// DirectedEdges.
        /// </summary>
        /// <param name="dirEdges"></param>
        /// <returns></returns>
        public static IList ToEdges(IList dirEdges)
        {
            IList edges = new ArrayList();
            for (IEnumerator i = dirEdges.GetEnumerator(); i.MoveNext(); )
            {
                edges.Add(((DirectedEdge)i.Current).Edge);
            }
            return edges;
        }

        /// <summary>
        /// Returns 1 if this DirectedEdge has a greater angle with the
        /// positive x-axis than b", 0 if the DirectedEdges are collinear, and -1 otherwise.
        /// Using the obvious algorithm of simply computing the angle is not robust,
        /// since the angle calculation is susceptible to roundoff. A robust algorithm
        /// is:
        /// first compare the quadrants. If the quadrants are different, it it
        /// trivial to determine which vector is "greater".
        /// if the vectors lie in the same quadrant, the robust
        /// <c>RobustCgAlgorithms.ComputeOrientation(Coordinate, Coordinate, Coordinate)</c>
        /// function can be used to decide the relative orientation of the vectors.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual int CompareDirection(DirectedEdge e)
        {
            // if the rays are in different quadrants, determining the ordering is trivial
            if (_quadrant > e.Quadrant)
                return 1;
            if (_quadrant < e.Quadrant)
                return -1;
            // vectors are in the same quadrant - check relative orientation of direction vectors
            // this is > e if it is CCW of e
            int i = CgAlgorithms.ComputeOrientation(e.StartPoint, e.EndPoint, _p1);
            return i;
        }

        /// <summary>
        /// Writes a detailed string representation of this DirectedEdge to the given PrintStream.
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void Write(StreamWriter outstream)
        {
            string className = GetType().FullName;
            int lastDotPos = className.LastIndexOf('.');
            string name = className.Substring(lastDotPos + 1);
            outstream.Write("  " + name + ": " + _p0 + " - " + _p1 + " " + _quadrant + ":" + _angle);
        }

        /// <summary>
        /// Removes this directed edge from its containing graph.
        /// </summary>
        internal void Remove()
        {
            _sym = null;
            _parentEdge = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "DirectedEdge: " + _p0 + " - " + _p1 + " " + _quadrant + ":" + _angle;
        }
    }
}