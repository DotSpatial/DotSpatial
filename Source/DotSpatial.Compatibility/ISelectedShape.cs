// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

using NetTopologySuite.Geometries;

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