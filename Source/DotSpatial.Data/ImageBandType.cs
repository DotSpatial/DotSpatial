// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/9/2009 2:07:29 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// An enumeration listing the types of image band interpretations supported.
    /// </summary>
    public enum ImageBandType
    {
        /// <summary>
        /// Alpha, Red, Green, Blue 4 bytes per pixel, 4 bands
        /// </summary>
        ARGB,

        /// <summary>
        ///  Red, Green, Blue 3 bytes per pixel, 3 bands
        /// </summary>
        RGB,

        /// <summary>
        /// Gray as one byte per pixel, 1 band
        /// </summary>
        Gray,

        /// <summary>
        /// Colors encoded 1 byte per pixel, 1 band
        /// </summary>
        PalletCoded
    }
}