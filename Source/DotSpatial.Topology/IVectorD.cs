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
// *********************************************************************************************************
namespace DotSpatial.Topology
{
    /// <summary>
    /// Vector
    /// </summary>
    public interface IVectorD : ICoordinate
    {
        #region Methods

        /// <summary>
        /// Adds each of the elements of V to the elements of this vector
        /// </summary>
        /// <param name="v">Vector, the vector to add to this vector</param>
        /// <returns>A vector result from the addition</returns>
        Vector Add(Vector v);

        /// <summary>
        /// Returns the cross product of this vector with the specified vector V
        /// </summary>
        /// <param name="v">The vector to perform a cross product against</param>
        /// <returns>A vector result from the inner product</returns>
        Vector Cross(Vector v);

        /// <summary>
        /// Returns the dot product of this vector with V2
        /// </summary>
        /// <param name="v">The vector to perform an inner product against</param>
        /// <returns>A Double result from the inner product</returns>
        double Dot(Vector v);

        /// <summary>
        /// Returns the scalar product of this vector against a scalar
        /// </summary>
        /// <param name="scalar">Double, a value to multiply against all the members of this vector</param>
        /// <returns>A vector multiplied by the scalar</returns>
        Vector Multiply(double scalar);

        /// <summary>
        /// Normalizes the vector.
        /// </summary>
        void Normalize();

        /// <summary>
        /// Subtracts each element of V from each element of this vector
        /// </summary>
        /// <param name="v">Vector, the vector to subtract from this vector</param>
        /// <returns>A vector result from the subtraction</returns>
        Vector Subtract(Vector v);

        #endregion

        #region Properties

        /// <summary>
        /// The Euclidean distance from the origin to the tip of the 3 dimensional vector
        /// Setting the magntiude won't change the direction.
        /// </summary>
        double Length
        {
            get;
        }

        #endregion
    }
}