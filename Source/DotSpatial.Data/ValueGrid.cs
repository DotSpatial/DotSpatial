// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// ValueGrid
    /// </summary>
    /// <typeparam name="T">Type of the items in the ValueGrid.</typeparam>
    public class ValueGrid<T> : IValueGrid
        where T : IEquatable<T>, IComparable<T>
    {
        #region Fields

        private readonly T[][] _rowBuffer;
        private readonly Raster<T> _sourceRaster;
        private readonly int _topRow;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueGrid{T}"/> class this way assumes the values are not in ram and will
        /// simply buffer 3 rows.
        /// </summary>
        /// <param name="sourceRaster">The source raster.</param>
        public ValueGrid(Raster<T> sourceRaster)
        {
            _sourceRaster = sourceRaster;
            _topRow = 0;
            _rowBuffer = new T[3][];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the values are updated. It is the responsibility of the user to set this
        /// value to false again when the situation warents it.
        /// </summary>
        public bool Updated { get; set; }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets a value at the 0 row, 0 column index.
        /// </summary>
        /// <param name="row">The 0 based vertical row index from the top</param>
        /// <param name="column">The 0 based horizontal column index from the left</param>
        /// <returns>An object reference to the actual value in the data member.</returns>
        public double this[int row, int column]
        {
            get
            {
                if (_sourceRaster.IsInRam)
                {
                    return ToDouble(_sourceRaster.Data[row][column]);
                }

                if (_rowBuffer[0] == null)
                {
                    _rowBuffer[0] = _sourceRaster.ReadRow(_topRow);
                }

                if (_rowBuffer[1] == null)
                {
                    _rowBuffer[1] = _sourceRaster.ReadRow(_topRow + 1);
                }

                if (_rowBuffer[2] == null)
                {
                    _rowBuffer[2] = _sourceRaster.ReadRow(_topRow + 2);
                }

                int shift = row - _topRow;
                if (shift >= 0 && shift < 3)
                {
                    // the value is in a row buffer
                    return ToDouble(_rowBuffer[row - _topRow][column]);
                }

                // the value was not found in the buffer. If the value is below the row buffer, load the new row as the bottom row.
                if (shift == 3)
                {
                    _rowBuffer[0] = _rowBuffer[1];
                    _rowBuffer[1] = _rowBuffer[2];
                    _rowBuffer[2] = _sourceRaster.ReadRow(row);
                    return ToDouble(_rowBuffer[2][column]);
                }

                if (shift == 4)
                {
                    _rowBuffer[0] = _rowBuffer[2];
                    _rowBuffer[1] = _sourceRaster.ReadRow(row - 1);
                    _rowBuffer[2] = _sourceRaster.ReadRow(row);
                    return ToDouble(_rowBuffer[2][column]);
                }

                if (shift > 4)
                {
                    _rowBuffer[0] = _sourceRaster.ReadRow(row - 2);
                    _rowBuffer[1] = _sourceRaster.ReadRow(row - 1);
                    _rowBuffer[2] = _sourceRaster.ReadRow(row);
                    return ToDouble(_rowBuffer[2][column]);
                }

                if (shift == -1)
                {
                    _rowBuffer[0] = _sourceRaster.ReadRow(row);
                    _rowBuffer[1] = _rowBuffer[0];
                    _rowBuffer[2] = _rowBuffer[1];
                    return ToDouble(_rowBuffer[0][column]);
                }

                if (shift == -2)
                {
                    _rowBuffer[0] = _sourceRaster.ReadRow(row);
                    _rowBuffer[1] = _sourceRaster.ReadRow(row + 1);
                    _rowBuffer[2] = _rowBuffer[0];
                    return ToDouble(_rowBuffer[0][column]);
                }

                if (shift < -2)
                {
                    _rowBuffer[0] = _sourceRaster.ReadRow(row);
                    _rowBuffer[1] = _sourceRaster.ReadRow(row + 1);
                    _rowBuffer[2] = _sourceRaster.ReadRow(row + 2);
                    return ToDouble(_rowBuffer[0][column]);
                }

                // this should never happen
                throw new ApplicationException(DataStrings.IndexingErrorIn_S.Replace("%S", "Raster.Value[" + row + ", " + column + "]"));
            }

            set
            {
                Updated = true;
                if (!_sourceRaster.IsInRam) return;

                T test = (T)Convert.ChangeType(value, typeof(T));
                if (test.CompareTo(Global.MaximumValue<T>()) > 0)
                {
                    _sourceRaster.Data[row][column] = Global.MaximumValue<T>();
                }
                else if (test.CompareTo(Global.MinimumValue<T>()) < 0)
                {
                    _sourceRaster.Data[row][column] = Global.MinimumValue<T>();
                }
                else
                {
                    // This is slow so only do this when we have to.
                    _sourceRaster.Data[row][column] = test;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This involves boxing and unboxing as well as a convert to double, but IConvertible was
        /// not CLS Compliant, so we were always getting warnings about it. I am trying to make
        /// all the code CLS Compliant to remove warnings.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value converted to double.</returns>
        public static double ToDouble(T value)
        {
            return Global.ToDouble(value);
        }

        #endregion
    }
}