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
using System.IO;
using System.Text;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.GeometriesGraph
{
    /// <summary>
    /// Models the end of an edge incident on a node.
    /// EdgeEnds have a direction
    /// determined by the direction of the ray from the initial
    /// point to the next point.
    /// EdgeEnds are IComparable under the ordering
    /// "a has a greater angle with the x-axis than b".
    /// This ordering is used to sort EdgeEnds around a node.
    /// </summary>
    public class EdgeEnd : IComparable
    {
        private double _dx, _dy;      // the direction vector for this edge from its starting point

        /// <summary>
        /// The parent edge of this edge end.
        /// </summary>
        private Edge _edge;
        private Label _label;
        private Node _node;          // the node this edge end originates at
        private Coordinate _p0, _p1;  // points of initial line segment
        private int _quadrant;

        /// <summary>
        ///
        /// </summary>
        /// <param name="inEdge"></param>
        protected EdgeEnd(Edge inEdge)
        {
            _edge = inEdge;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        public EdgeEnd(Edge edge, Coordinate p0, Coordinate p1) : this(edge, p0, p1, null) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="inLabel"></param>
        public EdgeEnd(Edge edge, Coordinate p0, Coordinate p1, Label inLabel)
            : this(edge)
        {
            DoInit(p0, p1);
            _label = inLabel;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Edge Edge
        {
            get { return _edge; }
            protected set { _edge = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Label Label
        {
            get { return _label; }
            protected set { _label = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                return _p0;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate DirectedCoordinate
        {
            get
            {
                return _p1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int Quadrant
        {
            get
            {
                return _quadrant;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual double Dx
        {
            get
            {
                return _dx;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual double Dy
        {
            get
            {
                return _dy;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Node Node
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
            }
        }

        #region IComparable Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual int CompareTo(object obj)
        {
            EdgeEnd e = (EdgeEnd)obj;
            return CompareDirection(e);
        }

        #endregion

        private void DoInit(Coordinate p0, Coordinate p1)
        {
            _p0 = p0;
            _p1 = p1;
            _dx = p1.X - p0.X;
            _dy = p1.Y - p0.Y;
            _quadrant = QuadrantOp.Quadrant(_dx, _dy);
            Assert.IsTrue(!(_dx == 0 && _dy == 0), "EdgeEnd with identical endpoints found");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        protected virtual void Init(Coordinate p0, Coordinate p1)
        {
            DoInit(p0, p1);
        }

        /// <summary>
        /// Implements the total order relation:
        /// a has a greater angle with the positive x-axis than b.
        /// Using the obvious algorithm of simply computing the angle is not robust,
        /// since the angle calculation is obviously susceptible to roundoff.
        /// A robust algorithm is:
        /// - first compare the quadrant.  If the quadrants
        /// are different, it it trivial to determine which vector is "greater".
        /// - if the vectors lie in the same quadrant, the computeOrientation function
        /// can be used to decide the relative orientation of the vectors.
        /// </summary>
        /// <param name="e"></param>
        public virtual int CompareDirection(EdgeEnd e)
        {
            if (_dx == e._dx && _dy == e._dy)
                return 0;
            // if the rays are in different quadrants, determining the ordering is trivial
            if (_quadrant > e._quadrant)
                return 1;
            if (_quadrant < e._quadrant)
                return -1;
            // vectors are in the same quadrant - check relative orientation of direction vectors
            // this is > e if it is CCW of e
            return CgAlgorithms.ComputeOrientation(e._p0, e._p1, _p1);
        }

        /// <summary>
        /// Subclasses should override this if they are using labels
        /// </summary>
        public virtual void ComputeLabel() { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="outstream"></param>
        public virtual void Write(StreamWriter outstream)
        {
            double angle = Math.Atan2(_dy, _dx);
            string fullname = GetType().FullName;
            int lastDotPos = fullname.LastIndexOf('.');
            string name = fullname.Substring(lastDotPos + 1);
            outstream.Write("  " + name + ": " + _p0 + " - " + _p1 + " " + _quadrant + ":" + angle + "   " + Label);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(('['));
            sb.Append(_p0.X);
            sb.Append((' '));
            sb.Append(_p1.Y);
            sb.Append((']'));
            return sb.ToString();
        }
    }
}