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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/20/2009 1:58:46 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IFeatureCategory
    /// </summary>
    public interface IFeatureCategory : ICategory
    {
        /// <summary>
        /// Occurs when the select features context menu is clicked.
        /// </summary>
        event EventHandler<ExpressionEventArgs> SelectFeatures;

        /// <summary>
        /// Occurs when the deselect features context menu is clicked.
        /// </summary>
        event EventHandler<ExpressionEventArgs> DeselectFeatures;

        #region Methods

        /// <summary>
        /// Queries this layer and the entire parental tree up to the map frame to determine if
        /// this layer is within the selected layers.
        /// </summary>
        bool IsWithinLegendSelection();

        /// <summary>
        /// This gets a single color that attempts to represent the specified
        /// category.  For polygons, for example, this is the fill color (or central fill color)
        /// of the top pattern.  If an image is being used, the color will be gray.
        /// </summary>
        /// <returns>The System.Color that can be used as an approximation to represent this category.</returns>
        Color GetColor();

        /// <summary>
        /// This applies the color to the top symbol stroke or pattern.
        /// </summary>
        /// <param name="color">The Color to apply</param>
        void SetColor(Color color);

        /// <summary>
        /// In some cases, it is useful to simply be able to show an approximation of the actual expression.
        /// This also removes brackets from the field names to make it slightly cleaner.
        /// </summary>
        void DisplayExpression();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer used for this category.
        /// </summary>
        IFeatureSymbolizer Symbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the symbolizer used for this category
        /// </summary>
        IFeatureSymbolizer SelectionSymbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or set the filter expression that is used to add members to generate a category based on this scheme.
        /// </summary>
        string FilterExpression
        {
            get;
            set;
        }

        #endregion
    }
}