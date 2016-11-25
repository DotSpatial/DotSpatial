// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 3:55:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// MapCursorModes
    /// </summary>
    /// <item>AppStarting</item>
    /// <item>Arrow</item>
    /// <item>Cross</item>
    /// <item>Help</item>
    /// <item>IBeam</item>
    /// <item>MapDefault</item>
    /// <item>No</item>
    /// <item>SizeAll</item>
    /// <item>SizeNESW</item>
    /// <item>SizeNS</item>
    /// <item>SizeNWSE</item>
    /// <item>SizeWE</item>
    /// <item>UpArrow</item>
    /// <item>UserDefined</item>
    /// <item>Wait</item>
    public enum MapCursorMode
    {
        /// <summary>
        /// App starting icon
        /// </summary>
        AppStarting,
        /// <summary>
        /// typical white arrow with black border
        /// </summary>
        Arrow,
        /// <summary>
        /// A right angle cross
        /// </summary>
        Cross,
        /// <summary>
        /// Question mark
        /// </summary>
        Help,
        /// <summary>
        /// An IBeam like for editing text
        /// </summary>
        IBeam,
        /// <summary>
        /// An arrow
        /// </summary>
        MapDefault,
        /// <summary>
        /// A circle with a diagonal line
        /// </summary>
        No,
        /// <summary>
        /// Arrows pointing north, south, east and west
        /// </summary>
        SizeAll,
        /// <summary>
        /// A diagonal line with arrows pointing northeast and southwest
        /// </summary>
        SizeNESW,
        /// <summary>
        /// A vertical line with arrows up and down
        /// </summary>
        SizeNS,
        /// <summary>
        /// A diagonal line with arrows pointing northwest and southeast
        /// </summary>
        SizeNWSE,
        /// <summary>
        /// A horizontal line with arrows pointing west and east
        /// </summary>
        SizeWE,
        /// <summary>
        /// An up pointed arrow
        /// </summary>
        UpArrow,
        /// <summary>
        /// Use a custom icon
        /// </summary>
        UserDefined,
        /// <summary>
        /// Windows wait cursor (varies from system to system)
        /// </summary>
        Wait
    }
}