// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Object given back when a button is added to a Toolbar. This
    /// object can then be used to manipulate (change properties) for the button.
    /// </summary>
    public interface IToolbarButton
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this item starts a new group by drawing a seperator line if necessary.
        /// </summary>
        bool BeginsGroup { get; set; }

        /// <summary>
        /// Gets or sets the Category for this ToolbarButton item (used when user is customizing the Toolbar).
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// Gets or sets the Cursor for this control.
        /// </summary>
        Cursor Cursor { get; set; }

        /// <summary>
        /// Gets or sets the description for the control (used when the user customizes the Toolbar).
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ToolbarButton is displayed.
        /// </summary>
        bool Displayed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ToolbarButton is enabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets the name of the ToolbarButton item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the number of subitems.
        /// </summary>
        int NumSubItems { get; }

        /// <summary>
        /// Gets or sets the picture for the ToolbarButton.
        /// </summary>
        object Picture { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ToolbarButton is pressed.
        /// </summary>
        bool Pressed { get; set; }

        /// <summary>
        /// Gets or sets the Text/Caption of the ToolbarButton.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the tooltip text for the control.
        /// </summary>
        string Tooltip { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ToolbarButton is visible.
        /// </summary>
        bool Visible { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the Subitem with the specified zero-based index (null if out of range).
        /// </summary>
        /// <param name="index">Index of the ToolbarButton that should be returned.</param>
        /// <returns>The specified ToolbarButton.</returns>
        IToolbarButton SubItem(int index);

        /// <summary>
        /// returns the Subitem with the specified name (null if it doesn't exist).
        /// </summary>
        /// <param name="name">Name of the ToolbarButton that should be returned.</param>
        /// <returns>The specified ToolbarButton.</returns>
        IToolbarButton SubItem(string name);

        #endregion
    }
}