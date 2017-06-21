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
    /// Contains a magnitude and direction
    /// Supports more fundamental calculations than LineSegment, rather than topological functions
    /// </summary>
    public interface IVector
    {
        #region Properties

        /// <summary>
        /// The Euclidean distance from the origin to the tip of the 3 dimensional vector
        /// Setting the magntiude won't change the direction.
        /// </summary>
        double Length
        {
            get;
            set;
        }

        /// <summary>
        /// Returns the magnitude of the projection of the vector onto the X-Y plane
        /// Setting this magnitude will not affect Z, which should be adjusted separately
        /// </summary>
        double Length2D
        {
            get;
            set;
        }

        /// <summary>
        /// Obtains the angle above the X-Y plane.  Positive towards positive Z.
        /// Values are in radians from -Pi/2 to Pi/2
        /// Setting this value when no magnitude exists results in a unit vector with angle phi in the X direction.
        /// </summary>
        double Phi
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the angle in the X-Y plane.  0 along the positive X axis, and increasing counterclockwise
        /// Values are in Radians.  Setting this value when no X-Y magnitude exists results in a unit vector
        /// between X and Y, but does not affect Z, so you may have something other than a unit vector in 3-D.
        /// Set theta before phi in order to obtain a unit vector in 3-D space.
        /// </summary>
        double Theta
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Transforms a point that has 3 dimensions by multiplying it by the
        /// specified 3 x 3 matrix in the upper left, but treats the
        /// bottom row as supplying the translation coordinates.
        /// </summary>
        /// <param name="transformMatrix"></param>
        /// <returns></returns>
        IVector TransformCoordinate(IMatrix4 transformMatrix);

        /// <summary>
        /// Rotations and transformations work by applying matrix mathematics,
        /// so this creates a 1 x 4 version of this vector.  The 4th value
        /// is always 1, and allows for the translation terms to work.
        /// </summary>
        /// <returns></returns>
        IMatrixD ToMatrix();

        /// <summary>
        /// Returns the square of the distance of the vector without taking the square root
        /// This is the same as doting the vector with itself
        /// </summary>
        /// <returns>Double, the square of the distance between the vectors</returns>
        double Norm2();

        /// <summary>
        /// Assuming the vector starts at the origin of 0, 0, 0, this function returns
        /// a Point representing the tip of the vector.
        /// </summary>
        IPoint ToPoint();

        /// <summary>
        /// Converts this vector to a coordinate by assuming that the X, Y and Z values
        /// are the X, Y and Z values of the locaiton.
        /// </summary>
        /// <returns>An ICoordinate</returns>
        Coordinate ToCoordinate();

        /// <summary>
        /// Returns a new segment from this vector, where the StartPoint is 0, 0, 0
        /// and the End Point is the tip of this vector
        /// </summary>
        /// <returns>An implementation of ILineSegment</returns>
        ILineSegment ToLineSegment();

        #region -------------------- NORMALIZE ----------------------------

        /// <summary>
        /// Normalizes the vector.
        /// </summary>
        void Normalize();

        #endregion

        #region -------------------- CROSS --------------------------------

        /// <summary>
        /// Returns the cross product of this vector with the specified vector V
        /// </summary>
        /// <param name="v">The vector to perform a cross product against</param>
        /// <returns>A vector result from the inner product</returns>
        IVector Cross(IVector v);

        #endregion

        #region -------------------- DOT ---------------------------------

        /// <summary>
        /// Returns the dot product of this vector with V2
        /// </summary>
        /// <param name="v">The vector to perform an inner product against</param>
        /// <returns>A Double result from the inner product</returns>
        double Dot(IVector v);

        #endregion

        #region -------------------- IS EQUAL TO---------------------------

        /// <summary>
        /// Compares the values of each element, and if all the elements are equal, returns true.
        /// </summary>
        /// <param name="v">The vector to compare against this vector.</param>
        /// <returns>Boolean, true if all the elements have the same value.</returns>
        bool Intersects(IVector v);

        /// <summary>
        /// Override  for definition of equality for vectors
        /// </summary>
        /// <param name="v">A vector to compare with</param>
        /// <returns>true if the X, Y, and Z coordinates are all equal</returns>
        bool Equals(IVector v);

        #endregion

        #region -------------------- SUBTRACT --------------------------------

        /// <summary>
        /// Subtracts each element of V from each element of this vector
        /// </summary>
        /// <param name="v">Vector, the vector to subtract from this vector</param>
        /// <returns>A vector result from the subtraction</returns>
        IVector Subtract(IVector v);

        #endregion

        #region -------------------- ADD ---------------------------------

        /// <summary>
        /// Adds each of the elements of V to the elements of this vector
        /// </summary>
        /// <param name="v">Vector, the vector to add to this vector</param>
        /// <returns>A vector result from the addition</returns>
        IVector Add(IVector v);

        #endregion

        #region -------------------- MULTIPLY --------------------------------

        /// <summary>
        /// Returns the scalar product of this vector against a scalar
        /// </summary>
        /// <param name="scalar">Double, a value to multiply against all the members of this vector</param>
        /// <returns>A vector multiplied by the scalar</returns>
        IVector Multiply(double scalar);

        /// <summary>
        /// Rotates the vector about the X axis as though the tail of the vector were at the origin
        /// </summary>
        /// <param name="degrees">The angle in degrees to rotate counter-clockwise when looking at the origin from the positive axis.</param>
        /// <returns>A new IVector that has been rotated</returns>
        IVector RotateX(double degrees);

        /// <summary>
        /// Rotates the vector about the Y axis as though the tail of the vector were at the origin
        /// </summary>
        /// <param name="degrees">The angle in degrees to rotate counter-clockwise when looking at the origin from the positive axis.</param>
        /// <returns>A new IVector that has been rotated</returns>
        IVector RotateY(double degrees);

        /// <summary>
        /// Rotates the vector about the Z axis as though the tail of the vector were at the origin
        /// </summary>
        /// <param name="degrees">The angle in degrees to rotate counter-clockwise when looking at the origin from the positive axis.</param>
        /// <returns>A new IVector that has been rotated</returns>
        IVector RotateZ(double degrees);

        #endregion

        #endregion
    }
}