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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:52:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// The object given back when a panel is added to the status bar.  This object can
    /// be used to
    /// </summary>
    public interface IStatusBarItem
    {
        /// <summary>
        /// Gets/Sets the Alignment of the text
        /// </summary>
        HJustification Alignment { get; set; }

        /// <summary>
        /// Gets/Sets whether or not this StatusBarItem should auto size itself
        /// </summary>
        bool AutoSize { get; set; }

        /// <summary>
        /// Gets/Sets the minimum allowed width for this StatusBarItem
        /// </summary>
        int MinWidth { get; set; }

        /// <summary>
        /// Gets/Sets the Text within the StatusBarItem
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets/Sets the width of the StatusBarItem
        /// </summary>
        int Width { get; set; }
    }
}