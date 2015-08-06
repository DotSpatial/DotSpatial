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

namespace DotSpatial.Topology.Index.KDTree
{
    /// <summary>
    /// A node of a <see cref="KdTree{T}"/>, which represents one or more points in the same location.
    /// </summary>
    /// <typeparam name="T">The type of the object</typeparam>
    /// <author>dskea</author>
    public class KdNode<T> where T : class
    {
        #region Constructors

        /// <summary>
        /// Creates a new KdNode.
        /// </summary>
        /// <param name="x">coordinate of point</param>
        /// <param name="y">coordinate of point</param>
        /// <param name="data">A data objects to associate with this node</param>
        public KdNode(double x, double y, T data)
        {
            Coordinate = new Coordinate(x, y);
            Left = null;
            Right = null;
            Count = 1;
            Data = data;
        }

        /// <summary>
        /// Creates a new KdNode.
        /// </summary>
        /// <param name="p">The point location of new node</param>
        /// <param name="data">A data objects to associate with this node</param>
        public KdNode(Coordinate p, T data)
        {
            Coordinate = new Coordinate(p);
            Left = null;
            Right = null;
            Count = 1;
            Data = data;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the location of this node
        /// </summary>
        public Coordinate Coordinate { get; private set; }

        /// <summary>
        /// Gets the number of inserted points that are coincident at this location.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the user data object associated with this node.
        /// </summary>
        public T Data { get; private set; }

        /// <summary>
        /// Gets whether more than one point with this value have been inserted (up to the tolerance)
        /// </summary>
        /// <returns></returns>
        public bool IsRepeated
        {
            get { return Count > 1; }
        }

        /// <summary>
        /// Gets x-ordinate of this node
        /// </summary>
        public double X
        {
            get { return Coordinate.X; }
        }

        /// <summary>
        /// Gets y-ordinate of this node
        /// </summary>
        public double Y
        {
            get { return Coordinate.Y; }
        }

        /// <summary>
        /// Gets or sets the left node of the tree
        /// </summary>
        public KdNode<T> Left { get; set; }

        /// <summary>
        /// Gets or sets the right node of the tree
        /// </summary>
        public KdNode<T> Right { get; set; }

        #endregion

        #region Methods

        // Increments counts of points at this location
        internal void Increment()
        {
            Count = Count + 1;
        }

        #endregion
    }
}