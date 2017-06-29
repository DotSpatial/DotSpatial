// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// A feature doesn't need to be abstract because the geometry is generic and the other
    /// properties are all the same.  It supports IRenderable so that even if you don't
    /// know what type of feature this is, you can still tell it to draw itself.  You won't
    /// be able to specify any drawing characteristics from this object however.
    /// </summary>
    public interface IFeature : IBasicGeometry, IComparable<IFeature>
    {
        /// <summary>
        /// Gets the datarow containing all the attributes related to this geometry
        /// </summary>
        DataRow DataRow
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a valid IBasicGeometry associated with the data elements of this feature.
        /// This will be enough geometry information to cast into a full fledged geometry
        /// that can be used in coordination with DotSpatial.Analysis
        /// </summary>
        IBasicGeometry BasicGeometry
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content length.  If the geometry for this shape was loaded from a file, this contains the size
        /// of this shape in 16-bit words as per the Esri Shapefile specification.
        /// </summary>
        int ContentLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a DotSpatial.Data.CacheTypes enumeration specifying whether the Envelope property
        /// returns a cached value in this object or is retrieved directly from the geometry.  The
        /// initial case for Shapefiles is to use a cache.  Setting the envelope assumes that you
        /// are going to use a cached value and will set this to Cached.  Setting this to Dynamic
        /// will cause the Envelope property to reference the geometry.
        /// </summary>
        CacheTypes EnvelopeSource
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the key that is associated with this feature.  This returns -1 if
        /// this feature is not a member of a feature layer.
        /// </summary>
        int Fid
        {
            get;
        }

        /// <summary>
        /// Gets a reference to the IFeatureLayer that contains this item.
        /// </summary>
        IFeatureSet ParentFeatureSet
        {
            get;
            set;
        }

        /// <summary>
        /// An index value that is saved in some file formats.
        /// </summary>
        int RecordNumber
        {
            get;
            set;
        }

        /// <summary>
        /// When a shape is loaded from a Shapefile, this will identify whether M or Z values are used
        /// and whether or not the shape is null.
        /// </summary>
        ShapeType ShapeType
        {
            get;
            set;
        }

        /// <summary>
        /// This is simply a quick access to the Vertices list for this specific
        /// feature.  If the Vertices have not yet been defined, this will be null.
        /// </summary>
        ShapeRange ShapeIndex
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a deep copy of this feature.  the new datarow created will not be connected
        /// to a data Table, so it should be added to one.
        /// </summary>
        /// <returns>Returns a deep copy of this feature as an IFeature</returns>
        IFeature Copy();

        /// <summary>
        /// Creates a new shape based on this feature by itself.
        /// </summary>
        /// <returns>A Shape object</returns>
        Shape ToShape();

        /// <summary>
        /// This uses the field names to copy attribute values from the source to this feature.
        /// Even if columns are missing or if there are extra columns, this method should work.
        /// </summary>
        /// <param name="source">The IFeature source to copy attributes from.</param>
        void CopyAttributes(IFeature source);
    }
}