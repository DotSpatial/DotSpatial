// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/23/2009 3:42:27 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Data;

namespace DotSpatial.Data
{
    /// <summary>
    /// DataTableStatisticsEM
    /// </summary>
    public static class DataTableStatisticsExt
    {
        /// <summary>
        /// Inspects the members of the data Table, focusing on the named field.  It calculates
        /// the median of the values in the named field.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
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
                double low = Convert.ToDouble(lst.Count / 2 - 1);
                result.Median = (high + low) / 2;
            }
            else
            {
                result.Median = lst[(int)Math.Floor(lst.Count / (double)2)];
            }
            return result;
        }
    }
}