// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Data;
using GeoAPI.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// A feature doesn't need to be abstract because the geometry is generic and the other
    /// properties are all the same. It supports IRenderable so that even if you don't
    /// know what type of feature this is, you can still tell it to draw itself. You won't
    /// be able to specify any drawing characteristics from this object however.
    /// </summary>
    public interface IFeature : ICloneable, IComparable<IFeature>
    {
        #region Properties

        /// <summary>
        /// Gets or sets the datarow containing all the attributes related to this geometry
        /// </summary>
        DataRow DataRow { get; set; }

        /// <summary>
        /// Gets the FeatureType of the feature. This can either be Point, Multipoint, Line, Polygon or Unspecified if the feature has no geometry.
        /// </summary>
        FeatureType FeatureType { get; }

        /// <summary>
        /// Gets the key that is associated with this feature. This returns -1 if
        /// this feature is not a member of a feature layer.
        /// </summary>
        int Fid { get; }

        /// <summary>
        /// Gets or sets a valid IBasicGeometry associated with the data elements of this feature.
        /// This will be enough geometry information to cast into a full fledged geometry
        /// that can be used in coordination with DotSpatial.Analysis
        /// </summary>
        IGeometry Geometry { get; set; }

        /// <summary>
        /// Gets or sets a reference to the IFeatureLayer that contains this item.
        /// </summary>
        IFeatureSet ParentFeatureSet { get; set; }

        /// <summary>
        /// Gets or sets a quick access to the Vertices list for this specific
        /// feature. If the Vertices have not yet been defined, this will be null.
        /// </summary>
        ShapeRange ShapeIndex { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a deep copy of this feature. the new datarow created will not be connected
        /// to a data Table, so it should be added to one.
        /// </summary>
        /// <returns>Returns a deep copy of this feature as an IFeature</returns>
        IFeature Copy();

        /// <summary>
        /// This uses the field names to copy attribute values from the source to this feature.
        /// Even if columns are missing or if there are extra columns, this method should work.
        /// </summary>
        /// <param name="source">The IFeature source to copy attributes from.</param>
        void CopyAttributes(IFeature source);

        /// <summary>
        /// Creates a new shape based on this feature by itself.
        /// </summary>
        /// <returns>A Shape object</returns>
        Shape ToShape();

        /// <summary>
        /// Forces the features geometry to update its envelope.
        /// </summary>
        void UpdateEnvelope();

        #endregion
    }
}