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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/21/2009 1:45:12 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ILabelScheme
    /// </summary>
    public interface ILabelScheme : ICloneable
    {
        #region Methods

        /// <summary>
        /// Adds a category to the scheme, and also names the category with an integer that has not yet been used.
        /// </summary>
        /// <returns>A new category with a name that has not yet been used</returns>
        ILabelCategory AddCategory();

        /// <summary>
        /// returns a duplicate scheme, where the categories are copied, but
        /// the underlying featureset is not duplicated.
        /// </summary>
        /// <returns>A copy of this label scheme</returns>
        ILabelScheme Copy();

        /// <summary>
        /// Attempts to reduce the integer index representing this categories rank in the
        /// list.  By doing this, it will be drawn sooner, and therefore subsequent
        /// layers will be drawn on top of this layer, and so it reduces the categories
        /// priority.  If this collection does not contain the category or it is already
        /// at index 0, this will return false.
        /// </summary>
        /// <param name="category">The ILabelCategory to demote</param>
        /// <returns>Boolean, true if the demotion was successful</returns>
        bool Demote(ILabelCategory category);

        /// <summary>
        /// This attempts to increase the numeric index, which will cause it to be drawn later,
        /// or higher up on the cue, which means it will be drawn AFTER the previous layers,
        /// and therefore is a higher priority.  If the category does not exist in the collection
        /// or the category is already at the highest value, this returns false.
        /// </summary>
        /// <param name="category">The category to promote if possible.</param>
        /// <returns>Boolean, true if the promotion was successful</returns>
        bool Promote(ILabelCategory category);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of categories that make up this label scheme
        /// </summary>
        IList<ILabelCategory> Categories
        {
            get;
            set;
        }

        #endregion
    }
}