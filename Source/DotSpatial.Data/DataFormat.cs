// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/12/2008 3:17:31 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// The data format.
    /// </summary>
    public enum DataFormat
    {
        /// <summary>
        /// Lines, Points and Polygons make up standard static vector formats.
        /// These are drawn dynamically based on the symbolizer.
        /// </summary>
        Vector,

        /// <summary>
        /// Rasters are grids of integers, doubles, floats, or other numeric value types.
        /// These can be symbolized and represented as images, but not drawn directly.
        /// </summary>
        Raster,

        /// <summary>
        /// Images specifically have pixels coordinates that store a color.
        /// These are drawn directly.
        /// </summary>
        Image,

        /// <summary>
        /// This represents an extended format that does not have a formal definition in DotSpatial.
        /// </summary>
        Custom
    }
}