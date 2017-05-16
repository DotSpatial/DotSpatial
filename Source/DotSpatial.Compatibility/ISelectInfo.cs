// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections;
using System.Drawing;

using GeoAPI.Geometries;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// This interface is used to manage and access all selected shapes.
    /// </summary>
    /// <remarks>All selection is done only to the selected layer. The selected layer handle can be accessed using the <c>LayerHandle</c> property.</remarks>
    public interface ISelectInfo : IEnumerable
    {
        #region Properties

        /// <summary>
        /// Gets the LayerHandle of the selected layer.
        /// </summary>
        int LayerHandle { get; }

        /// <summary>
        /// Gets the number of shapes that are currently selected.
        /// </summary>
        int NumSelected { get; }

        /// <summary>
        /// Gets the total extents of all selected shapes.
        /// </summary>
        Envelope SelectBounds { get; }

        #endregion

        /// <summary>
        /// Returns the <c>SelectedShape</c> at the specified index.
        /// </summary>
        /// <param name="index">Index of the element that should be returned.</param>
        ISelectedShape this[int index] { get; }

        #region Methods

        /// <summary>
        /// Adds a new <c>SelectedShape</c> to the collection from the provided shape index.
        /// </summary>
        /// <param name="shapeIndex">The index of the shape to add.</param>
        /// <param name="selectColor">The color to use when highlighting the shape.</param>
        void AddByIndex(int shapeIndex, Color selectColor);

        /// <summary>
        /// Adds a <c>SelectedShape</c> object to the managed collection of all selected shapes.
        /// </summary>
        /// <param name="newShape">The <c>SelectedShape</c> object to add.</param>
        void AddSelectedShape(ISelectedShape newShape);

        /// <summary>
        /// Clears the list of selected shapes, returning each selected shape to it's original color.
        /// </summary>
        void ClearSelectedShapes();

        /// <summary>
        /// Removes a <c>SelectedShape</c> from the collection, reverting it to it's original color.
        /// </summary>
        /// <param name="shapeIndex">The shape index of the <c>SelectedShape</c> to remove.</param>
        void RemoveByShapeIndex(int shapeIndex);

        /// <summary>
        /// Removes a <c>SelectedShape</c> from the collection, reverting it to it's original color.
        /// </summary>
        /// <param name="listIndex">Index in the collection of the <c>SelectedShape</c>.</param>
        void RemoveSelectedShape(int listIndex);

        #endregion
    }
}