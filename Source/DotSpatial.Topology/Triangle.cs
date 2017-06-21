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
namespace DotSpatial.Topology
{
    /// <summary>
    /// Represents a planar triangle, and provides methods for calculating various
    /// properties of triangles.
    /// </summary>
    public class Triangle
    {
        private Coordinate _p0, _p1, _p2;

        /// <summary>
        ///
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        public Triangle(Coordinate p0, Coordinate p1, Coordinate p2)
        {
            _p0 = p0;
            _p1 = p1;
            _p2 = p2;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate P2
        {
            get { return _p2; }
            set { _p2 = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate P1
        {
            get { return _p1; }
            set { _p1 = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate P0
        {
            get { return _p0; }
            set { _p0 = value; }
        }

        /// <summary>
        /// The inCentre of a triangle is the point which is equidistant
        /// from the sides of the triangle.  This is also the point at which the bisectors
        /// of the angles meet.
        /// </summary>
        /// <returns>
        /// The point which is the InCentre of the triangle.
        /// </returns>
        public virtual Coordinate InCentre
        {
            get
            {
                // the lengths of the sides, labelled by their opposite vertex
                double len0 = P1.Distance(P2);
                double len1 = P0.Distance(P2);
                double len2 = P0.Distance(P1);
                double circum = len0 + len1 + len2;

                double inCentreX = (len0 * P0.X + len1 * P1.X + len2 * P2.X) / circum;
                double inCentreY = (len0 * P0.Y + len1 * P1.Y + len2 * P2.Y) / circum;
                return new Coordinate(inCentreX, inCentreY);
            }
        }
    }
}