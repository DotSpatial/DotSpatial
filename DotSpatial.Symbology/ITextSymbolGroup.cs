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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/18/2008 9:25:46 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ITextSymbolGroup
    /// </summary>
    [Obsolete("Do not use it. This interface is not used in DotSpatial anymore.")] // Marked in 1.7
    public interface ITextSymbolGroup : ILegendItem
    {
        #region Methods

        /// <summary>
        /// Add a new integer into this group.
        /// </summary>
        /// <param name="label">The label to be added to the group.</param>
        /// <returns>Boolean, false if the integer was already in this group.</returns>
        bool Add(ILabel label);

        /// <summary>
        /// Removes all the selected indices and returns them to the regular indices.
        /// This returns a list of integer indices for the labels that were changed.
        /// </summary>
        IList<int> Clear();

        /// <summary>
        /// Selects a single label
        /// </summary>
        /// <param name="label">The integer label to select</param>
        void Select(int label);

        /// <summary>
        /// Unselect a single label in this group.
        /// </summary>
        /// <param name="label">The integer label</param>
        void UnSelect(int label);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the text symbolizer for this group of labels
        /// </summary>
        ILabelSymbolizer Symbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of regular labels based on their index in the actual list of labels
        /// </summary>
        IList<int> RegularLabels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of selected labels based on their index in the actual list of labels
        /// </summary>
        IList<int> SelectedLabels
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the set of symbolic characteristics that should be used for drawing selected labels.
        /// </summary>
        ILabelSymbolizer SelectionSymbolizer
        {
            get;
            set;
        }

        #endregion
    }
}