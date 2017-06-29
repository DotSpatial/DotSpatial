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
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/16/2009 5:48:52 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Topology;

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
            IEnvelope ignoreMe;
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
        public static bool InvertSelection(this ISelectable self, IEnvelope tolerant, IEnvelope strict)
        {
            IEnvelope ignoreMe;
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
        public static bool InvertSelection(this IFeatureLayer self, IEnvelope tolerant, IEnvelope strict, out IEnvelope affectedArea)
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
        public static bool Select(this ISelectable self, IEnvelope tolerant, IEnvelope strict)
        {
            IEnvelope ignoreMe;
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
        public static bool Select(this IFeatureLayer self, IEnvelope tolerant, IEnvelope strict, out IEnvelope affectedArea)
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
        public static bool UnSelect(this ISelectable self, IEnvelope tolerant, IEnvelope strict)
        {
            IEnvelope ignoreMe;
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
        public static bool UnSelect(this IFeatureLayer self, IEnvelope tolerant, IEnvelope strict, out IEnvelope affectedArea)
        {
            return self.UnSelect(tolerant, strict, SelectionMode.Intersects, out affectedArea);
        }
    }
}