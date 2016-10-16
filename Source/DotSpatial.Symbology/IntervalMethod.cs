// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/28/2009 10:45:02 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IntervalMethods
    /// </summary>
    public enum IntervalMethod
    {
        /// <summary>
        /// The breaks are set to being evenly spaced.
        /// </summary>
        EqualInterval,

        /// <summary>
        /// The breaks are positioned to ensure close to equal quantities
        /// in each break. (each group contains approximately same number of values)
        /// </summary>
        EqualFrequency,

        /// <summary>
        /// Jenks natural breaks looks for "clumping" in the data and
        /// attempts to group according to the clumps.
        /// </summary>
        NaturalBreaks,

        /// <summary>
        /// Breaks start equally placed, but can be positioned manually instead.
        /// </summary>
        Manual,
    }
}