// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:42:49 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

using GeoAPI.Geometries;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// The <c>SelectedShape</c> interface is used to access information about a shape that is selected in the DotSpatial.
    /// </summary>
    public interface ISelectedShape
    {
        #region Properties

        /// <summary>
        /// Gets the extents of this selected shape.
        /// </summary>
        Envelope Extents { get; }

        /// <summary>
        /// Gets the shape index of this selected shape.
        /// </summary>
        int ShapeIndex { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes all information in the <c>SelectedShape</c> object then highlights the shape on the map.
        /// </summary>
        /// <param name="shapeIndex">Index of the shape in the shapefile.</param>
        /// <param name="selectColor">Color to use when highlighting the shape.</param>
        void Add(int shapeIndex, Color selectColor);

        #endregion
    }
}