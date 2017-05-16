// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;

namespace DotSpatial.Plugins.Taudem
{
    /// <summary>
    /// Used to support legacy style code. These are intended to be removed when the extension is redesigned.
    /// </summary>
    public static class TaudemHelpers
    {
        #region Methods

        /// <summary>
        /// Updates the cell value.
        /// </summary>
        /// <param name="file">Featureset whose cell should be updated.</param>
        /// <param name="col">Column of the cell.</param>
        /// <param name="row">Row of the cell.</param>
        /// <param name="newValue">Value that should be set.</param>
        public static void EditCellValue(this IFeatureSet file, int col, int row, object newValue)
        {
            file.DataTable.Rows[row][col] = newValue;
        }

        /// <summary>
        /// Gets the cell value.
        /// </summary>
        /// <param name="file">Featureset that contains the cell value.</param>
        /// <param name="col">Column of the cell.</param>
        /// <param name="row">Row of the cell.</param>
        /// <returns>The cell value.</returns>
        public static object GetCellValue(this IFeatureSet file, int col, int row)
        {
            return file.DataTable.Rows[row].ItemArray[col];
        }

        /// <summary>
        /// Gets the feature with the given index.
        /// </summary>
        /// <param name="file">Featureset to the get feature from.</param>
        /// <param name="index">Index of the feature.</param>
        /// <returns>The feature belonging to the index.</returns>
        public static IFeature GetShape(this IFeatureSet file, int index)
        {
            IFeature shape = file.Features[index];

            return shape;
        }

        /// <summary>
        /// Gets the file name of the given dataset.
        /// </summary>
        /// <param name="dataset">Dataset to get the file name for.</param>
        /// <returns>The file name of the dataset.</returns>
        public static string GetFileName(dynamic dataset)
        {
            return dataset.Filename;
        }

        /// <summary>
        /// Gets the maximum value from the given raster.
        /// </summary>
        /// <param name="raster">Raster to get the maximum value from.</param>
        /// <returns>The maximum value.</returns>
        public static int GetMaximum(this IRaster raster)
        {
            int max = int.MinValue;
            var values = raster.Value as ValueGrid<int>;
            for (int row = 0; row < raster.NumRows; row++)
            {
                for (int col = 0; col < raster.NumColumns; col++)
                {
                    int val = Convert.ToInt32(values[row, col]);
                    if (val > max)
                        max = val;
                }
            }

            return max;
        }

        #endregion
    }
}