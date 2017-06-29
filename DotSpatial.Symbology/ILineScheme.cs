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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/21/2009 9:57:24 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.ComponentModel;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IPointScheme
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface ILineScheme : IFeatureScheme
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list of scheme categories belonging to this scheme.
        /// </summary>
        LineCategoryCollection Categories
        {
            get;
            set;
        }

        #endregion
    }
}