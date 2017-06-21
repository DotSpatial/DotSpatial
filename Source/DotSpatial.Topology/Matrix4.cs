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

namespace DotSpatial.Topology
{
    /// <summary>
    /// Matrix4
    /// </summary>
    public class Matrix4 : MatrixD, IMatrix4
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Matrix4 that is an identity matrix
        /// </summary>
        public Matrix4()
            : base(4)
        {
            Values[0, 0] = 1;
            Values[1, 1] = 1;
            Values[2, 2] = 1;
            Values[3, 3] = 1;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Rotates this matrix by the specified angle in degrees about the X axis.
        /// </summary>
        /// <param name="degrees">Specified the angle in degrees to rotate counter clockwise about the positive axis</param>
        /// <returns></returns>
        public IMatrix4 RotateX(double degrees)
        {
            Matrix4 rot = RotationX(degrees);
            return Multiply(rot) as IMatrix4;
        }

        /// <summary>
        /// Rotates this matrix by the specified angle in degrees about the Y axis.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public IMatrix4 RotateY(double degrees)
        {
            Matrix4 rot = RotationY(degrees);
            return Multiply(rot) as IMatrix4;
        }

        /// <summary>
        /// Rotates this matrix by the specified angle in degrees about the Z axis.
        /// </summary>
        /// <param name="degrees"></param>
        /// <returns></returns>
        public IMatrix4 RotateZ(double degrees)
        {
            Matrix4 rot = RotationZ(degrees);
            return Multiply(rot) as IMatrix4;
        }

        /// <summary>
        /// Translates the matrix by the specified amount in each of the directions
        /// by multiplying by a translation matrix created from the specified values.
        /// </summary>
        /// <param name="x">The translation in the X coordinate</param>
        /// <param name="y">The translation in the Y coordinate</param>
        /// <param name="z">The translation in the Z coordinate</param>
        /// <returns></returns>
        public IMatrix4 Translate(double x, double y, double z)
        {
            Matrix4 tran = Translation(x, y, z);
            return Multiply(tran) as IMatrix4;
        }

        #endregion

        #region Properties

        #endregion

        /// <summary>
        ///
        /// </summary>
        public static Matrix4 Identity
        {
            get { return new Matrix4(); }
        }

        /// <summary>
        /// Creates a 4 x 4 matrix that can be used to rotate a 3D vector about the X axis.
        /// </summary>
        /// <param name="degrees">The counter-clockwise angle of rotation when looking at the origin from the positive axis</param>
        /// <returns>A 4x4 rotation matrix</returns>
        public static Matrix4 RotationX(double degrees)
        {
            Matrix4 result = new Matrix4();
            double rad = degrees * Math.PI / 180;
            double[,] vals = result.Values;
            vals[1, 1] = Math.Cos(rad);
            vals[2, 1] = -Math.Sin(rad);
            vals[1, 2] = Math.Sin(rad);
            vals[2, 2] = Math.Cos(rad);
            return result;
        }

        /// <summary>
        /// Creates a 4 x 4 matrix that can be used to rotate a 3D vector about the Y axis.
        /// </summary>
        /// <param name="degrees">The counter-clockwise angle of rotation when looking at the origin from the positive axis</param>
        /// <returns>A 4x4 rotation matrix</returns>
        public static Matrix4 RotationY(double degrees)
        {
            Matrix4 result = new Matrix4();
            double rad = degrees * Math.PI / 180;
            double[,] vals = result.Values;
            vals[0, 0] = Math.Cos(rad);
            vals[0, 2] = -Math.Sin(rad);
            vals[2, 0] = Math.Sin(rad);
            vals[2, 2] = Math.Cos(rad);
            return result;
        }

        /// <summary>
        /// Creates a 4 x 4 matrix that can be used to rotate a 3D vector about the Z axis.
        /// </summary>
        /// <param name="degrees">The counter-clockwise angle of rotation when looking at the origin from the positive axis</param>
        /// <returns>A 4x4 rotation matrix</returns>
        public static Matrix4 RotationZ(double degrees)
        {
            Matrix4 result = new Matrix4();
            double rad = degrees * Math.PI / 180;
            double[,] vals = result.Values;
            vals[0, 0] = Math.Cos(rad);
            vals[0, 1] = -Math.Sin(rad);
            vals[1, 0] = Math.Sin(rad);
            vals[1, 1] = Math.Cos(rad);
            return result;
        }

        /// <summary>
        /// Creates a 4 x 4 matrix where all the values represent an identity matrix except
        /// that the bottom row has been set to be the translation values.  The result is
        /// that if a 3D vector is transformed by this matrix, the last row will
        /// control the translation terms.
        /// </summary>
        /// <param name="x">The translation in the x direction</param>
        /// <param name="y">The translation in the y direction</param>
        /// <param name="z">The translation in the z direction</param>
        /// <returns>The translation matrix</returns>
        public static Matrix4 Translation(double x, double y, double z)
        {
            Matrix4 result = new Matrix4();
            double[,] vals = result.Values;
            vals[3, 0] = x;
            vals[3, 1] = y;
            vals[3, 2] = z;
            return result;
        }
    }
}