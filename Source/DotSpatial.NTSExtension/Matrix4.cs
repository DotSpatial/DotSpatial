// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// Matrix4
    /// </summary>
    public class Matrix4 : MatrixD, IMatrix4
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Matrix4"/> class that is an identity matrix.
        /// </summary>
        public Matrix4()
            : base(4)
        {
            Values[0, 0] = 1;
            Values[1, 1] = 1;
            Values[2, 2] = 1;
            Values[3, 3] = 1;
        }

        #region Properties

        /// <summary>
        /// Gets the Identity matrix.
        /// </summary>
        public static Matrix4 Identity => new Matrix4();

        #endregion

        #region Methods

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
        /// that the bottom row has been set to be the translation values. The result is
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

        /// <summary>
        /// Rotates this matrix by the specified angle in degrees about the X axis.
        /// </summary>
        /// <param name="degrees">Specified the angle in degrees to rotate counter clockwise about the positive axis</param>
        /// <returns>The rotated matrix.</returns>
        public IMatrix4 RotateX(double degrees)
        {
            Matrix4 rot = RotationX(degrees);
            return Multiply(rot) as IMatrix4;
        }

        /// <summary>
        /// Rotates this matrix by the specified angle in degrees about the Y axis.
        /// </summary>
        /// <param name="degrees">Specified the angle in degrees to rotate about the Y axis.</param>
        /// <returns>The rotated maxtrix.</returns>
        public IMatrix4 RotateY(double degrees)
        {
            Matrix4 rot = RotationY(degrees);
            return Multiply(rot) as IMatrix4;
        }

        /// <summary>
        /// Rotates this matrix by the specified angle in degrees about the Z axis.
        /// </summary>
        /// <param name="degrees">Specified the angle in degrees to rotate about the Z axis.</param>
        /// <returns>The rotated matrix.</returns>
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
        /// <returns>The translate matrix.</returns>
        public IMatrix4 Translate(double x, double y, double z)
        {
            Matrix4 tran = Translation(x, y, z);
            return Multiply(tran) as IMatrix4;
        }

        #endregion
    }
}