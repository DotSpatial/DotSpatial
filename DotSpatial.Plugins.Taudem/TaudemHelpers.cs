// -----------------------------------------------------------------------
// <copyright file="TaudemHelpers.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using DotSpatial.Data;

namespace DotSpatial.Plugins.Taudem
{
    /// <summary>
    /// Used to support legacy style code. These are intended to be removed when the extension is redesigned.
    /// </summary>
    public static class TaudemHelpers
    {
        public static string GetFileName(dynamic dataset)
        {
            return dataset.Filename;
        }

        public static object get_CellValue(this IFeatureSet file, int col, int row)
        {
            return file.DataTable.Rows[row].ItemArray[col];
        }

        public static void EditCellValue(this IFeatureSet file, int col, int row, object newValue)
        {
            file.DataTable.Rows[row][col] = newValue;
        }

        public static IFeature get_Shape(this IFeatureSet file, int index)
        {
            IFeature shape = file.Features[index];

            return shape;
        }

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
    }
}