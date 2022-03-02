// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// Operations on 3D vectors can be carried out using a 4D Matrix. This interface
    /// provides access to methods that are specific to 3D vector opperations.
    /// </summary>
    public interface IMatrix4 : IMatrixD
    {
        #region Methods

        /// <summary>
        /// Multiplies the current matrix by a rotation matrix corresponding to the specified angle to create rotation in the Z direction.
        /// </summary>
        /// <param name="degrees">The angle to rotate in degrees.</param>
        /// <returns>The rotated Matrix.</returns>
        IMatrix4 RotateX(double degrees);

        /// <summary>
        /// Rotates the current matrix around the Y axis by multiplying the current matrix by a rotation matrix.
        /// </summary>
        /// <param name="degrees">The angle to rotate in degrees.</param>
        /// <returns>The rotated Matrix.</returns>
        IMatrix4 RotateY(double degrees);

        /// <summary>
        /// Rotates the current matrix around the Z axis by multiplying the current matrix by a rotation matrix.
        /// </summary>
        /// <param name="degrees">The angle to rotate in degrees.</param>
        /// <returns>The rotated Matrix.</returns>
        IMatrix4 RotateZ(double degrees);

        /// <summary>
        /// Translates the matrix by the specified amount in each of the directions by multiplying by a translation matrix created from the specified values.
        /// </summary>
        /// <param name="x">The translation in the X coordinate.</param>
        /// <param name="y">The translation in the Y coordinate.</param>
        /// <param name="z">The translation in the Z coordinate.</param>
        /// <returns>The translated matrix.</returns>
        IMatrix4 Translate(double x, double y, double z);

        #endregion
    }
}