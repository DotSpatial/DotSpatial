// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 8:22:45 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// RasterFileTypes
    /// </summary>
    public enum RasterFileType
    {
        /// <summary>
        /// Ascii
        /// </summary>
        ASCII,
        /// <summary>
        /// Binary interlaced Layers
        /// </summary>
        BIL,
        /// <summary>
        /// BGD (Original DotSpatial format)
        /// </summary>
        BINARY,
        /// <summary>
        /// DTED
        /// </summary>
        DTED,
        /// <summary>
        /// Wavelet format
        /// </summary>
        ECW,
        /// <summary>
        /// ArcGIS format
        /// </summary>
        ESRI,
        /// <summary>
        /// FLT
        /// </summary>
        FLT,
        /// <summary>
        /// GeoTiff
        /// </summary>
        GeoTiff,
        /// <summary>
        /// SID
        /// </summary>
        MrSID,
        /// <summary>
        /// AUX
        /// </summary>
        PAUX,
        /// <summary>
        /// PCIDsk
        /// </summary>
        PCIDsk,
        /// <summary>
        /// SDTS
        /// </summary>
        SDTS,
        /// <summary>
        /// Custom - specified as string
        /// </summary>
        CUSTOM
    }
}