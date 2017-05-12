// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/5/2010 6:59:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************
namespace DotSpatial.Data
{
    /// <summary>
    /// A very simple, 2D extent specification.
    /// </summary>
    public interface IExtent
    {
        /// <summary>
        /// Gets or sets the minimum in the X dimension, usually left or minimum longitude.
        /// </summary>
        double MinX { get; set; }

        /// <summary>
        /// Gets or sets the maximum in the x dimension, usually right or maximum longitude.
        /// </summary>
        double MaxX { get; set; }

        /// <summary>
        /// Gets or sets the minimum in the y dimension, usually the bottom or minimum latitude.
        /// </summary>
        double MinY { get; set; }

        /// <summary>
        /// Gets or sets the maximum in the y dimension, usually the top or maximum latitude.
        /// </summary>
        double MaxY { get; set; }

        /// <summary>
        /// Gets a value indicating whether the Min and Max M range should be used.
        /// </summary>
        bool HasM { get; }

        /// <summary>
        /// Gets a value indicating whether the Z value should be used.
        /// </summary>
        bool HasZ { get; }
    }
}