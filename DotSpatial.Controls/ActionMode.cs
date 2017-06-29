// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
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
// The Initial Developer of this Original Code is Keenedge Created 2010-10-22 09:17:34 -0700
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Renamed and changed from "Flags" characteristic to normal enum.  These states do not combine. 11/25/2010
// ********************************************************************************************************

namespace DotSpatial.Controls
{
    /// <summary>
    /// Describe different behaviors that map functions can have when working with
    /// respect to other map-functions.
    /// </summary>
    public enum ActionMode
    {
        /// <summary>
        /// Prompt the user to decide if Layers should be reprojected
        /// </summary>
        Prompt,

        /// <summary>
        /// Always reproject layers to match the MapFrame projection
        /// </summary>
        Always,

        /// <summary>
        /// Never reproject layers to match the MapFrame projection
        /// </summary>
        Never,

        /// <summary>
        /// Prompt once and accept that answer for all future layers added to the map frame.
        /// </summary>
        PromptOnce,
    }
}