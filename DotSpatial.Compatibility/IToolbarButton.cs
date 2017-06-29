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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:55:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Object given back when a button is added to a Toolbar.  This
    /// object can then be used to manipulate (change properties) for the button.
    /// </summary>
    public interface IToolbarButton
    {
        #region Properties

        /// <summary>
        /// Gets/Sets a flag indicating that this item starts a new group by drawing a seperator line if necessary
        /// </summary>
        bool BeginsGroup { get; set; }

        /// <summary>
        /// Gets/Sets the Category for this ToolbarButton item (used when user is customizing the Toolbar)
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// Gets/Sets the Cursor for this control
        /// </summary>
        Cursor Cursor { get; set; }

        /// <summary>
        /// Gets/Sets the description for the control (used when the user customizes the Toolbar)
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets/Sets a flag marking whether or not this ToolbarButton is displayed
        /// </summary>
        bool Displayed { get; set; }

        /// <summary>
        /// Gets/Sets the enabled state
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets the name of the ToolbarButton item
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the number of subitems
        /// </summary>
        int NumSubItems { get; }

        /// <summary>
        /// Gets/Sets the picture for the ToolbarButton
        /// </summary>
        object Picture { get; set; }

        /// <summary>
        /// Gets/Sets the pressed state of the ToolbarButton
        /// </summary>
        bool Pressed { get; set; }

        /// <summary>
        /// Gets/Sets the Text/Caption of the ToolbarButton
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets/Sets the tooltip text for the control
        /// </summary>
        string Tooltip { get; set; }

        /// <summary>
        /// Gets/Sets the visibility of the ToolbarButton
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Returns the Subitem with the specified zero-based index (null if out of range)
        /// </summary>
        IToolbarButton SubItem(int index);

        /// <summary>
        /// returns the Subitem with the specified name (null if it doesn't exist)
        /// </summary>
        IToolbarButton SubItem(string name);

        #endregion
    }
}