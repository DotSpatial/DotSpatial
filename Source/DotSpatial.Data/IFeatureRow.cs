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

using GeoAPI.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// This interface allows access to geometries and attributes in a more cohesive package.
    /// </summary>
    public interface IFeatureRow
    {
        #region Properties

        /// <summary>
        /// Gets or sets a cached extent. This will not reflect changes in Geometry or Shape unless
        /// they have been stored, but will reflect the most recently stored or retrieved
        /// extent from accessing either the shape or geometry. In cases where reading from
        /// WKB, this will be lazily created, but it will be cached when reading from
        /// shapefiles.
        /// </summary>
        IExtent Extent { get; set; }

        /// <summary>
        /// Gets or sets a Geometry created lazily from a WKB byte structure in the GEOMETRY field preferentially
        /// or if the field is DBNull and the shape is not null, then from the Shape object.
        /// </summary>
        IGeometry Geometry { get; set; }

        /// <summary>
        /// Gets a value indicating whether the extent has been created.
        /// </summary>
        bool IsExtentCreated { get; }

        /// <summary>
        /// Gets a value indicating whether the geometry has already been created.
        /// </summary>
        bool IsGeometryCreated { get; }

        /// <summary>
        /// Gets a value indicating whether the shape has already been created.
        /// </summary>
        bool IsShapeCreated { get; }

        /// <summary>
        /// Gets or sets a Shape that is either created lazily from the WKB Geometry or directly from a shapefile.
        /// </summary>
        Shape Shape { get; set; }

        #endregion

        #region Indexers

        /// <summary>
        /// The rest of the attributes are stored by an indexed property.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <returns>The field value.</returns>
        object this[string fieldName] { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Stores the geometry to the geometry wkb.
        /// </summary>
        void StoreGeometry();

        /// <summary>
        /// Stores the shape to the GEOMETRY WKB.
        /// </summary>
        void StoreShape();

        #endregion
    }
}