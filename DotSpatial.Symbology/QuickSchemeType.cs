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
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/23/2009 1:58:39 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// QuickSchemeTypes
    /// </summary>
    public enum QuickSchemeType
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
        /// High Outlier (Greater than Q3 plus the interquartile range
        /// </summary>
        Box,

        /// <summary>
        /// The interval size is defined, rather than the number of subdivisions
        /// </summary>
        DefinedInterval,

        /// <summary>
        /// Symbolize so that the total range of values for a specific field are subdivided into
        /// a number of breaks where all the breaks cover the same difference in values.
        /// </summary>
        EqualInterval,

        /// <summary>
        /// Intervals are selected so that the number of observations in each successive interval
        /// increases or decreases exponentially
        /// </summary>
        Exponential,

        /// <summary>
        /// The Jenks natural breaks algorithm strives for variance minimization within
        /// a classification.
        /// </summary>
        NaturalBreaks,

        /// <summary>
        /// Percentile subdivides into the following six categories:
        /// less than 1%,
        /// 1% to just less than 10%,
        /// 10% to just less than 50%
        /// 50% to just less than 90%
        /// 90% to just less than 99%
        /// greater than or equal to 99%
        /// </summary>
        Percentile,

        /// <summary>
        /// The number of members found in each cateogry is the same, but the range might be
        /// widely different for datasets that don't have equally distributed values.
        /// </summary>
        Quantile,

        /// <summary>
        /// The mean and standard deviation of the attribute values are calculated, and
        /// values are classified according to their deviation from the mean (z-transform)
        /// The transformed values are mapped, usually at intervales of 1.0 or .5 standard
        /// deviations.  This often results in no central class.
        /// </summary>
        StandardDeviation,

        /// <summary>
        /// Symbolize so that each separate entry will have a separate category for that entry.
        /// </summary>
        UniqueValues
    }
}