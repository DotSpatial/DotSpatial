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
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/23/2009 4:03:53 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    //This is the testing comment by Jiri Kadlec

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
    /// High Outlier (Greater than Q3 plus the interquartile range
    /// </summary>
    public class BoxStatistics
    {
        /// <summary>
        /// The median between the median and the highest value.
        /// This separates the third and fourth quartiles.
        /// </summary>
        public object HighMedian;

        /// <summary>
        /// The highest value that is not considered an outlier.
        /// If the values are not numeric, this will be the Maximum.
        /// </summary>
        public object HighWisker;

        /// <summary>
        /// The median between the lowest value and the median value.
        /// This separates the first quartile from the second.
        /// </summary>
        public object LowMedian;

        /// <summary>
        /// The lowest value that is not considered to be an outlier.
        /// If the values are not numeric, this will be the minimum.
        /// </summary>
        public object LowWhisker;

        /// <summary>
        /// The Median value (the value of the middle member)
        /// </summary>
        public object Median;
    }
}