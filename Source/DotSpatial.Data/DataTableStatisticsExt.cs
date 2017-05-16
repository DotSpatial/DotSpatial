// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Data;

namespace DotSpatial.Data
{
    /// <summary>
    /// This contains statistic extensions for the DataTable class.
    /// </summary>
    public static class DataTableStatisticsExt
    {
        /// <summary>
        /// Inspects the members of the dataTable, focusing on the named field. It calculates the median of the values in the named field.
        /// </summary>
        /// <param name="self">DataTable</param>
        /// <param name="fieldName">The field name.</param>
        /// <returns>The BoxStatistics.</returns>
        public static BoxStatistics GetBoxStatistics(this DataTable self, string fieldName)
        {
            DataColumn dc = self.Columns[fieldName];
            ArrayList lst = new ArrayList();
            foreach (DataRow row in self.Rows)
            {
                lst.Add(row[fieldName]);
            }

            lst.Sort();
            BoxStatistics result = new BoxStatistics();
            if (lst.Count % 2 == 0)
            {
                if (dc.DataType == typeof(string))
                {
                }

                // For an even number of items, the mean is the average of the middle two values (after sorting)
                double high = Convert.ToDouble(lst.Count / 2);
                double low = Convert.ToDouble((lst.Count / 2) - 1);
                result.Median = (high + low) / 2;
            }
            else
            {
                result.Median = lst[(int)Math.Floor(lst.Count / 2D)];
            }

            return result;
        }
    }
}