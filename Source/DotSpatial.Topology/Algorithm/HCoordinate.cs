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

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Represents a homogeneous coordinate for 2-D coordinates.
    /// </summary>
    public class HCoordinate
    {
        private double _w;
        private double _x;
        private double _y;

        /// <summary>
        ///
        /// </summary>
        public HCoordinate()
        {
            _x = 0.0;
            _y = 0.0;
            _w = 1.0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        public HCoordinate(double x, double y, double w)
        {
            _x = x;
            _y = y;
            _w = w;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        public HCoordinate(Coordinate p)
        {
            _x = p.X;
            _y = p.Y;
            _w = 1.0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public HCoordinate(HCoordinate p1, HCoordinate p2)
        {
            _x = p1._y * p2._w - p2._y * p1._w;
            _y = p2._x * p1._w - p1._x * p2._w;
            _w = p1._x * p2._y - p2._x * p1._y;
        }

        /// <summary>
        /// Direct access to x private field
        /// </summary>
        [Obsolete("This is a simple access to x private field: use GetX() instead.")]
        protected virtual double X
        {
            get { return _x; }
            set { _x = value; }
        }

        /// <summary>
        /// Direct access to y private field
        /// </summary>
        [Obsolete("This is a simple access to y private field: use GetY() instead.")]
        protected virtual double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        /// <summary>
        /// Direct access to w private field
        /// </summary>
        [Obsolete("This is a simple access to w private field: how do you use this field for?...")]
        protected virtual double W
        {
            get { return _w; }
            set { _w = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                return new Coordinate(GetX(), GetY());
            }
        }

        /// <summary>
        /// Computes the (approximate) intersection point between two line segments
        /// using homogeneous coordinates.
        /// Notice that this algorithm is
        /// not numerically stable; i.e. it can produce intersection points which
        /// lie outside the envelope of the line segments themselves.  In order
        /// to increase the precision of the calculation input points should be normalized
        /// before passing them to this routine.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <returns></returns>
        public static Coordinate Intersection(Coordinate p1, Coordinate p2, Coordinate q1, Coordinate q2)
        {
            HCoordinate l1 = new HCoordinate(new HCoordinate(p1), new HCoordinate(p2));
            HCoordinate l2 = new HCoordinate(new HCoordinate(q1), new HCoordinate(q2));
            HCoordinate intHCoord = new HCoordinate(l1, l2);
            Coordinate intPt = intHCoord.Coordinate;
            return intPt;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual double GetX()
        {
            double a = _x / _w;
            if ((Double.IsNaN(a)) || (Double.IsInfinity(a)))
                throw new NotRepresentableException();
            return a;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual double GetY()
        {
            double a = _y / _w;
            if ((Double.IsNaN(a)) || (Double.IsInfinity(a)))
                throw new NotRepresentableException();
            return a;
        }
    }
}