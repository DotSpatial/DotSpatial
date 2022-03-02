// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// IMatrix4.
    /// </summary>
    public interface IMatrixD : IMatrix
    {
        #region Properties

        /// <summary>
        /// Gets or sets the values for this matrix of double precision coordinates.
        /// </summary>
        double[,] Values
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Multiplies every value in the specified n x m matrix by the specified double inScalar.
        /// </summary>
        /// <param name="inScalar">The double precision floating point to multiply all the members against.</param>
        /// <returns>A new n x m matrix.</returns>
        IMatrixD Multiply(double inScalar);

        /// <summary>
        /// This replaces the underlying general multiplication with a more specific type.
        /// </summary>
        /// <param name="matrix">The matrix to multiply with.</param>
        /// <returns>The resulting matrix.</returns>
        IMatrixD Multiply(IMatrixD matrix);

        #endregion
    }
}