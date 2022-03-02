// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for label scheme.
    /// </summary>
    public interface ILabelScheme : ICloneable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list of categories that make up this label scheme.
        /// </summary>
        IList<ILabelCategory> Categories { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a category to the scheme, and also names the category with an integer that has not yet been used.
        /// </summary>
        /// <returns>A new category with a name that has not yet been used.</returns>
        ILabelCategory AddCategory();

        /// <summary>
        /// returns a duplicate scheme, where the categories are copied, but
        /// the underlying featureset is not duplicated.
        /// </summary>
        /// <returns>A copy of this label scheme.</returns>
        ILabelScheme Copy();

        /// <summary>
        /// Attempts to reduce the integer index representing this categories rank in the
        /// list. By doing this, it will be drawn sooner, and therefore subsequent
        /// layers will be drawn on top of this layer, and so it reduces the categories
        /// priority. If this collection does not contain the category or it is already
        /// at index 0, this will return false.
        /// </summary>
        /// <param name="category">The ILabelCategory to demote.</param>
        /// <returns>Boolean, true if the demotion was successful.</returns>
        bool Demote(ILabelCategory category);

        /// <summary>
        /// This attempts to increase the numeric index, which will cause it to be drawn later,
        /// or higher up on the cue, which means it will be drawn AFTER the previous layers,
        /// and therefore is a higher priority. If the category does not exist in the collection
        /// or the category is already at the highest value, this returns false.
        /// </summary>
        /// <param name="category">The category to promote if possible.</param>
        /// <returns>Boolean, true if the promotion was successful.</returns>
        bool Promote(ILabelCategory category);

        #endregion
    }
}