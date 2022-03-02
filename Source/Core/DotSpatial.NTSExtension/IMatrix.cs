// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// IMatrix.
    /// </summary>
    public interface IMatrix
    {
        #region Properties

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        int NumColumns
        {
            get;
        }

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        int NumRows
        {
            get;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the matrix multiplication against the specified matrix.
        /// </summary>
        /// <param name="matrix">The matrix to multiply with.</param>
        /// <returns>The resulting matrix.</returns>
        IMatrix Multiply(IMatrix matrix);

        #endregion
    }
}