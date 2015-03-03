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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/21/2009 1:36:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ILabelCategory
    /// </summary>
    public interface ILabelCategory : ICloneable
    {
        #region Methods

        /// <summary>
        /// Returns a shallow copy of this object cast as a LabelCategory.
        /// </summary>
        /// <returns>A shallow copy of this object.</returns>
        ILabelCategory Copy();

        /// <summary>
        /// Calculates the expression for the given row. The Expression.Columns have to be in sync with the Features columns for calculation to work without error.
        /// </summary>
        /// <param name="row">Datarow the expression gets calculated for.</param>
        /// <param name="selected">Indicates whether the feature is selected.</param>
        /// <returns>null if there was an error while parsing the expression, else the calculated expression</returns>
        string CalculateExpression(DataRow row, bool selected);

        /// <summary>
        /// Updates the Expression-Object with the columns that exist inside the features that belong to this category. The will be used for calculating the expression.
        /// </summary>
        /// <param name="columns">Columns that should be updated.</param>
        /// <returns>False if columns were not set.</returns>
        bool UpdateExpressionColumns(DataColumnCollection columns);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string expression that controls the integration of field values into the label text. 
        /// This is the raw text that is used to do calculations and concat fields and strings.
        /// </summary>
        string Expression
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string filter expression that controls which features
        /// that this should apply itself to.
        /// </summary>
        string FilterExpression
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the string name
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text symbolizer to use for this category
        /// </summary>
        ILabelSymbolizer Symbolizer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text symbolizer to use for this category
        /// </summary>
        ILabelSymbolizer SelectionSymbolizer
        {
            get;
            set;
        }

        #endregion
    }
}