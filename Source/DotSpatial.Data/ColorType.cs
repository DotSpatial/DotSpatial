// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/18/2010 3:45:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// ColorType
    /// </summary>
    public enum ColorType : byte
    {
        /// <summary>
        /// Each pixel is a greyscale sample
        /// </summary>
        Greyscale = 0,
        /// <summary>
        /// Each pixel is an RGB triple
        /// </summary>
        Truecolor = 2,
        /// <summary>
        /// Each pixel is a palette index
        /// </summary>
        Indexed = 3,
        /// <summary>
        /// Each pixel is a greyscale sample followed by an alpha sample
        /// </summary>
        GreyscaleAlpha = 4,
        /// <summary>
        /// EAch pixel is an RGB triple followed by an alhpa sample
        /// </summary>
        TruecolorAlpha = 6
    }
}