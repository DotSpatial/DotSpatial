// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
// The Initial Developer of this Original Code is Ted Dunsford. Created Before 11/25/2010 5:26 PM
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// An interface for specifying something has a Bounds property on it that is a raster bounds.
    /// </summary>
    public interface IContainRasterBounds
    {
        /// <summary>
        /// Gets or sets the image bounds being used to define the georeferencing of the image
        /// </summary>
        IRasterBounds Bounds { get; set; }
    }
}