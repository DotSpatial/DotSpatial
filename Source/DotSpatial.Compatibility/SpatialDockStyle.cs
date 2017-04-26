// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:42:09 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Docking styles for DotSpatial UIPanels
    /// </summary>
    public enum SpatialDockStyle
    {
        /// <summary>Floating</summary>
        None = -1,

        /// <summary>Dock Left</summary>
        Left = 0,

        /// <summary>Dock Right</summary>
        Right = 1,

        /// <summary>Dock Top</summary>
        Top = 2,

        /// <summary>Dock Bottom</summary>
        Bottom = 3,

        /// <summary>Dock Left Autohidden</summary>
        LeftAutoHide = 4,

        /// <summary>Dock Right Autohidden</summary>
        RightAutoHide = 5,

        /// <summary>Dock Top Autohidden</summary>
        TopAutoHide = 6,

        /// <summary>Dock Bottom Autohidden</summary>
        BottomAutoHide = 7
    }
}