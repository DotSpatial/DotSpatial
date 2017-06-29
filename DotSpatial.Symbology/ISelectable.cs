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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/18/2009 2:23:17 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This supports the FeatureLayer event SelectionChanged, for both groups and feature layers, or
    /// any other layer that supports both an ISelection and inherits from IFeatureLayer.
    /// </summary>
    public interface ISelectable
    {
        #region Events

        /// <summary>
        /// OCcurs after all of the layers have been updated with new selection content.
        /// </summary>
        event EventHandler SelectionChanged;

        #endregion

        #region Methods

        /// <summary>
        /// Removes any members from existing in the selected state
        /// </summary>
        bool ClearSelection(out IEnvelope affectedArea);

        /// <summary>
        /// Inverts the selected state of any members in the specified region.
        /// </summary>
        /// <param name="tolerant">The geographic region to invert the selected state of members</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode">The selection mode determining how to test for intersection</param>
        /// <param name="affectedArea">The geographic region encapsulating the changed members</param>
        /// <returns>Boolean, true if members were changed by the selection process.</returns>
        bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea);

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as
        /// SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode">The selection mode</param>
        /// <param name="affectedArea">The envelope affected area</param>
        /// <returns>Boolean, true if any members were added to the selection</returns>
        bool Select(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea);

        /// <summary>
        /// Removes any members found in the specified region from the selection
        /// </summary>
        /// <param name="tolerant">The geographic region to investigate</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode">The selection mode to use for selecting items</param>
        /// <param name="affectedArea">The geographic region containing all the shapes that were altered</param>
        /// <returns>Boolean, true if any members were removed from the selection</returns>
        bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Boolean indicating whether this item is actively supporting selection
        /// </summary>
        bool SelectionEnabled { get; set; }

        #endregion
    }
}