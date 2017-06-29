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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2009 4:05:32 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// SelectionModes
    /// </summary>
    public enum SelectionMode
    {
        /// <summary>
        /// The vertices of the feature are all inside (and not touching the boundary of) the region.
        /// </summary>
        /// <remarks>
        /// Use ContainsExtent for faster selections.
        /// </remarks>
        Contains,

        /// <summary>
        /// *Faster, the item to be selected must be completely contained by the extent
        /// </summary>
        ContainsExtent,

        /// <summary>
        /// The entire region is inside the feature and not touching the border.
        /// </summary>
        CoveredBy,

        /// <summary>
        /// The feature is completely inside the region, but touching the edges is ok (unlike Contains).
        /// </summary>
        Covers,

        /// <summary>
        /// For this to be true the feature must be a point or a curve and must cross
        /// the specified
        /// </summary>
        Crosses,

        /// <summary>
        /// The interiors and boundaries don't intersect, touch, or overlap, so they are
        /// completely separate.
        /// </summary>
        Disjoint,

        /// <summary>
        /// *Faster, the item will be selected if any part of that item is visible in the extent
        /// </summary>
        IntersectsExtent,

        /// <summary>
        /// The most inclusive possible.  If any element is touching or overlapping the region in any
        /// way, then this will be true.
        /// </summary>
        Intersects,

        /// <summary>
        /// Like intersects, specifically requires that part of the feature is inside
        /// and part of the feature is outside of the region.
        /// </summary>
        Overlaps,

        /// <summary>
        /// The feature has borders or edges in common, but otherwise does not fall inside
        /// the region.
        /// </summary>
        Touches,

        /// <summary>
        /// The region is found completely inside the specified region to the extent
        /// that no borders come in contact with the outside edge of the region.
        /// </summary>
        Within
    }
}