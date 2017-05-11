// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. 2/17/2008 12:28:12 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// An enumeration specifying the way that a gradient of color is attributed to the values in the specified range.
    /// </summary>
    public enum GradientModel
    {
        /// <summary>
        /// The values are colored in even steps in each of the Red, Green and Blue bands.
        /// </summary>
        Linear,

        /// <summary>
        /// The even steps between values are used as powers of two, greatly increasing the impact of higher values.
        /// </summary>
        Exponential,

        /// <summary>
        /// The log of the values is used, reducing the relative impact of the higher values in the range.
        /// </summary>
        Logarithmic
    }
}