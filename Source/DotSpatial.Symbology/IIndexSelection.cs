// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/5/2009 11:15:47 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for IndexSelection.
    /// </summary>
    public interface IIndexSelection : ISelection, ICollection<int>
    {
        #region Properties

        /// <summary>
        /// Gets the integer count of the members in the collection
        /// </summary>
        new int Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a range of indices all at once.
        /// </summary>
        /// <param name="indices">The indices to add</param>
        void AddRange(IEnumerable<int> indices);

        /// <summary>
        /// Clears the selection
        /// </summary>
        new void Clear();

        /// <summary>
        /// Removes a set of indices all at once.
        /// </summary>
        /// <param name="indices">Indices that get removed.</param>
        /// <returns>True, if the selection was changed.</returns>
        bool RemoveRange(IEnumerable<int> indices);

        #endregion
    }
}