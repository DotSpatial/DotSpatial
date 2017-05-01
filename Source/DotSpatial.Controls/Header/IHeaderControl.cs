// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in May, 2011.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
//  Yang  Cao         | 05/16/2011         |  Create the interface IHeaderControl to work with standard toolbar and ribbon control
// ********************************************************************************************************

using System;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// By using this interface, the developers can create a button, menu, ribbon tab. etc
    /// without considering whether the user interface is ribbon style or standard toolbar
    /// style
    /// </summary>
    public interface IHeaderControl
    {
        #region Events

        /// <summary>
        /// This event occurs when a root item is selected
        /// </summary>
        event EventHandler<RootItemEventArgs> RootItemSelected;

        #endregion

        #region Methods

        /// <summary>
        /// This will add a new item that will appear on the standard toolbar or ribbon control.
        /// </summary>
        /// <returns>Added GUI item</returns>
        object Add(HeaderItem item);

        /// <summary>
        /// Remove item from the standard toolbar or ribbon control
        /// </summary>
        /// <param name="key">The string itemName to remove from the standard toolbar or ribbon control</param>
        void Remove(string key);

        /// <summary>
        /// Removes all items the plugin created.
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Selects the root, making it the active root.
        /// </summary>
        /// <param name="key">The key.</param>
        void SelectRoot(string key);

        #endregion
    }
}