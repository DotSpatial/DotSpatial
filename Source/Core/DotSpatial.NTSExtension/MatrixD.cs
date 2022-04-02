// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// A 4 x 4 matrix is required for transformations in 3 dimensions.
    /// </summary>
    public class MatrixD : IMatrixD
    {
        #region Fields

        private double[,] _values;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixD"/> class that is a square of the given size.
        /// </summary>
        /// <param name="m">The size of the matrix to create.</param>
        public MatrixD(int m)
        {
            _values = new double[m, m];
            M = m;
            N = m;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixD"/> class with m rows and n columns.
        /// </summary>
        /// <param name="m">The row size of the matrix to create.</param>
        /// <param name="n">The column size of the matrix to create.</param>
        public MatrixD(int m, int n)
        {
            _values = new double[m, n];
            M = m;
            N = n;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MatrixD"/> class using the specified values.
        /// </summary>
        /// <param name="values">Values to use for the matrix.</param>
        public MatrixD(double[,] values)
        {
            _values = values;
            M = values.GetLength(0);
            N = values.GetLength(1);
        }

        #region Properties

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        public int M { get; private set; }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int N { get; private set; }

        /// <summary>
        /// Gets the number of columns.
        /// </summary>
        public int NumColumns => N;

        /// <summary>
        /// Gets the number of rows.
        /// </summary>
        public int NumRows => M;

        /// <summary>
        /// Gets or sets the values for this matrix.
        /// </summary>
        public double[,] Values
        {
            get
            {
                return _values;
            }

            set
            {
                _values = value;
                M = _values.GetLength(0);
                N = _values.GetLength(1);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Matrix multiplication only works if the number of columns of the first matrix is the same as the number of rows of the second matrix.
        /// The first matrix is this object, so this will only work if inMatrix has the same number of rows as this matrix has columns.
        /// </summary>
        /// <param name="inMatrix">The IMatrix to multiply against this matrix.</param>
        /// <returns>Result of the multiplication.</returns>
        /// <exception cref="ArgumentException">Thrown if number of columns of the first matrix differs from the number of rows from the second matrix.</exception>
        public IMatrixD Multiply(IMatrixD inMatrix)
        {
            if (inMatrix.NumRows != NumColumns) throw new ArgumentException("Matrix multiplication only works if the number of columns of the first matrix is the same as the number of rows of the second matrix");

            int m = NumRows;
            int n = inMatrix.NumColumns;
            double[,] vals = new double[m, n];
            for (int row = 0; row < m; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    vals[row, col] = 0;
                    for (int i = 0; i < NumColumns; i++)
                    {
                        vals[row, col] += _values[row, i] * inMatrix.Values[i, col];
                    }
                }
            }

            return new MatrixD(vals);
        }

        /// <summary>
        /// Multiplies this matrix by the specified scalar value.
        /// </summary>
        /// <param name="inScalar">Scalar used for multiplication.</param>
        /// <returns>The resulting matrix.</returns>
        public IMatrixD Multiply(double inScalar)
        {
            double[,] vals = new double[NumRows, NumColumns];
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    vals[row, col] = _values[row, col] * inScalar;
                }
            }

            return new MatrixD(vals);
        }

        /// <summary>
        /// Matrix multiplication only works if the number of columns of the first matrix is the same as the number of rows of the second matrix.
        /// The first matrix is this object, so this will only work if inMatrix has the same number of rows as this matrix has columns.
        /// </summary>
        /// <param name="inMatrix">The IMatrix to multiply against this matrix.</param>
        /// <returns>Result of the multiplication.</returns>
        /// <exception cref="ArgumentException">Thrown if inMatrix is not a IMatrixD or the number of columns of the first matrix differs from the number of rows from the second matrix.</exception>
        IMatrix IMatrix.Multiply(IMatrix inMatrix)
        {
            if (inMatrix is not IMatrixD mat) throw new ArgumentException("Invalid Matrix provided for inMatrix");
            return Multiply(mat);
        }

        #endregion
    }
}