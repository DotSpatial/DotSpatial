// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2008 8:25:39 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Data
{
    public interface IChangeEventList<T> : IList<T>, ISuspendEvents, IChangeItem where T : IChangeItem
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end of the System.Collections.Generic.List&lt;T&gt;
        /// </summary>
        /// <param name="collection">collection: The collection whose elements should be added to the end of the
        /// System.Collections.Generic.List&lt;T&gt;. The collection itself cannot be null, but it can contain
        /// elements that are null,
        /// if type T is a reference type.</param>
        /// <exception cref="System.ApplicationException">Unable to add while the ReadOnly property is set to
        ///  true.</exception>
        void AddRange(IEnumerable<T> collection);
    }
}