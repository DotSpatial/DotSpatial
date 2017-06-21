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
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/9/2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// | Kyle Ellison    |12/08/2010| Added ability to edit multiple rows in one call for performance
// | Kyle Ellison    |12/10/2010| Added Extent property
// ********************************************************************************************************

using System.Collections.Generic;
using System.Data;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// A new interface for accessing database or file features to large for memory.  This allows editing only for attribute content,
    /// but allows Add, Remove, AddRange, and Query functionality that covers both vector and raster content.
    /// </summary>
    public interface IFeatureSource
    {
        ///<summary>
        /// The geographic extent of the feature source
        ///</summary>
        Extent Extent { get; }

        /// <summary>
        /// Get the attribute table associated with the feature source
        /// </summary>
        AttributeTable AttributeTable { get; }

        /// <summary>
        /// Updates the file with an additional feature.
        /// </summary>
        /// <param name="feature"></param>
        void Add(IFeature feature);

        /// <summary>
        /// Removes the feature with the specified index
        /// </summary>
        /// <param name="index">Removes the feature from the specified index location</param>
        void RemoveAt(int index);

        /// <summary>
        /// The default implementation calls the add method repeatedly.
        /// </summary>
        /// <param name="features">The set of features to add</param>
        void AddRange(IEnumerable<IFeature> features);

        /// <summary>
        /// Select function passed with a filter expression.
        /// </summary>
        /// <param name="filterExpression">The string filter expression based on attributes</param>
        /// <param name="envelope">The envelope for vector filtering.</param>
        /// <param name="startIndex">The integer start index where values should be checked.  This will be updated to the </param>
        /// last index that was checked before the return, so that paging the query can begin with that index in the next cycle.
        /// Be sure to add one before the next call.
        /// <param name="maxCount">The integer maximum number of IFeature values that should be returned.</param>
        /// <returns>A dictionary with FID keys and IFeature values.</returns>
        IFeatureSet Select(string filterExpression, IEnvelope envelope, ref int startIndex, int maxCount);

        /// <summary>
        /// Conditionally modify attributes as searched in a single pass via client supplied callback.
        /// </summary>
        /// <param name="envelope">The envelope for vector filtering.</param>
        /// <param name="chunkSize">Number of shapes to request from the ShapeSource in each chunk</param>
        /// <param name="rowCallback">Callback on each feature</param>
        void SearchAndModifyAttributes(IEnvelope envelope, int chunkSize, FeatureSourceRowEditEvent rowCallback);

        /// <summary>
        /// Edits the values of the specified row in the attribute table.  Since not all data formats
        /// can handle the dynamic change of attributes, updating vectors can be done with Remove and Add.
        /// </summary>
        /// <param name="fid">The feature offest</param>
        /// <param name="attributeValues">The row of new attribute values.</param>
        void EditAttributes(int fid, DataRow attributeValues);

        /// <summary>
        /// Edits the values of the specified rows in the attribute table.
        /// </summary>
        /// <param name="indexDataRowPairs"></param>
        void EditAttributes(IEnumerable<KeyValuePair<int, DataRow>> indexDataRowPairs);

        /// <summary>
        /// Adding and removing shapes may make the bounds for the entire shapefile invalid.
        /// This triggers a check that ensures that the collective extents contain all the shapes.
        /// </summary>
        void UpdateExtents();
    }
}