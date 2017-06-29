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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 3:51:03 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Object given back when a ComboBox is added to the Toolbar.
    /// </summary>
    public interface IComboBoxItem
    {
        /// <summary>
        /// Gets/Sets the cursor
        /// </summary>
        Cursor Cursor { get; set; }

        /// <summary>
        /// Gets/Sets the description for the control (used when the user customizes the Toolbar)
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets/Sets the style of the combo box
        /// </summary>
        ComboBoxStyle DropDownStyle { get; set; }

        /// <summary>
        /// Gets/Sets the enabled state of the ComboBoxItem
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets the name of the ComboBoxItem object
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets/Sets the zero based index of the selected item
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Gets/Sets the selected object
        /// </summary>
        object SelectedItem { get; set; }

        /// <summary>
        /// Gets/Sets the selected item text
        /// </summary>
        string SelectedText { get; set; }

        /// <summary>
        /// Gets/Sets the length of the highlighted text
        /// </summary>
        int SelectionLength { get; set; }

        /// <summary>
        /// Gets/Sets the start index of the highlighted text
        /// </summary>
        int SelectionStart { get; set; }

        /// <summary>
        /// Gets/Sets the text for this object
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets/Sets the tooltip text for the control
        /// </summary>
        string Tooltip { get; set; }

        /// <summary>
        /// Gets/Sets the width of the control
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Returns a collection of items
        /// </summary>
        ComboBox.ObjectCollection Items();
    }
}