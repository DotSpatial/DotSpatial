// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/27/2009 9:08:11 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Reprsents som basic statistics like Min/Max/Meduim, etc.
    /// </summary>
    public class Statistics
    {
        #region Methods

        /// <summary>
        /// Resets all statistics to 0.
        /// </summary>
        public void Clear()
        {
            Count = 0;
            Minimum = 0;
            Maximum = 0;
            Mean = 0;
            Median = 0;
            Sum = 0;
            StandardDeviation = 0;
        }

        /// <summary>
        /// Calculates the statistics for the specified values
        /// </summary>
        public void Calculate(List<double> values, double min = double.MinValue, double max = double.MaxValue)
        {
            values.Sort(); // Linear solutions for finding median can be problematic.  Easier to simply sort values.

            if (values.Count == 0)
            {
                Clear();
                return;
            }
            if (values.Count % 2 == 0)
            {
                // In the even case, take the average of the two middle values
                int lowIndex = (values.Count - 1) / 2; // Divide by one less than values.Count to grab the middle two values.
                Median = (values[lowIndex] + values[lowIndex + 1]) / 2;
            }
            else
            {
                int index = values.Count / 2; // integer division causes 5-> 2, not 2.5.
                Median = values[index];
            }
            Count = values.Count;
            Minimum = values[0];
            Maximum = values[values.Count - 1];

            double total = 0;
            double sqrTotal = 0;
            foreach (double val in values)
            {
                total += val;
                sqrTotal += val * val;
            }
            Sum = total;
            Mean = total / Count;
            StandardDeviation = Math.Sqrt((sqrTotal / Count) - (total / Count) * (total / Count));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the integer count of the values
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        /// Gets the highest value
        /// </summary>
        public double Maximum { get; protected set; }

        /// <summary>
        /// Gets the average value
        /// </summary>
        public double Mean { get; protected set; }

        /// <summary>
        /// Gets the middle value, or the average of the two middle values
        /// </summary>
        public double Median { get; protected set; }

        /// <summary>
        /// Gets the minimum value
        /// </summary>
        public double Minimum { get; protected set; }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        public double StandardDeviation { get; protected set; }

        /// <summary>
        /// Gets the sum of the values.
        /// </summary>
        public double Sum { get; protected set; }

        #endregion
    }
}