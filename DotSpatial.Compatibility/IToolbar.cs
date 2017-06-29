// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:54:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Used to add/remove Toolbars, buttons, combo boxes, etc. to/from the application
    /// </summary>
    public interface IToolbar
    {
        /// <summary>
        /// Adds a Toolbar group to the Main Toolbar
        /// </summary>
        /// <param name="name">The name to give to the Toolbar item</param>
        /// <returns>true on success, false on failure</returns>
        bool AddToolbar(string name);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton</param>
        IToolbarButton AddButton(string name);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton</param>
        /// <param name="isDropDown">Should this button support drop-down items?</param>
        IToolbarButton AddButton(string name, bool isDropDown);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton</param>
        /// /// <param name="toolbar">The name of the Toolbar to which this ToolbarButton should belong (if null or empty, then the default Toolbar will be used</param>
        /// <param name="isDropDown">Should this button support drop-down items?</param>
        IToolbarButton AddButton(string name, string toolbar, bool isDropDown);

        /// <summary>
        /// Adds a separator to a toolstrip dropdown button.
        /// </summary>
        /// <param name="name">The name to give to the new separator.</param>
        /// <param name="toolbar">The name of the Toolbar to which this separator should belong (if null or empty, then the default Toolbar will be used.</param>
        /// <param name="parentButton">The name of the ToolbarButton to which this new separator should be added as a subitem.</param>
        void AddButtonDropDownSeparator(string name, string toolbar, string parentButton);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton</param>
        /// <param name="picture">The Icon/Bitmap to use as a picture on the ToolbarButton face</param>
        IToolbarButton AddButton(string name, object picture);

        /// <summary>
        /// Adds a button to a specified to the default Toolbar
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton</param>
        /// <param name="picture">The Icon/Bitmap to use as a picture on the ToolbarButton face</param>
        /// <param name="text">The text name for the ToolbarButton.  This is the text the user will see if customizing the Toolbar</param>
        IToolbarButton AddButton(string name, object picture, string text);

        /// <summary>
        /// Adds a button to a specified to the specified Toolbar.
        /// </summary>
        /// <param name="name">The name to give to the new ToolbarButton.</param>
        /// <param name="toolbar">The name of the Toolbar to which this ToolbarButton should belong (if null or empty, then the default Toolbar will be used.</param>
        /// <param name="parentButton">The name of the ToolbarButton to which this new ToolbarButton should be added as a subitem.</param>
        /// <param name="after">The name of the ToolbarButton after which this new ToolbarButton should be added.</param>
        IToolbarButton AddButton(string name, string toolbar, string parentButton, string after);

        /// <summary>
        /// Adds a ComboBoxItem to a specified to the default Toolbar.
        /// </summary>
        /// <param name="name">The name to give to the new ComboBoxItem.</param>
        /// <param name="toolbar">The name of the Toolbar to which this ToolbarButton should belong.</param>
        /// <param name="after">The name of the ToolbarButton/ComboBoxItem afterwhich this new item should be added.</param>
        IComboBoxItem AddComboBox(string name, string toolbar, string after);

        /// <summary>
        /// returns the specified ToolbarButton (null on failure)
        /// </summary>
        /// <param name="name">The name of the ToolbarButton to retrieve</param>
        IToolbarButton ButtonItem(string name);

        /// <summary>
        /// returns the specified ComboBoxItem
        /// </summary>
        /// <param name="name">Name of the item to retrieve</param>
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
        /// <returns>true on success, false on failure (i.e. bad toolbar button name)</returns>
        bool PressToolbarButton(string name);

        /// <summary>
        /// Removes the specified Toolbar and any ToolbarButton/ComboBoxItems contained within the control
        /// </summary>
        /// <param name="name">The name of the Toolbar to be removed</param>
        /// <returns>true on success, false on failure</returns>
        bool RemoveToolbar(string name);

        /// <summary>
        /// Removes the specified ToolbarButton item
        /// </summary>
        /// <param name="name">The name of the ToolbarButton to be removed</param>
        /// <returns>true on success, false on failure</returns>
        bool RemoveButton(string name);

        /// <summary>
        /// Removes the specified ComboBoxItem
        /// </summary>
        /// <param name="name">The name of the ComboBoxItem to be removed</param>
        /// <returns>true on success, false on failure</returns>
        bool RemoveComboBox(string name);
    }
}