// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/5/2010 6:59:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// ********************************************************************************************************

using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// This interface allows access to geometries and attributes in a more cohesive package.
    /// </summary>
    public interface IFeatureRow
    {
        /// <summary>
        /// This is a cached extent.  This will not reflect changes in Geometry or Shape unless
        /// they have been stored, but will reflect the most recently stored or retrieved
        /// extent from accessing either the shape or geometry.  In cases where reading from
        /// WKB, this will be lazily created, but it will be cached when reading from
        /// shapefiles.
        /// </summary>
        IExtent Extent { get; set; }

        /// <summary>
        /// A Geometry created lazily from a WKB byte structure in the GEOMETRY field preferentially
        /// or if the field is DBNull and the shape is not null, then from the Shape object.
        /// </summary>
        IGeometry Geometry { get; set; }

        /// <summary>
        /// A Shape that is either created lazily from the WKB Geometry or directly from a shapefile.
        /// </summary>
        Shape Shape { get; set; }

        /// <summary>
        /// The rest of the attributes are stored by an indexed property.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        object this[string fieldName] { get; set; }

        /// <summary>
        /// Boolean, true if the geometry has already been created, false otherwise.
        /// </summary>
        bool IsGeometryCreated { get; }

        /// <summary>
        /// Boolean, true if the shape has already been created, false otherwise.
        /// </summary>
        bool IsShapeCreated { get; }

        /// <summary>
        /// Boolean, true if the extent has been created, false otherwise.
        /// </summary>
        bool IsExtentCreated { get; }

        /// <summary>
        /// Stores the shape to the GEOMETRY WKB.
        /// </summary>
        void StoreShape();

        /// <summary>
        /// Stores the geometry to the geometry wkb.
        /// </summary>
        void StoreGeometry();
    }
}