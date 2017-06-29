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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:42:49 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.Topology;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// The <c>SelectedShape</c> interface is used to access information about a shape that is selected in the DotSpatial.
    /// </summary>
    public interface ISelectedShape
    {
        /// <summary>
        /// Returns the extents of this selected shape.
        /// </summary>
        Envelope Extents { get; }

        /// <summary>
        /// Returns the shape index of this selected shape.
        /// </summary>
        int ShapeIndex { get; }

        /// <summary>
        /// Initializes all information in the <c>SelectedShape</c> object then highlights the shape on the map.
        /// </summary>
        /// <param name="shapeIndex">Index of the shape in the shapefile.</param>
        /// <param name="selectColor">Color to use when highlighting the shape.</param>
        void Add(int shapeIndex, Color selectColor);
    }
}