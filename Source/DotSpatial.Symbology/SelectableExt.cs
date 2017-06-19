// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Extends the ISelectable interface with some overloads that ignore the output affected areas
    /// </summary>
    public static class SelectableExt
    {
        /// <summary>
        /// Clears the selection, and ignores the affected area.
        /// </summary>
        /// <param name="self">This selectable item</param>
        /// <returns>Boolean, true if members were removed from the selection</returns>
        public static bool ClearSelection(this ISelectable self)
        {
            Envelope ignoreMe;
            return self.ClearSelection(out ignoreMe);
        }

        /// <summary>
        /// This ignores the affected region and assumes that you want to use a selection based on
        /// the Intersects selection mode.
        /// </summary>
        /// <param name="self">The ISelectable object</param>
        /// <param name="tolerant">The region where selection should take place</param>
        /// <param name="strict">The region in cases where tolerance is not used</param>
        /// <returns>Boolean, true if the selected state of any members of this item were altered</returns>
        public static bool InvertSelection(this ISelectable self, Envelope tolerant, Envelope strict)
        {
            Envelope ignoreMe;
            return self.InvertSelection(tolerant, strict, SelectionMode.Intersects, out ignoreMe);
        }

        /// <summary>
        /// Inverts the selection state of members that intersect the specified region.
        /// The affected area will be returned.
        /// </summary>
        /// <param name="self">The IFeatureLayer to modify the selection for</param>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="affectedArea">The affected area to modify</param>
        /// <returns>Boolean, true if the selection state was modified by this action</returns>
        public static bool InvertSelection(this IFeatureLayer self, Envelope tolerant, Envelope strict, out Envelope affectedArea)
        {
            return self.InvertSelection(tolerant, strict, SelectionMode.Intersects, out affectedArea);
        }

        /// <summary>
        /// This ignores the affected region and assumes that you want to use a selection based on
        /// the Intersects selection mode.
        /// </summary>
        /// <param name="self">This ISelectable</param>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <returns>Boolean, true if any items were added to the selection</returns>
        public static bool Select(this ISelectable self, Envelope tolerant, Envelope strict)
        {
            Envelope ignoreMe;
            return self.Select(tolerant, strict, SelectionMode.Intersects, out ignoreMe);
        }

        /// <summary>
        /// Highlights the values in the specified region, and returns the affected area from the selection,
        /// which should allow for slightly faster drawing in cases where only a small area is changed.
        /// </summary>
        /// <param name="self">The IFeatureLayer from which to select features</param>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="affectedArea">The geographic envelope of the region impacted by the selection.</param>
        /// <returns>True if any members were added to the current selection.</returns>
        public static bool Select(this IFeatureLayer self, Envelope tolerant, Envelope strict, out Envelope affectedArea)
        {
            return self.Select(tolerant, strict, SelectionMode.Intersects, out affectedArea);
        }

        /// <summary>
        /// This ignores the affected region and assumes that you want to use a selection based on
        /// the Intersects selection mode.
        /// </summary>
        /// <param name="self">This ISelectable</param>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <returns>Boolean, true if any items were added to the selection</returns>
        public static bool UnSelect(this ISelectable self, Envelope tolerant, Envelope strict)
        {
            Envelope ignoreMe;
            return self.UnSelect(tolerant, strict, SelectionMode.Intersects, out ignoreMe);
        }

        /// <summary>
        /// Un-highlights or returns the features that intersect with the specified region.
        /// </summary>
        /// <param name="self">The IFeatureLayer from which to unselect features.</param>
        /// <param name="tolerant">The geographic envelope in cases like cliking near points where tolerance is allowed.</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance.</param>
        /// <param name="affectedArea">The geographic envelope that will be visibly impacted by the change.</param>
        /// <returns>Boolean, true if members were removed from the selection.</returns>
        public static bool UnSelect(this IFeatureLayer self, Envelope tolerant, Envelope strict, out Envelope affectedArea)
        {
            return self.UnSelect(tolerant, strict, SelectionMode.Intersects, out affectedArea);
        }
    }
}