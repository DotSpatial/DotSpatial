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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/10/2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |-----------------|----------|---------------------------------------------------------------------
// |      Name       |  Date    |                        Comments
// |-----------------|----------|----------------------------------------------------------------------
// | Kyle Ellison    |10/13/2010| Added EndGetShapesSession() to free shx resources.
// | Kyle Ellison    |12/15/2010| Added method to get multiple shapes by index values, and consolidated code.
// |-----------------|----------|----------------------------------------------------------------------
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// Rendering is considered a different pathway from editing.  For rendering, we can distance
    /// ourselves from the attributes all together in order to speedily grab just the indices
    /// that are within the view extent.
    /// </summary>
    public interface IShapeSource
    {
        /// <summary>
        /// This is most likely set once per source.
        /// </summary>
        FeatureType FeatureType { get; }

        /// <summary>
        /// Returns a dictionary with the FID and Shape, but only returns shapes within the envelope.
        /// </summary>
        /// <param name="startIndex">The integer offset of the first shape to test.  When this returns, the offset is set to the integer index of the last shape tested, regardless of whether or not it was returned.</param>
        /// <param name="count">The integer count of the maximum number of shapes to return here. </param>
        /// <param name="envelope">The geographic extents that can be used to limit the shapes.  If this is null, then no envelope is used.</param>
        /// <returns>The Dictionary with FID indices and Shape values</returns>
        Dictionary<int, Shape> GetShapes(ref int startIndex, int count, IEnvelope envelope);

        /// <summary>
        /// Returns array of shapes at the specified indices
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        Shape[] GetShapes(int[] indices);

        /// <summary>
        /// Gets the integer count of the total number of shapes in the set.  This may run a query,
        /// so it is better to cache the result than call this repeatedly.
        /// </summary>
        /// <returns>The integer number of shapes in the entire source.</returns>
        int GetShapeCount();

        /// <summary>
        /// Should be called when done making repeated calls to GetShapes to free internal resources.
        /// </summary>
        void EndGetShapesSession();
    }
}