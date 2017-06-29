// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:41:18 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections;
using System.Drawing;
using DotSpatial.Topology;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// This interface is used to manage and access all selected shapes.
    /// </summary>
    /// <remarks>All selection is done only to the selected layer.  The selected layer handle can be accessed using the <c>LayerHandle</c> property.</remarks>
    public interface ISelectInfo : IEnumerable
    {
        /// <summary>
        /// Returns the LayerHandle of the selected layer.
        /// </summary>
        int LayerHandle { get; }

        /// <summary>
        /// Returns the number of shapes that are currently selected.
        /// </summary>
        int NumSelected { get; }

        /// <summary>
        /// Returns the total extents of all selected shapes.
        /// </summary>
        Envelope SelectBounds { get; }

        /// <summary>
        /// Returns the <c>SelectedShape</c> at the specified index.
        /// </summary>
        ISelectedShape this[int index] { get; }

        /// <summary>
        /// Adds a <c>SelectedShape</c> object to the managed collection of all selected shapes.
        /// </summary>
        /// <param name="newShape">The <c>SelectedShape</c> object to add.</param>
        void AddSelectedShape(ISelectedShape newShape);

        /// <summary>
        /// Adds a new <c>SelectedShape</c> to the collection from the provided shape index.
        /// </summary>
        /// <param name="shapeIndex">The index of the shape to add.</param>
        /// <param name="selectColor">The color to use when highlighting the shape.</param>
        void AddByIndex(int shapeIndex, Color selectColor);

        /// <summary>
        /// Clears the list of selected shapes, returning each selected shape to it's original color.
        /// </summary>
        void ClearSelectedShapes();

        /// <summary>
        /// Removes a <c>SelectedShape</c> from the collection, reverting it to it's original color.
        /// </summary>
        /// <param name="listIndex">Index in the collection of the <c>SelectedShape</c>.</param>
        void RemoveSelectedShape(int listIndex);

        /// <summary>
        /// Removes a <c>SelectedShape</c> from the collection, reverting it to it's original color.
        /// </summary>
        /// <param name="shapeIndex">The shape index of the <c>SelectedShape</c> to remove.</param>
        void RemoveByShapeIndex(int shapeIndex);
    }
}