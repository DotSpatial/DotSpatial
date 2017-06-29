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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 10:54:37 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IScheme
    /// </summary>
    public interface IScheme : ILegendItem
    {
        #region Methods

        /// <summary>
        /// Adds a new scheme, assuming that the new scheme is the correct type.
        /// </summary>
        /// <param name="category">The category to add</param>
        void AddCategory(ICategory category);

        /// <summary>
        /// Applies the snapping rule directly to the categories, based on the most recently
        /// collected set of values, and the current VectorEditorSettings.
        /// </summary>
        void ApplySnapping(ICategory category);

        /// <summary>
        /// Creates the category using a random fill color
        /// </summary>
        /// <param name="fillColor">The base color to use for creating the category</param>
        /// <param name="size">For points this is the maximal point size, for lines this is the maximum line width</param>
        /// <returns>A new IFeatureCategory that matches the type of this scheme</returns>
        ICategory CreateNewCategory(Color fillColor, double size);

        /// <summary>
        /// Uses the settings on this scheme to create a random category.
        /// </summary>
        /// <returns>A new ICategory</returns>
        ICategory CreateRandomCategory();

        /// <summary>
        /// Reduces the index value of the specified category by 1 by
        /// exchaning it with the category before it.  If there is no
        /// category before it, then this does nothing.
        /// </summary>
        /// /// <param name="category">The category to decrease the index of</param>
        bool DecreaseCategoryIndex(ICategory category);

        /// <summary>
        /// Draws the regular symbolizer for the specified cateogry to the specified graphics
        /// surface in the specified bounding rectangle.
        /// </summary>
        /// <param name="index">The integer index of the feature to draw.</param>
        /// <param name="g">The Graphics object to draw to</param>
        /// <param name="bounds">The rectangular bounds to draw in</param>
        void DrawCategory(int index, Graphics g, Rectangle bounds);

        /// <summary>
        /// Re-orders the specified member by attempting to exchange it with the next higher
        /// index category.  If there is no higher index, this does nothing.
        /// </summary>
        ///  <param name="category">The category to increase the index of</param>
        bool IncreaseCategoryIndex(ICategory category);

        /// <summary>
        /// Inserts the category at the specified index
        /// </summary>
        /// <param name="index">The integer index where the category should be inserted</param>
        /// <param name="category">The category to insert</param>
        void InsertCategory(int index, ICategory category);

        /// <summary>
        /// Removes the specified category
        /// </summary>
        /// <param name="category">The category to insert</param>
        void RemoveCategory(ICategory category);

        /// <summary>
        /// Suspends the category events
        /// </summary>
        void SuspendEvents();

        /// <summary>
        /// Resumes the category events
        /// </summary>
        void ResumeEvents();

        /// <summary>
        /// Clears the categories
        /// </summary>
        void ClearCategories();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the editor settings that control how this scheme operates.
        /// </summary>
        EditorSettings EditorSettings
        {
            get;
            set;
        }

        /// <summary>
        /// This is cached until a GetValues call is made, at which time the statistics will
        /// be re-calculated from the values.
        /// </summary>
        Statistics Statistics
        {
            get;
        }

        /// <summary>
        /// Gets the current list of values calculated in the case of numeric breaks.
        /// This includes only members that are not excluded by the exclude expression,
        /// and have a valid numeric value.
        /// </summary>
        List<double> Values
        {
            get;
        }

        #endregion
    }
}