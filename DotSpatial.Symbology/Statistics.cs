// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
    /// Statistics
    /// </summary>
    public class Statistics
    {
        #region Private Variables

        private int _count;
        private double _maximum;
        private double _mean;
        private double _median;
        private double _minimum;
        private double _std;
        private double _sum;

        #endregion

        #region Methods

        /// <summary>
        /// resets all statistics to 0.
        /// </summary>
        public void Clear()
        {
            _count = 0;
            _minimum = 0;
            _maximum = 0;
            _mean = 0;
            _median = 0;
            _sum = 0;
            _std = 0;
        }

        /// <summary>
        /// Calculates the statistics for the specified values
        /// </summary>
        /// <param name="values"></param>
        public void Calculate(List<double> values)
        {
            Calculate(values, double.MinValue, double.MaxValue);
        }

        /// <summary>
        /// Calculates the statistics for the specified values
        /// </summary>
        /// <param name="values"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void Calculate(List<double> values, double min, double max)
        {
            values.Sort(); // Linear solutions for finding median can be problematic.  Easier to simply sort values.
            // Trim the extremes when calculating statistics.
            //while (values.Count > 0 && values[0] < min)
            //{
            //    values.RemoveAt(0);
            //}
            //while (values.Count > 0 && values[values.Count-1] > max)
            //{
            //    values.RemoveAt(values.Count - 1);
            //}

            if (values.Count == 0)
            {
                Clear();
                return;
            }
            if (values.Count % 2 == 0)
            {
                // In the even case, take the average of the two middle values
                int lowIndex = (values.Count - 1) / 2; //Divide by one less than values.Count to grab the middle two values.
                _median = (values[lowIndex] + values[lowIndex + 1]) / 2;
            }
            else
            {
                int index = values.Count / 2; // integer division causes 5-> 2, not 2.5.
                _median = values[index];
            }
            _count = values.Count;
            _minimum = values[0];
            _maximum = values[values.Count - 1];

            double total = 0;
            double sqrTotal = 0;
            foreach (double val in values)
            {
                total += val;
                sqrTotal += val * val;
            }
            _sum = total;
            _mean = total / _count;
            _std = Math.Sqrt((sqrTotal / _count) - (total / _count) * (total / _count));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the integer count of the values
        /// </summary>
        public int Count
        {
            get { return _count; }
            protected set { _count = value; }
        }

        /// <summary>
        /// Gets the highest value
        /// </summary>
        public double Maximum
        {
            get { return _maximum; }
            protected set { _maximum = value; }
        }

        /// <summary>
        /// Gets the average value
        /// </summary>
        public double Mean
        {
            get { return _mean; }
            protected set { _mean = value; }
        }

        /// <summary>
        /// Gets the middle value, or the average of the two middle values
        /// </summary>
        public double Median
        {
            get { return _median; }
            protected set { _median = value; }
        }

        /// <summary>
        /// Gets the minimum value
        /// </summary>
        public double Minimum
        {
            get { return _minimum; }
            protected set { _minimum = value; }
        }

        /// <summary>
        /// Gets the standard deviation
        /// </summary>
        public double StandardDeviation
        {
            get { return _std; }
            protected set { _std = value; }
        }

        /// <summary>
        /// Gets the sum of the values.
        /// </summary>
        public double Sum
        {
            get { return _sum; }
            protected set { _sum = value; }
        }

        #endregion
    }
}