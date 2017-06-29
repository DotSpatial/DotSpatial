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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2009 4:16:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IFilterCollection
    /// </summary>
    public interface IFeatureSelection : ICollection<IFeature>, ISelection
    {
        /// <summary>
        /// Gets the integer count of the members in the collection
        /// </summary>
        new int Count
        {
            get;
        }

        /// <summary>
        /// Gets the drawing filter used by this collection.
        /// </summary>
        IDrawingFilter Filter
        {
            get;
        }

        /// <summary>
        /// Clears the selection
        /// </summary>
        new void Clear();
    }
}