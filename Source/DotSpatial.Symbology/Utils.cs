// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Contains misc utils methods.
    /// </summary>
    public static class Utils
    {
        #region Methods

        /// <summary>
        /// Bytes the range.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Value in bytes</returns>
        public static int ByteRange(double value)
        {
            int rounded = (int)Math.Round(value);
            if (rounded > 255) return 255;
            if (rounded < 0) return 0;

            return rounded;
        }

        /// <summary>
        /// Gets the nearest value.
        /// </summary>
        /// <param name="value">The value to search.</param>
        /// <param name="values">The values to search in.</param>
        /// <returns>Nearest value</returns>
        public static double GetNearestValue(double value, List<double> values)
        {
            if (values == null || values.Count == 0) return 0;

            int indx = values.BinarySearch(value);
            if (indx >= 0)
            {
                return values[indx];
            }

            int iHigh = -indx;
            if (iHigh >= 0 && iHigh < values.Count)
            {
                double high = values[iHigh];
                int iLow = -indx - 1;
                if (iLow >= 0 && iLow < values.Count && iLow != iHigh)
                {
                    double low = values[iLow];
                    return value - low < high - value ? low : high;
                }
            }

            int iLow2 = -indx - 1;
            if (iLow2 >= 0 && iLow2 < values.Count)
            {
                return values[iLow2];
            }

            return 0;
        }

        /// <summary>
        /// Returns value with specified significant digits.
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="numFigures">Number of significant digits.</param>
        /// <returns>Value with with specified significant digits.</returns>
        public static double SigFig(double value, int numFigures)
        {
            if (value == 0.0) return 0.0;

            int md = (int)Math.Ceiling(Math.Log10(Math.Abs(value)));
            md -= numFigures;
            double norm = Math.Pow(10, md);
            return norm * Math.Round(value / norm);
        }

        #endregion
    }
}