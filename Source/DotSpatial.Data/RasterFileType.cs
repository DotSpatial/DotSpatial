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
        Ascii,

        /// <summary>
        /// Binary interlaced Layers
        /// </summary>
        Bil,

        /// <summary>
        /// BGD (Original DotSpatial format)
        /// </summary>
        Binary,

        /// <summary>
        /// DTED
        /// </summary>
        Dted,

        /// <summary>
        /// Wavelet format
        /// </summary>
        Ecw,

        /// <summary>
        /// ArcGIS format
        /// </summary>
        Esri,

        /// <summary>
        /// FLT
        /// </summary>
        Flt,

        /// <summary>
        /// GeoTiff
        /// </summary>
        GeoTiff,

        /// <summary>
        /// SID
        /// </summary>
        MrSid,

        /// <summary>
        /// AUX
        /// </summary>
        Paux,

        /// <summary>
        /// PCIDsk
        /// </summary>
        PciDsk,

        /// <summary>
        /// SDTS
        /// </summary>
        Sdts,

        /// <summary>
        /// Custom - specified as string
        /// </summary>
        Custom
    }
}