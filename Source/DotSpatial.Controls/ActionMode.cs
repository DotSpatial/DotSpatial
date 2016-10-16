// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
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