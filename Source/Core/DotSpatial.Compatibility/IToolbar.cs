// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Used to add/remove Toolbars, buttons, combo boxes, etc. to/from the application.
    /// </summary>
    public interface IToolbar
    {
        #region Methods

        /// <summary>
        /// Adds a button to a specified to the default Toolbar.
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton.</param>
        /// <returns>The specified ToolbarButton.</returns>
        IToolbarButton AddButton(string name);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar.
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton.</param>
        /// <param name="isDropDown">Should this button support drop-down items?.</param>
        /// <returns>The specified ToolbarButton.</returns>
        IToolbarButton AddButton(string name, bool isDropDown);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar.
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton.</param>
        /// /// <param name="toolbar">The name of the Toolbar to which this ToolbarButton should belong (if null or empty, then the default Toolbar will be used.</param>
        /// <param name="isDropDown">Should this button support drop-down items?.</param>
        /// <returns>The specified ToolbarButton.</returns>
        IToolbarButton AddButton(string name, string toolbar, bool isDropDown);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar.
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton.</param>
        /// <param name="picture">The Icon/Bitmap to use as a picture on the ToolbarButton face.</param>
        /// <returns>The specified ToolbarButton.</returns>
        IToolbarButton AddButton(string name, object picture);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar.
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton.</param>
        /// <param name="picture">The Icon/Bitmap to use as a picture on the ToolbarButton face.</param>
        /// <param name="text">The text name for the ToolbarButton.  This is the text the user will see if customizing the Toolbar.</param>
        /// <returns>The specified ToolbarButton.</returns>
        IToolbarButton AddButton(string name, object picture, string text);

        /// <summary>
        /// Adds a button to a specified to the specified Toolbar.
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton.</param>
        /// <param name="toolbar">The name of the Toolbar to which this ToolbarButton should belong (if null or empty, then the default Toolbar will be used.</param>
        /// <param name="parentButton">The name of the ToolbarButton to which this new ToolbarButton should be added as a subitem.</param>
        /// <param name="after">The name of the ToolbarButton after which this new ToolbarButton should be added.</param>
        /// <returns>The specified ToolbarButton.</returns>
        IToolbarButton AddButton(string name, string toolbar, string parentButton, string after);

        /// <summary>
        /// Adds a separator to a toolstrip dropdown button.
        /// </summary>
        /// <param name="name">The name to give to the new separator.</param>
        /// <param name="toolbar">The name of the Toolbar to which this separator should belong (if null or empty, then the default Toolbar will be used.</param>
        /// <param name="parentButton">The name of the ToolbarButton to which this new separator should be added as a subitem.</param>
        void AddButtonDropDownSeparator(string name, string toolbar, string parentButton);

        /// <summary>
        /// Adds a ComboBoxItem to a specified to the default Toolbar.
        /// </summary>
        /// <param name="name">The name to give to the new ComboBoxItem.</param>
        /// <param name="toolbar">The name of the Toolbar to which this ToolbarButton should belong.</param>
        /// <param name="after">The name of the ToolbarButton/ComboBoxItem afterwhich this new item should be added.</param>
        /// <returns>The specified ComboBoxItem.</returns>
        IComboBoxItem AddComboBox(string name, string toolbar, string after);

        /// <summary>
        /// Adds a Toolbar group to the Main Toolbar.
        /// </summary>
        /// <param name="name">The name to give to the Toolbar item.</param>
        /// <returns>true on success, false on failure.</returns>
        bool AddToolbar(string name);

        /// <summary>
        /// returns the specified ToolbarButton (null on failure).
        /// </summary>
        /// <param name="name">The name of the ToolbarButton to retrieve.</param>
        /// <returns>The specified ToolbarButton.</returns>
        IToolbarButton ButtonItem(string name);

        /// <summary>
        /// returns the specified ComboBoxItem.
        /// </summary>
        /// <param name="name">Name of the item to retrieve.</param>
        /// <returns>The specified ComboBoxItem.</returns>
        IComboBoxItem ComboBoxItem(string name);

        /// <summary>
        /// Returns the number of buttons on the specified toolbar, or 0 if the toolbar can't be found.
        /// </summary>
        /// <param name="toolbarName">The name of the toolbar.</param>
        /// <returns>The number of buttons on the toolbar.</returns>
        int NumToolbarButtons(string toolbarName);

        /// <summary>
        /// Presses the specified ToolBar button (if it's enabled) as if a user
        /// had pressed it.
        /// </summary>
        /// <param name="name">The name of the toolbar button to press.</param>
        /// <returns>true on success, false on failure (i.e. bad toolbar button name).</returns>
        bool PressToolbarButton(string name);

        /// <summary>
        /// Removes the specified ToolbarButton item.
        /// </summary>
        /// <param name="name">The name of the ToolbarButton to be removed.</param>
        /// <returns>true on success, false on failure.</returns>
        bool RemoveButton(string name);

        /// <summary>
        /// Removes the specified ComboBoxItem.
        /// </summary>
        /// <param name="name">The name of the ComboBoxItem to be removed.</param>
        /// <returns>true on success, false on failure.</returns>
        bool RemoveComboBox(string name);

        /// <summary>
        /// Removes the specified Toolbar and any ToolbarButton/ComboBoxItems contained within the control.
        /// </summary>
        /// <param name="name">The name of the Toolbar to be removed.</param>
        /// <returns>true on success, false on failure.</returns>
        bool RemoveToolbar(string name);

        #endregion
    }
}