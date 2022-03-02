// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace DotSpatial.Data
{
    /// <summary>
    /// Rendering is considered a different pathway from editing. For rendering, we can distance ourselves from
    /// the attributes all together in order to speedily grab just the indices that are within the view extent.
    /// </summary>
    public interface IShapeSource
    {
        /// <summary>
        /// Gets the feature type of the features in the IShapeSource.
        /// </summary>
        FeatureType FeatureType { get; }

        /// <summary>
        /// Returns a dictionary with the FID and Shape, but only returns shapes within the envelope.
        /// </summary>
        /// <param name="startIndex">The integer offset of the first shape to test. When this returns, the offset is set to the integer index of the last shape tested, regardless of whether or not it was returned.</param>
        /// <param name="count">The integer count of the maximum number of shapes to return here. </param>
        /// <param name="envelope">The geographic extents that can be used to limit the shapes. If this is null, then no envelope is used.</param>
        /// <returns>The Dictionary with FID indices and Shape values.</returns>
        Dictionary<int, Shape> GetShapes(ref int startIndex, int count, Envelope envelope);

        /// <summary>
        /// Returns an array of the shapes with the specified indices.
        /// </summary>
        /// <param name="indices">Indices of the shapes that should be returned.</param>
        /// <returns>An array of the shapes with the specified indices.</returns>
        Shape[] GetShapes(int[] indices);

        /// <summary>
        /// Gets the integer count of the total number of shapes in the set.
        /// This may run a query, so it is better to cache the result than call this repeatedly.
        /// </summary>
        /// <returns>The integer number of shapes in the entire source.</returns>
        int GetShapeCount();

        /// <summary>
        /// Should be called when done making repeated calls to GetShapes to free internal resources.
        /// </summary>
        void EndGetShapesSession();
    }
}