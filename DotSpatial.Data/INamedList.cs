// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/29/2009 9:21:51 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// INamedList
    /// </summary>
    public interface INamedList
    {
        #region Methods

        /// <summary>
        /// Re-orders the list so that the index of the specifeid item is lower,
        /// and threfore will be drawn earlier, and therefore should appear
        /// in a lower position on the list.
        /// </summary>
        /// <param name="name">The name of the item to demote</param>
        void Demote(string name);

        /// <summary>
        /// Gets the item with the specified name as an object.
        /// This enables the INamedList to work with items even
        /// if it doesn't know the strong type.
        /// </summary>
        /// <param name="name">The string name of the item to retrieve</param>
        /// <returns>The actual item cast as an object.</returns>
        object GetItem(string name);

        /// <summary>
        /// Gets the name of the specified item, even if the strong type of the
        /// item is not known.
        /// </summary>
        /// <param name="item">The item to get the name of cast as an object</param>
        /// <returns>The string name of the specified object.</returns>
        string GetNameOfObject(object item);

        /// <summary>
        /// Gets the list of names for the items currently stored in the list,
        /// in the sequence defined by the list of items.
        /// </summary>
        string[] GetNames();

        /// <summary>
        /// Re-orders the list so that the index of the specified item is higher,
        /// and therefore will be drawn later, and therefore should appear
        /// in a higher position on the list.
        /// </summary>
        /// <param name="name"></param>
        void Promote(string name);

        /// <summary>
        /// Updates the names to match the current set of actual items.
        /// </summary>
        void RefreshNames();

        /// <summary>
        /// Removes the item with the specified name from the list.
        /// </summary>
        /// <param name="name">The string name of the item to remove</param>
        void Remove(string name);

        #endregion

        /// <summary>
        /// Gets or sets the base name to use for naming items
        /// </summary>
        string BaseName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the count of the items in the list.
        /// </summary>
        int Count
        {
            get;
        }
    }
}