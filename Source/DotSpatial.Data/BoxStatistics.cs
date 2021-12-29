// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// Box-plot: Tukey 1977
    /// Basically it is like subdividing the data into 4 quartiles,
    /// denoted like Q1, Q2, Q3, Q4.
    /// -------
    /// Low Outlier (less than the Quartile - 3/2 Interquartile range)
    /// Q1 (Q1 - 3/2 the Interquartile range to Q1
    /// Q2 (Quartile 1 to Median)
    /// Q3 (Median to Quartile 3)
    /// Q4 (Q3 to Q3 + 3/2 the Interquartile range
    /// High Outlier (Greater than Q3 plus the interquartile range.
    /// </summary>
    public class BoxStatistics
    {
        /// <summary>
        /// Gets or sets the median between the median and the highest value.
        /// This separates the third and fourth quartiles.
        /// </summary>
        public object HighMedian { get; set; }

        /// <summary>
        /// Gets or sets the highest value that is not considered an outlier.
        /// If the values are not numeric, this will be the Maximum.
        /// </summary>
        public object HighWisker { get; set; }

        /// <summary>
        /// Gets or sets the median between the lowest value and the median value.
        /// This separates the first quartile from the second.
        /// </summary>
        public object LowMedian { get; set; }

        /// <summary>
        /// Gets or sets the lowest value that is not considered to be an outlier.
        /// If the values are not numeric, this will be the minimum.
        /// </summary>
        public object LowWhisker { get; set; }

        /// <summary>
        /// Gets or sets the Median value (the value of the middle member).
        /// </summary>
        public object Median { get; set; }
    }
}