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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/18/2010 9:57:27 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Data;
using DotSpatial.Projections;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// IDataLayer
    /// </summary>
    [Obsolete("Do not use it. This interface is not used in DotSpatial anymore.")] // Marked in 1.7
    public interface IDataLayer
    {
        #region Methods

        /// <summary>
        /// This should finalize the transaction, saving changes to the database or disk
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Creates a feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        bool CreateFeature(IFeature feature);

        /// <summary>
        /// Creates a feature from a shape and some data row values.
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        bool CreateShape(Shape shape, DataRow attributes);

        /// <summary>
        /// Creates a new field for this data layer in the data source
        /// </summary>
        /// <param name="fld"></param>
        /// <returns></returns>
        bool CreateField(Field fld);

        /// <summary>
        /// Deletes the feature at the specified index location
        /// </summary>
        /// <param name="fid">The fid</param>
        /// <returns></returns>
        bool DeleteFeature(int fid);

        /// <summary>
        /// Disposes any unmanaged memory objects
        /// </summary>
        void Dispose();

        /// <summary>
        /// Gets the feature at the specified index
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        IFeature GetFeature(int fid);

        /// <summary>
        /// Gets the shape
        /// </summary>
        /// <param name="fid"></param>
        /// <returns></returns>
        Shape GetShape(int fid);

        /// <summary>
        /// Gets the features
        /// </summary>
        /// <returns></returns>
        int GetFeatureCount();

        /// <summary>
        /// Gets the name of the feature ID column
        /// </summary>
        /// <returns></returns>
        string GetFidColumn();

        /// <summary>
        /// Gets the string name of the geometry column
        /// </summary>
        /// <returns></returns>
        string GetGeometryColumn();

        /// <summary>
        /// Returns the field definitions as an array of fields.
        /// </summary>
        /// <returns>An array of field objects that encompass the schema</returns>
        Field[] GetFields();

        /// <summary>
        /// Rolls back the transactions
        /// </summary>
        /// <returns></returns>
        bool RollbackTransaction();

        /// <summary>
        /// Gets the projection information associated with this projection
        /// </summary>
        /// <returns></returns>
        ProjectionInfo GetSpatialRef();

        /// <summary>
        /// sets the specified feature
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="feature"></param>
        /// <returns></returns>
        bool SetFeature(int fid, IFeature feature);

        /// <summary>
        /// Sets the shape
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="shape"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        bool SetShape(int fid, Shape shape, DataRow values);

        /// <summary>
        /// Sets the spatial filter by using the specified rectangular extents instead of a geometry.
        /// </summary>
        /// <param name="xMin">The minimum value in the X direction.</param>
        /// <param name="yMin">The minimum value in the Y direction.</param>
        /// <param name="xMax">The maximum value in the X direction.</param>
        /// <param name="yMax">The maximum value in the Y direction.</param>
        /// <returns>Boolean, true if hte spatial filter rectangle is set.</returns>
        bool SetSpatialFilterRect(double xMin, double yMin, double xMax, double yMax);

        /// <summary>
        /// Starts the transaction
        /// </summary>
        void StartTransaction();

        /// <summary>
        /// Syncrhonize to disk
        /// </summary>
        void SyncToDisk();

        /// <summary>
        /// Test Capability
        /// </summary>
        void TestCapability();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the extent of the data layer
        /// </summary>
        IEnvelope Extent
        {
            get;
        }

        /// <summary>
        /// Gets the name of this layer
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Gets or sets the spatial filter so that only members that intersect with the specified
        /// geometry will be returned.
        /// </summary>
        IGeometry SpatialFilter
        {
            get;
            set;
        }

        #endregion
    }
}