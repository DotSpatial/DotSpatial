// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/25/2010 3:28:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// Kyle Ellison 11/02/2010 Draw map background using map control BackColor
//
// ********************************************************************************************************

namespace DotSpatial.Projections.Forms
{
    /// <summary>
    /// The Undefined Projection Action enumeration
    /// </summary>
    public enum UndefinedProjectionAction
    {
        /// <summary>
        /// No action should be taken.
        /// </summary>
        Nothing,

        /// <summary>
        /// Always assume an undefined projection is Latitude Longitude
        /// </summary>
        WGS84,

        /// <summary>
        /// Always rely on the existing Map projection
        /// </summary>
        Map,

        /// <summary>
        /// Use a projection that was specified from the list
        /// </summary>
        Chosen
    }
}