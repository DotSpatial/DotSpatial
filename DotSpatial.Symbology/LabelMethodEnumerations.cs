// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Brian Marchionni. Created 9/08/2009
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Methods used in calculating the placement of a label
    /// </summary>
    public enum LabelPlacementMethod
    {
        /// <summary>
        /// Use the centroid of the feature
        /// </summary>
        Centroid,

        /// <summary>
        /// Use the center of the extents of the feature
        /// </summary>
        Center,

        /// <summary>
        /// Use the closest point to the centroid that is in the feature
        /// </summary>
        InteriorPoint,
    }

    /// <summary>
    /// Determins if all parts should be labeled or just the largest
    /// </summary>
    public enum PartLabelingMethod
    {
        /// <summary>
        /// Label all parts
        /// </summary>
        LabelAllParts,

        /// <summary>
        /// Only label the largest part
        /// </summary>
        LabelLargestPart
    }
}