// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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